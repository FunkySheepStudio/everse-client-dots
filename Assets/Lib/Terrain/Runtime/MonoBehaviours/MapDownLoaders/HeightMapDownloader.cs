using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using FunkySheep.Images;
using FunkySheep.Maps;
using System.Collections.Generic;

namespace FunkySheep.Terrain
{
    public class HeightMapDownloader : MapDownloader
    {
        List<MapPositionComponent> currentMapPositions = new List<MapPositionComponent>();

        public override void Download(MapSingletonComponent mapSingleton, MapPositionComponent mapPosition)
        {
            if (!currentMapPositions.Contains(mapPosition))
            {
                base.Download(mapSingleton, mapPosition);
            }
        }

        public override void Process(string fileId, Texture2D texture, MapSingletonComponent mapSingleton, MapPositionComponent mapPosition)
        {
            NativeArray<PixelComponent> pixelBuffer = new NativeArray<PixelComponent>();
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            pixelBuffer = texture.GetRawTextureData<PixelComponent>();

            Entity entity = entityManager.Instantiate(tileEntity);
            entityManager.GetBuffer<PixelComponent>(entity).CopyFrom(pixelBuffer.ToArray());
            entityManager.SetComponentData<MapPositionComponent>(entity, mapPosition);
            currentMapPositions.Add(mapPosition);
        }
    }

}
