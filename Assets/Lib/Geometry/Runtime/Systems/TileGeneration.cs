using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;

namespace FunkySheep.Geometry
{
    public partial class TileGeneration : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnStartRunning()
        {
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, in TileComponent tileComponent) =>
            {
                DynamicBuffer<Vertex> vertices = GetBuffer<Vertex>(entity);
                DynamicBuffer<Triangle> triangles = GetBuffer<Triangle>(entity);
                DynamicBuffer<Uv> uvs = GetBuffer<Uv>(entity);

                for (int z = 0; z <= tileComponent.count; z++)
                {
                    for (int x = 0; x <= tileComponent.count; x++)
                    {
                        // indexes
                        vertices.Add(new Vertex
                        {
                            Value = new float3(
                                x * tileComponent.size,
                                0,
                                z * tileComponent.size
                            )
                        });

                        // Uvs
                        uvs.Add(new Uv
                        {
                            Value = new float3(
                                (float)x * 1 / (float)tileComponent.count,
                                (float)z * 1 / (float)tileComponent.count,
                                0
                            )
                        });

                        //Triangles
                        if (x != tileComponent.count && z != tileComponent.count)
                        {
                            // First triangle
                            triangles.Add(new Triangle
                            {
                                Value = x + z * (tileComponent.count + 1)
                            });

                            triangles.Add(new Triangle
                            {
                                Value = x + 1 + (z + 1) * (tileComponent.count + 1)
                            });

                            triangles.Add(new Triangle
                            {
                                Value = x + 1 + z * (tileComponent.count + 1)
                            });

                            // Second triangle
                            triangles.Add(new Triangle
                            {
                                Value = x + z * (tileComponent.count + 1)
                            });

                            triangles.Add(new Triangle
                            {
                                Value = x + (z + 1) * (tileComponent.count + 1)
                            });

                            triangles.Add(new Triangle
                            {
                                Value = x + 1 + (z + 1) * (tileComponent.count + 1)
                            });
                        }
                    }
                }

                ecb.AddComponent<MeshUpdateTag>(entityInQueryIndex, entity);
            }).Schedule();
        }

        protected override void OnUpdate()
        {
            
        }
    }
}