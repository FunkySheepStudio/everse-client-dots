using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using FunkySheep.Geometry;
using UnityEngine;
using FunkySheep.Maps;

namespace FunkySheep.Terrain
{
    public partial class ConnectTopTiles : SystemBase
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
                        int count = 256;

                        for (int j = 0; j < count; j++)
                        {
                            vertices.Add(
                                new Vertex
                                {
                                    Value =
                                    {
                                        x = bottomVertices[j].Value.x,
                                        y = bottomVertices[j].Value.y,
                                        z = tileSize + tileComponent.step //Shift to the world position
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
                                    Value = i
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = i + 1 + count
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = i + 1
                                });


                                // Second triangle
                                triangles.Add(new Triangle
                                {
                                    Value = i
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = i + count
                                });

                                triangles.Add(new Triangle
                                {
                                    Value = i + 1 + count
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
