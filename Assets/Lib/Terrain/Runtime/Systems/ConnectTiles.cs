using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using FunkySheep.Geometry;
using UnityEngine;
using FunkySheep.Maps;

namespace FunkySheep.Terrain
{
    public partial class ConnectTiles : SystemBase
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

            NativeArray<Entity> bottomEntities = GetEntityQuery(typeof(BottomConnectComponentTag), typeof(TileComponent), typeof(TileMapGeneratedComponentTag)).ToEntityArray(Allocator.TempJob);

            Entities.ForEach((Entity entity, int entityInQueryIndex, in TopConnectComponentTag topConnectComponentTag, in TileComponent tileComponent, in TileMapGeneratedComponentTag tileMapGeneratedComponentTag) => {
                DynamicBuffer<Vertex> vertices = GetBuffer<Vertex>(entity);
                DynamicBuffer<Triangle> triangles = GetBuffer<Triangle>(entity);
                DynamicBuffer<Uv> uvs = GetBuffer<Uv>(entity);

                for (int i = 0; i < bottomEntities.Length; i++)
                {
                    int2 bottomGridPosition = GetComponent<TileComponent>(bottomEntities[i]).gridPosition;
                    int2 bottomTile = tileComponent.gridPosition;
                    bottomTile.y += 1;

                    if (bottomGridPosition.Equals(bottomTile))
                    {
                        DynamicBuffer<Vertex> bottomVertices = GetBuffer<Vertex>(bottomEntities[i]);
                        int count = (int)(Mathf.Sqrt(bottomVertices.Length));

                        for (int j = 0; j < count; j++)
                        {
                            vertices.Add(
                                new Vertex
                                {
                                    Value =
                                    {
                                        x = bottomVertices[j].Value.x,
                                        y = bottomVertices[j].Value.y,
                                        z = bottomVertices[j].Value.z + bottomTile.y * tileSize //Shift to the world position
                                    }
                                }
                               );


                            int index = vertices.Length - 257;

                            // Uvs
                            uvs.Add(new Uv
                            {
                                Value = new float3(
                                    count - 1,
                                    count - 1,
                                    0
                                )
                            });

                            if (j != (count - 1))
                            {
                                // First triangle
                                triangles.Add(new Triangle
                                {
                                    Value = index
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = index + 1
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = index + 1 + count
                                });


                                // Second triangle
                                triangles.Add(new Triangle
                                {
                                    Value = index
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = index + 1 + count
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = index + count
                                });
                            }
                        }

                        ecb.AddComponent<MeshUpdateTag>(entityInQueryIndex, entity);
                        ecb.RemoveComponent<TopConnectComponentTag>(entityInQueryIndex, entity);
                        ecb.RemoveComponent<BottomConnectComponentTag>(entityInQueryIndex, bottomEntities[i]);
                    }
                }
            })
            .WithDisposeOnCompletion(bottomEntities)
            .Schedule();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}
