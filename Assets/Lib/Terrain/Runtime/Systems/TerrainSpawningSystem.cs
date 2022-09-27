using Unity.Entities;
using Unity.Jobs;

namespace FunkySheep.Terrain
{
    public partial class TerrainSpawningSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnStartRunning()
        {
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, in TerrainComponent terrainComponent, in TilePrefab tilePrefab) => {
                for (int z = 0; z < terrainComponent.size; z++)
                {
                    for (int x = 0; x < terrainComponent.size; x++)
                    {
                        Entity tile = ecb.Instantiate(x + z * x, tilePrefab.Value);
                        ecb.SetComponent<TileComponent>(x + z * x, tile, new TileComponent {
                            count = terrainComponent.tile.count,
                            size = terrainComponent.tile.size,
                            position = new Unity.Mathematics.int2(x, z)
                        });
                    }

                }
            }).Schedule();
        }
        protected override void OnUpdate()
        {
        }
    }

}
