using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using FunkySheep.Maps;

namespace FunkySheep.Terrain
{
    public partial class SpawnTile : SystemBase
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

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref TileSpawnerComponent tileSpawner, in TilePositionComponent tilePositionComponent, in TerrainTilePrefabComponent terrainTilePrefab, in MapPositionComponent mapPosition) =>
            {

                if (!tileSpawner.currentPosition.Equals(mapPosition.Value))
                {
                    tileSpawner.currentPosition = mapPosition.Value;

                    Entity tile = ecb.Instantiate(entityInQueryIndex, terrainTilePrefab.Value);

                    ecb.SetComponent<Translation>(entityInQueryIndex, tile, new Translation
                    {
                        Value = new Unity.Mathematics.float3(
                            tilePositionComponent.Value.x * tileSize,
                            0,
                            tilePositionComponent.Value.y * tileSize
                        )
                    });

                    ecb.SetComponent<MapPositionComponent>(entityInQueryIndex, tile, mapPosition);
                }

            }).ScheduleParallel();
            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }

        public struct SpawnTileJob : IJobEntityBatch
        {
            public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
            {
            }
        }
    }
}
