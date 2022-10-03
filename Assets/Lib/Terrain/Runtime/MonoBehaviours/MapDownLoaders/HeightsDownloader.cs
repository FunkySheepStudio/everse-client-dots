using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using FunkySheep.Images;
using FunkySheep.Maps;

namespace FunkySheep.Terrain
{
    public class HeightsDownloader : MapDownloader
    {
        public override void Process(string fileId, Texture2D texture, MapSingletonComponent mapSingleton, MapPositionComponent mapPosition)
        {
            NativeArray<PixelComponent> pixelBuffer = new NativeArray<PixelComponent>();
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            pixelBuffer = texture.GetRawTextureData<PixelComponent>();


            for (int x = 0; x < 256; x++)
            {
                for (int z = 0; z < 256; z++)
                {
                    Entity entity = entityManager.Instantiate(tileEntity);

                    entityManager.SetComponentData<Pixel>(entity, new Pixel
                    {
                        Value = pixelBuffer[
                                z + 
                                x * 256
                            ].Value
                    });

                    float3 tilePosition = new float3
                    {
                        x = ((mapPosition.Value.x - (int)mapSingleton.initialMapPosition.x) * mapSingleton.tileSize + mapSingleton.initialOffset.x * mapSingleton.tileSize) + x * (mapSingleton.tileSize / 256),
                        y = 0,
                        z = (((int)mapSingleton.initialMapPosition.y - mapPosition.Value.y) * mapSingleton.tileSize + mapSingleton.initialOffset.y * mapSingleton.tileSize) + z * (mapSingleton.tileSize / 256)
                    };

                    entityManager.SetComponentData<Translation>(entity, new Translation
                    {
                        Value = tilePosition
                    });
                }
            }
        }
    }

}
