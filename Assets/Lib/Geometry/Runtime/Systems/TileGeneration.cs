using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace FunkySheep.Geometry
{
    public partial class TileGeneration : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, in TileComponent tileComponent, in DynamicBuffer<TileDataComponent> tileDataComponent, in TileMapUpdateComponentTag tileMapUpdateComponentTag) =>
            {
                DynamicBuffer<Vertex> vertices = GetBuffer<Vertex>(entity);
                DynamicBuffer<Triangle> triangles = GetBuffer<Triangle>(entity);
                DynamicBuffer<Uv> uvs = GetBuffer<Uv>(entity);

                for (int i = 0; i < tileDataComponent.Length; i++)
                {
                    int count = (int)Mathf.Sqrt(tileDataComponent.Length);
                    float x = tileDataComponent[i].Value.x;
                    float y = tileDataComponent[i].Value.y;
                    float z = tileDataComponent[i].Value.z;

                    // indexes
                    vertices.Add(new Vertex { Value = new float3 {
                        x = tileDataComponent[i].Value.x * tileComponent.step,
                        y = tileDataComponent[i].Value.y,
                        z = tileDataComponent[i].Value.z * tileComponent.step,
                    }
                    });

                    // Uvs
                    uvs.Add(new Uv
                    {
                        Value = new float3(
                            x * 1 / count,
                            z * 1 / count,
                            0
                        )
                    });

                    //Triangles
                    if (x != (count - 1) && z != (count - 1))
                    {
                        // First triangle
                        triangles.Add(new Triangle
                        {
                            Value = i
                        });

                        triangles.Add(new Triangle
                        {
                            Value = i + 1
                        });

                        triangles.Add(new Triangle
                        {
                            Value = i + count + 1
                        });

                       
                        // Second triangle
                        triangles.Add(new Triangle
                        {
                            Value = i
                        });

                        triangles.Add(new Triangle
                        {
                            Value = i + count + 1
                        });

                        triangles.Add(new Triangle
                        {
                            Value = i + count
                        });
                    }
                }
                //ecb.RemoveComponent<TileDataComponent>(entityInQueryIndex, entity);
                ecb.RemoveComponent<TileMapUpdateComponentTag>(entityInQueryIndex, entity);
                ecb.AddComponent<MeshUpdateTag>(entityInQueryIndex, entity);
            }).Schedule();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}