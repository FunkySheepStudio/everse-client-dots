using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Images;
using FunkySheep.Maps;
using Unity.Transforms;

namespace FunkySheep.Terrain
{
    public partial class SpawnHeights : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            MapSingletonComponent mapSingleton = GetSingleton<MapSingletonComponent>();
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, in DynamicBuffer<PixelComponent> pixelComponents, in HeightPrefab heightPrefab, in MapPositionComponent mapPosition) =>
            {
                float step = mapSingleton.tileSize / Mathf.Sqrt(pixelComponents.Length);

                for (int x = 0; x < Mathf.Sqrt(pixelComponents.Length); x++)
                {
                    for (int z = 0; z < Mathf.Sqrt(pixelComponents.Length); z++)
                    {
                        Color32 color = pixelComponents[
                            x +
                            z * (int)Mathf.Sqrt(pixelComponents.Length) // x and z are inverted since images muse be rotated
                        ].Value;

                        float height = GetHeightFromColor(color, x, z);

                        Entity heightEntity = ecb.Instantiate(entityInQueryIndex, heightPrefab.Value);

                        float3 tilePosition = new float3
                        {
                            x = ((mapPosition.Value.x - (int)mapSingleton.initialMapPosition.x) * mapSingleton.tileSize + mapSingleton.initialOffset.x * mapSingleton.tileSize) + x * (mapSingleton.tileSize / 256),
                            y = height,
                            z = (((int)mapSingleton.initialMapPosition.y - mapPosition.Value.y) * mapSingleton.tileSize + mapSingleton.initialOffset.y * mapSingleton.tileSize) + z * (mapSingleton.tileSize / 256)
                        };

                        ecb.SetComponent<Translation>(entityInQueryIndex, heightEntity, new Translation
                        {
                            Value = tilePosition
                        });

                        ecb.RemoveComponent<PixelComponent>(entityInQueryIndex, entity);
                    }
                }
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
