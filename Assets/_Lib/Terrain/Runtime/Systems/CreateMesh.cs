using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Earth;
using UnityEngine;

namespace FunkySheep.Terrain
{
    public partial class CreateMesh : SystemBase
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
                .ForEach((Entity entity, int entityInQueryIndex, ref DynamicBuffer <TerrainTopBorderHeights> terrainTopBorderHeights, in DynamicBuffer<AdjacentTiles> adjacentTiles, in EarthGridPosition earthGridPosition) =>
                {
                    if (adjacentTiles.Length == 8)
                    {
                        for (int i = 0; i < adjacentTiles.Length; i++)
                        {
                            int2 ajdacentGridPosition = GetComponent<EarthGridPosition>(adjacentTiles[i].entity).Value;

                            // Top tile
                            if (ajdacentGridPosition.x == earthGridPosition.Value.x && ajdacentGridPosition.y == earthGridPosition.Value.y + 1)
                            {
                                DynamicBuffer<TerrainBottomBorderHeights> bottomAdjacentPixels;
                                bool gotBuffer = GetBufferFromEntity<TerrainBottomBorderHeights>(true).TryGetBuffer(adjacentTiles[i].entity, out bottomAdjacentPixels);
                                for (int j = 0; j < terrainTopBorderHeights.Length; j++)
                                {
                                    terrainTopBorderHeights[j] = new TerrainTopBorderHeights
                                    {
                                        Value = (terrainTopBorderHeights[j].Value + bottomAdjacentPixels[j].Value) / 2
                                    };
                                }
                            }
                        }
                    }
                })
                .ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}
