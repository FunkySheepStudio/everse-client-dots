using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using FunkySheep.Geometry;

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

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref TileSpawner tileSpawner, in Translation translation, in TerrainTilePrefab terrainTilePrefab) =>
            {
                int2 newtilePosition = new int2(
                    (int) translation.Value.x / tileSpawner.size,
                    (int) translation.Value.z / tileSpawner.size
                );

                if (!tileSpawner.currentPosition.Equals(newtilePosition))
                {
                    tileSpawner.currentPosition = newtilePosition;

                    Entity tile = ecb.Instantiate(entityInQueryIndex, terrainTilePrefab.Value);

                    ecb.SetComponent<Translation>(entityInQueryIndex, tile, new Translation
                    {
                        Value = new Unity.Mathematics.float3(
                            newtilePosition.x * tileSpawner.size,
                            0,
                            newtilePosition.y * tileSpawner.size
                        )
                    });
                }

            }).ScheduleParallel();
            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}
