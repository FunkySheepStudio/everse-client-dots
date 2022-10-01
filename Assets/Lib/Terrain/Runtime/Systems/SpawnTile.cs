using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using FunkySheep.Maps;

namespace FunkySheep.Terrain
{
    [DisableAutoCreation]
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

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref TileSpawnerComponent tileSpawner, in Translation translation, in TerrainTilePrefabComponent terrainTilePrefab, in MapPositionComponent mapPosition) =>
            {

                int2 newtilePosition = new int2(
                    (int) (translation.Value.x / tileSize),
                    (int) (translation.Value.z / tileSize)
                );

                if (!tileSpawner.currentPosition.Equals(newtilePosition))
                {
                    tileSpawner.currentPosition = newtilePosition;

                    Entity tile = ecb.Instantiate(entityInQueryIndex, terrainTilePrefab.Value);

                    ecb.SetComponent<Translation>(entityInQueryIndex, tile, new Translation
                    {
                        Value = new Unity.Mathematics.float3(
                            newtilePosition.x * tileSize,
                            0,
                            newtilePosition.y * tileSize
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
