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
    [DisableAutoCreation]
    public partial class DownloadTilesHeightMap : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, int entityInQueryIndex,  ref TileComponent tileComponent, in MapPositionComponent mapPosition) =>
            {
                EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

                int zoomLevel = GetSingleton<MapSingletonComponent>().zoomLevel;
                string url = $"https://s3.amazonaws.com/elevation-tiles-prod/terrarium/{zoomLevel}/{(int)mapPosition.Value.x}/{(int)mapPosition.Value.y}.png";
                World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<MapPositionComponent>(entity);
                Task<NativeArray<PixelComponent>> pixelComponents = Download(url);
                pixelComponents.ContinueWith((t) =>
                {
                    DynamicBuffer<PixelComponent> pixelBuffer = GetBuffer<PixelComponent>(entity);
                    pixelBuffer.CopyFrom(t.Result.ToArray());
                    ecb.AddBuffer<TileDataComponent>(entityInQueryIndex, entity);
                    pixelComponents.Dispose();
                });
            })
            .WithStructuralChanges()
            .WithoutBurst()
            .Run();
        }

        async Task<NativeArray<PixelComponent>> Download(string url)
        {
            NativeArray<PixelComponent> pixelBuffer = new NativeArray<PixelComponent>();

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url, false);
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.filterMode = FilterMode.Point;
                pixelBuffer = texture.GetRawTextureData<PixelComponent>();
            }
            else
            {
                Debug.Log("Unable to download height map from: " + url);
            }

            return pixelBuffer;
        }
    }
}
