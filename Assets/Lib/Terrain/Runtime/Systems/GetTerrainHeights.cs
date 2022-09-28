using System.Threading.Tasks;
using Unity.Entities;
using FunkySheep.Maps;
using FunkySheep.Geometry;
using UnityEngine;
using UnityEngine.Networking;
using FunkySheep.Images;
using Unity.Collections;

namespace FunkySheep.Terrain
{
    public partial class GetTerrainHeights : SystemBase
    {

        async Task<byte[]> Download(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                byte[] file = request.downloadHandler.data;
                return file;

            } else
            {
                Debug.Log("Unable to download height map from: " + url);
                return null;
            }
        }

        static NativeArray<PixelComponent> SetHeights(byte[] file, Entity entity)
        {
            Texture2D texture = new Texture2D(256, 256);
            texture.LoadImage(file);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            NativeArray<PixelComponent> pixelBuffer = texture.GetRawTextureData<PixelComponent>();
            return pixelBuffer;
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, in TileComponent tileComponent, in MapPosition mapPosition, in ZoomLevel zoomLevel) =>
            {
                string url = $"https://s3.amazonaws.com/elevation-tiles-prod/terrarium/{zoomLevel.Value}/{(int)mapPosition.Value.x}/{(int)mapPosition.Value.y}.png";
                Download(url)
                .ContinueWith((t) =>
                {
                    if (t.Result != null)
                    {
                        DynamicBuffer<PixelComponent> pixelBuffer = GetBuffer<PixelComponent>(entity);
                        pixelBuffer.CopyFrom(SetHeights(t.Result, entity).ToArray());
                        Debug.Log("test");
                    }
                });

                /*World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<TileComponent>(entity);
                World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<MapPosition>(entity);
                World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<ZoomLevel>(entity);*/
            })
            .WithStructuralChanges()
            .WithoutBurst()
            .Run();
        }
    }
}
