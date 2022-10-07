using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using FunkySheep.Earth;

namespace FunkySheep.Terrain
{
    public partial class SpawnTerrainTile : SystemBase
    {
        public HeightMapDownloader heightMapDownloader;
        protected override void OnCreate()
        {
            heightMapDownloader = GameObject.FindObjectOfType<HeightMapDownloader>();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, int entityInQueryIndex, in TerrainTileSpawnerTag terrainTileSpawnerTag , in EarthGridPosition earthGridPosition) =>
            {
                heightMapDownloader.DownloadAtGridPosition(earthGridPosition);
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
