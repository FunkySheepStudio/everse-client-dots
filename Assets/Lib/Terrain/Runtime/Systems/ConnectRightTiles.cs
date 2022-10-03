using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using FunkySheep.Geometry;
using UnityEngine;
using FunkySheep.Maps;

namespace FunkySheep.Terrain
{
    public partial class ConnectRightTiles : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float tileSize = GetSingleton<MapSingletonComponent>().tileSize;

            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            NativeArray<Entity> leftEntities = GetEntityQuery(typeof(LeftConnectComponentTag), typeof(TileComponent), typeof(TileMapGeneratedComponentTag)).ToEntityArray(Allocator.TempJob);

            Entities.ForEach((Entity entity, int entityInQueryIndex, in RightConnectComponentTag rightConnectComponentTag, in TileComponent tileComponent, in TileMapGeneratedComponentTag tileMapGeneratedComponentTag) => {
                DynamicBuffer<Vertex> vertices = GetBuffer<Vertex>(entity);
                DynamicBuffer<Triangle> triangles = GetBuffer<Triangle>(entity);
                DynamicBuffer<Uv> uvs = GetBuffer<Uv>(entity);

                for (int i = 0; i < leftEntities.Length; i++)
                {
                    int2 rightGridPosition = GetComponent<TileComponent>(leftEntities[i]).gridPosition;
                    int2 rightTile = tileComponent.gridPosition;
                    rightTile.x += 1;

                    if (rightGridPosition.Equals(rightTile))
                    {
                        DynamicBuffer<Vertex> rightVertices = GetBuffer<Vertex>(leftEntities[i]);
                        int count = 256;

                        for (int j = 256; j < rightVertices.Length; j+= count)
                        {
                            vertices.Add(
                                new Vertex
                                {
                                    Value =
                                    {
                                        x = tileSize + tileComponent.step,
                                        y = rightVertices[j].Value.y,
                                        z = rightVertices[j].Value.z //Shift to the world position
                                    }
                                }
                               );


                            int index = j - 1;

                            // Uvs
                            uvs.Add(new Uv
                            {
                                Value = new float3(
                                    count - 1,
                                    count - 1,
                                    0
                                )
                            });

                            if (j != (rightVertices.Length - count))
                            {
                                // First triangle
                                triangles.Add(new Triangle
                                {
                                    Value = index
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = index + count
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = vertices.Length
                                });


                                // Second triangle
                                triangles.Add(new Triangle
                                {
                                    Value = index
                                });


                                triangles.Add(new Triangle
                                {
                                    Value = vertices.Length
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = vertices.Length -1
                                });
                            }
                        }

                        ecb.AddComponent<MeshUpdateTag>(entityInQueryIndex, entity);
                        ecb.RemoveComponent<RightConnectComponentTag>(entityInQueryIndex, entity);
                        ecb.RemoveComponent<LeftConnectComponentTag>(entityInQueryIndex, leftEntities[i]);
                    }
                }
            })
            .WithDisposeOnCompletion(leftEntities)
            .Schedule();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}
