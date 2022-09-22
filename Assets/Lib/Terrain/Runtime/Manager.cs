using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;

namespace FunkySheep.Terrain
{
    [AddComponentMenu("FunkySheep/Terrain/Manager")]
    public class Manager : MonoBehaviour, IConvertGameObjectToEntity
    {
        public int tileSize;
        public int tileResolution;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddBuffer<VerticeComponent>(entity);

            dstManager.AddComponentData<TileComponent>(entity, new TileComponent
            {
                size = tileSize,
                resolution = tileResolution,
                position = new int2(0, 0)
            });
        }
    }

}
