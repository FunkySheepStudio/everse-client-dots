using Unity.Entities;
using UnityEngine;
using FunkySheep.Network;
using FunkySheep.Maps;
using FunkySheep.Types;

namespace FunkySheep.Terrain
{
    public class MapDownloader : MonoBehaviour
    {
        //public String urlTemplate;
        public GameObject tilePrefab;
        [HideInInspector]
        public Entity tileEntity;
        [HideInInspector]
        public EntityManager entityManager;
        [HideInInspector]
        public BlobAssetStore blobAssetStore;

        private void Awake()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            blobAssetStore = new BlobAssetStore();
            GameObjectConversionSettings convertionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
            tileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(tilePrefab, convertionSettings);
        }

        public void Download(MapSingletonComponent mapSingleton, MapPositionComponent mapPosition)
        {
            string url = $"https://s3.amazonaws.com/elevation-tiles-prod/terrarium/{mapSingleton.zoomLevel}/{mapPosition.Value.x}/{mapPosition.Value.y}.png";

            StartCoroutine(Downloader.DownloadTexture(url, (fileID, texture) =>
            {
                Process(fileID, texture, mapSingleton, mapPosition);
            }));
        }

        public virtual void Process(string fileId, Texture2D texture, MapSingletonComponent mapSingleton, MapPositionComponent mapPosition)
        {
            
        }

        private void OnDestroy()
        {
            blobAssetStore.Dispose();
        }
    }

}
