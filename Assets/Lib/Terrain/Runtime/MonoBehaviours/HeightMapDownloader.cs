using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using FunkySheep.Network;
using FunkySheep.Images;
using FunkySheep.Maps;
using FunkySheep.Geometry;

namespace FunkySheep.Terrain
{
    public class HeightMapDownloader : MonoBehaviour
{
    public GameObject tilePrefab;
    private Entity tileEntity;
    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;

    private void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings convertionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        tileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(tilePrefab, convertionSettings);
    }


    void Start()
    {
        string url = $"https://s3.amazonaws.com/elevation-tiles-prod/terrarium/15/16783/11836.png";
        Download(url);
    }

    public void Download(string url)
    {
        StartCoroutine(Downloader.DownloadTexture(url, (fileID, texture) =>
        {
            NativeArray<PixelComponent> pixelBuffer = new NativeArray<PixelComponent>();
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            pixelBuffer = texture.GetRawTextureData<PixelComponent>();

            Entity entity = entityManager.Instantiate(tileEntity);
            entityManager.AddBuffer<PixelComponent>(entity);
            entityManager.GetBuffer<PixelComponent>(entity).CopyFrom(pixelBuffer.ToArray());
            entityManager.AddBuffer<TileDataComponent>(entity);
        }));
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }
}

}
