using Unity.Entities;
using FunkySheep.Images;
using UnityEngine;

namespace FunkySheep.Terrain
{
    public partial class ConvertPixelsToHeights : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            // Main Heights
            Entities
                .ForEach((Entity entity, int entityInQueryIndex, in DynamicBuffer<PixelComponent> pixels) =>
            {
                DynamicBuffer<TerrainHeights> terrainHeights = ecb.AddBuffer<TerrainHeights>(entityInQueryIndex, entity);

                for (int i = 0; i < pixels.Length; i++)
                {
                    terrainHeights.Add(
                        new TerrainHeights
                        {
                            Value = GetHeightFromColor(pixels[i].Value)
                        }
                    );
                }
                ecb.RemoveComponent<PixelComponent>(entityInQueryIndex, entity);

            }).ScheduleParallel();

            // Bottom Heights
            Entities
                .ForEach((Entity entity, int entityInQueryIndex, in DynamicBuffer<TerrainBottomBorder> pixels) =>
                {
                    DynamicBuffer<TerrainBottomBorderHeights> terrainHeights = ecb.AddBuffer<TerrainBottomBorderHeights>(entityInQueryIndex, entity);

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        terrainHeights.Add(
                            new TerrainBottomBorderHeights
                            {
                                Value = GetHeightFromColor(pixels[i].Value)
                            }
                        );
                    }

                    ecb.RemoveComponent<TerrainBottomBorder>(entityInQueryIndex, entity);

                }).ScheduleParallel();

            // Top Heights
            Entities
                .ForEach((Entity entity, int entityInQueryIndex, in DynamicBuffer<TerrainTopBorder> pixels) =>
                {
                    DynamicBuffer<TerrainTopBorderHeights> terrainHeights = ecb.AddBuffer<TerrainTopBorderHeights>(entityInQueryIndex, entity);

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        terrainHeights.Add(
                            new TerrainTopBorderHeights
                            {
                                Value = GetHeightFromColor(pixels[i].Value)
                            }
                        );
                    }

                    ecb.RemoveComponent<TerrainTopBorder>(entityInQueryIndex, entity);

                }).ScheduleParallel();

            // Left Heights
            Entities
                .ForEach((Entity entity, int entityInQueryIndex, in DynamicBuffer<TerrainLeftBorder> pixels) =>
                {
                    DynamicBuffer<TerrainLeftBorderHeights> terrainHeights = ecb.AddBuffer<TerrainLeftBorderHeights>(entityInQueryIndex, entity);

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        terrainHeights.Add(
                            new TerrainLeftBorderHeights
                            {
                                Value = GetHeightFromColor(pixels[i].Value)
                            }
                        );
                    }

                    ecb.RemoveComponent<TerrainLeftBorder>(entityInQueryIndex, entity);

                }).ScheduleParallel();

            // Right Heights
            Entities
                .ForEach((Entity entity, int entityInQueryIndex, in DynamicBuffer<TerrainRightBorder> pixels) =>
                {
                    DynamicBuffer<TerrainRightBorderHeights> terrainHeights = ecb.AddBuffer<TerrainRightBorderHeights>(entityInQueryIndex, entity);

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        terrainHeights.Add(
                            new TerrainRightBorderHeights
                            {
                                Value = GetHeightFromColor(pixels[i].Value)
                            }
                        );
                    }

                    ecb.RemoveComponent<TerrainRightBorder>(entityInQueryIndex, entity);

                }).ScheduleParallel();

            // Corners Heights
            Entities
                .ForEach((Entity entity, int entityInQueryIndex, in TerrainCorners pixels) =>
                {
                    ecb.AddComponent<TerrainCornersHeights>(entityInQueryIndex, entity);
                    ecb.SetComponent<TerrainCornersHeights>(entityInQueryIndex, entity, new TerrainCornersHeights {
                        TopLeft = GetHeightFromColor(pixels.TopLeft),
                        TopRight = GetHeightFromColor(pixels.TopRight),
                        BottomLeft = GetHeightFromColor(pixels.BottomLeft),
                        BottomRight = GetHeightFromColor(pixels.BottomRight)
                    });

                    ecb.RemoveComponent<TerrainCorners>(entityInQueryIndex, entity);

                }).ScheduleParallel();


            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }

        private static float GetHeightFromColor(Color32 color)
        {
            float height = (Mathf.Floor(color.g * 256.0f) + Mathf.Floor(color.b) + color.a / 256) - 32768.0f;
            return height;
        }
    }
}
