using Unity.Entities;
using UnityEngine;
using FunkySheep.Earth;

namespace FunkySheep.Terrain
{
    public class MapDownloader : MonoBehaviour
    {
        public FunkySheep.Types.Int32 earthZoomLevel;
        public FunkySheep.Types.Vector2Int earthInitialMapPosition;

        [HideInInspector]
        public EntityManager entityManager;

        public virtual void Awake()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        public virtual void Download(Entity entity)
        {
            EarthGridPosition entityGridPosition = entityManager.GetComponentData<EarthGridPosition>(entity);

            Vector2Int mapPosition = new Vector2Int(
                earthInitialMapPosition.value.x + entityGridPosition.Value.x,
                earthInitialMapPosition.value.y + entityGridPosition.Value.y
                );

            string url = $"https://s3.amazonaws.com/elevation-tiles-prod/terrarium/{earthZoomLevel.value}/{mapPosition.x}/{mapPosition.y}.png";

            StartCoroutine(FunkySheep.Network.Downloader.DownloadTexture(url, (fileID, texture) =>
            {
                Process(fileID, texture, mapPosition, entity);
            }));
        }

        public virtual void Process(string fileId, Texture2D texture, Vector2Int mapPosition, Entity entity)
        {
            
        }
    }

}
