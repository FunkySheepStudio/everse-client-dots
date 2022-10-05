using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace FunkySheep.Buildings
{
    public partial class SpawnTile : SystemBase
    {
        public Downloader downloader;
        protected override void OnCreate()
        {
            downloader = GameObject.FindObjectOfType<Downloader>();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, int entityInQueryIndex, ref TileSpawnerComponent tileSpawner, in Maps.MapPositionComponent mapPosition) =>
            {
                Maps.MapSingletonComponent mapSingleton = GetSingleton<Maps.MapSingletonComponent>();

                if (!tileSpawner.currentPosition.Equals(mapPosition.Value))
                {
                    tileSpawner.currentPosition = mapPosition.Value;

                    double[] gpsBoundaries = FunkySheep.Maps.Utils.CaclulateGpsBoundaries(mapSingleton.zoomLevel, mapPosition.Value);

                    downloader.Download(mapSingleton, mapPosition, gpsBoundaries);
                }

            })
            .WithStructuralChanges()
            .WithoutBurst()
            .Run();
        }

        public struct SpawnTileJob : IJobEntityBatch
        {
            public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
            {
            }
        }
    }
}
