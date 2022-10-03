using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Images;
using FunkySheep.Geometry;
using FunkySheep.Maps;

namespace FunkySheep.Terrain
{
    public partial class UpdateTileHeights : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
            float tileSize = GetSingleton<MapSingletonComponent>().tileSize;

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref DynamicBuffer<TileDataComponent> tileDataComponents, in DynamicBuffer<PixelComponent> pixelComponents) =>
            {
                float step = tileSize / Mathf.Sqrt(pixelComponents.Length);

                for (int z = 0; z < Mathf.Sqrt(pixelComponents.Length); z++)
                {
                    for (int x = 0; x < Mathf.Sqrt(pixelComponents.Length); x++)
                    {
                        Color32 color = pixelComponents[
                            z +
                            x * (int)Mathf.Sqrt(pixelComponents.Length)
                        ].Value;

                        float height = GetHeightFromColor(color, x, z);
                        tileDataComponents.Add(new TileDataComponent
                        {
                            Value = new float3
                            {
                                x = z, // x and z are inverted since images muse be rotated
                                y = height,
                                z = x
                            }
                        });
                    }
                }

                ecb.RemoveComponent<PixelComponent>(entityInQueryIndex, entity);
                ecb.AddComponent<TileMapUpdateComponentTag>(entityInQueryIndex, entity);
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }

        private static float GetHeightFromColor(Color32 color, int x, int y)
        {
            float height = (Mathf.Floor(color.g * 256.0f) + Mathf.Floor(color.b) + color.a / 256) - 32768.0f;
            return height;
        }
    }
}
