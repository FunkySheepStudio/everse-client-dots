using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace FunkySheep.Maps
{
    [GenerateAuthoringComponent]
    public struct MapSingletonComponent : IComponentData
    {
        public int zoomLevel;
        public float tileSize;
        public float2 initialMapPosition;
        public float2 initialOffset;
    }
}
