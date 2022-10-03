using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using FunkySheep.Network;
using FunkySheep.Images;
using FunkySheep.Maps;
using FunkySheep.Geometry;

namespace FunkySheep.Terrain
{
    public class HeightMapDownloader : MapDownloader
    {
        public override void Process(string fileId, Texture2D texture, MapSingletonComponent mapSingleton, MapPositionComponent mapPosition)
        {
            NativeArray<PixelComponent> pixelBuffer = new NativeArray<PixelComponent>();
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            pixelBuffer = texture.GetRawTextureData<PixelComponent>();

            Entity entity = entityManager.Instantiate(tileEntity);
            entityManager.AddBuffer<PixelComponent>(entity);
            entityManager.GetBuffer<PixelComponent>(entity).CopyFrom(pixelBuffer.ToArray());
            entityManager.AddBuffer<TileDataComponent>(entity);

            entityManager.SetComponentData<TileComponent>(entity, new TileComponent
            {
                step = mapSingleton.tileSize / 256,
                gridPosition = new int2
                {
                    x = (mapPosition.Value.x - (int)mapSingleton.initialMapPosition.x),
                    y = (int)mapSingleton.initialMapPosition.y - mapPosition.Value.y
                }
            });

            float3 tilePosition = new float3
            {
                x = (mapPosition.Value.x - (int)mapSingleton.initialMapPosition.x) * mapSingleton.tileSize + mapSingleton.initialOffset.x * mapSingleton.tileSize,
                y = 0,
                z = ((int)mapSingleton.initialMapPosition.y - mapPosition.Value.y) * mapSingleton.tileSize + mapSingleton.initialOffset.y * mapSingleton.tileSize
            };

            entityManager.SetComponentData<Translation>(entity, new Translation
            {
                Value = tilePosition
            });
        }
    }

}
