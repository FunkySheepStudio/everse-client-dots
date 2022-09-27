using Unity.Entities;
using Unity.Transforms;
using FunkySheep.Geometry;

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
                for (int z = 0; z < terrainComponent.Cachesize; z++)
                {
                    for (int x = 0; x < terrainComponent.Cachesize; x++)
                    {
                        Entity tile = ecb.Instantiate(x + z * x, tilePrefab.Value);

                        ecb.SetComponent<Translation>(x + z * x, tile, new Translation {
                            Value = new Unity.Mathematics.float3(
                                x * terrainComponent.TileItemsCount * terrainComponent.TileItemSize,
                                0,
                                z * terrainComponent.TileItemsCount * terrainComponent.TileItemSize
                            )
                        });

                        ecb.SetComponent<TileComponent>(x + z * x, tile, new TileComponent {
                            count = terrainComponent.TileItemsCount,
                            size = terrainComponent.TileItemSize
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
