using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using FunkySheep.Maps;
using UnityEngine;

namespace FunkySheep.Terrain
{
    public partial class SpawnTile : SystemBase
    {
        public HeightMapDownloader heightMapDownloader;
        protected override void OnCreate()
        {
            heightMapDownloader = GameObject.FindObjectOfType<HeightMapDownloader>();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, int entityInQueryIndex, ref TileSpawnerComponent tileSpawner, in TilePositionComponent tilePositionComponent, in MapPositionComponent mapPosition) =>
            {
                MapSingletonComponent mapSingleton = GetSingleton<MapSingletonComponent>();

                if (!tileSpawner.currentPosition.Equals(mapPosition.Value))
                {
                    tileSpawner.currentPosition = mapPosition.Value;

                    string url = $"https://s3.amazonaws.com/elevation-tiles-prod/terrarium/{mapSingleton.zoomLevel}/{mapPosition.Value.x}/{mapPosition.Value.y}.png";
                    heightMapDownloader.Download(url);
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
