using Unity.Entities;
using Unity.Collections;
using FunkySheep.Earth;
using Unity.Jobs;

namespace FunkySheep.Terrain
{
    [DisableAutoCreation]
    public partial class MergeHeightsWithBorders : SystemBase
    {
        protected override void OnUpdate()
        {
            NativeArray<Entity> spawners = GetEntityQuery(typeof(TerrainTileSpawnerTag), typeof(EarthGridPosition)).ToEntityArray(Allocator.TempJob);
            NativeArray<Entity> tiles = GetEntityQuery(typeof(TerrainHeights), typeof(EarthGridPosition)).ToEntityArray(Allocator.TempJob);

            DynamicBuffer<Entity> tilesToProcess = new DynamicBuffer<Entity>();

            Entities.ForEach((Entity entity, int entityInQueryIndex, in TerrainTileSpawnerTag spawner, in EarthGridPosition spawnerTilePosition) =>
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    if (GetComponent<EarthGridPosition>(tiles[i]).Value.Equals(spawnerTilePosition.Value))
                    {
                        tilesToProcess.Add(tiles[i]);
                    }
                }

            }).ScheduleParallel();

            Dependency = JobHandle.CombineDependencies(Dependency, spawners.Dispose(Dependency), tiles.Dispose(Dependency));
        }
    }
}
