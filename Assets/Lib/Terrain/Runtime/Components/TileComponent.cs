using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;

namespace FunkySheep.Terrain
{
    public struct TileComponent : IComponentData
    {
        public int2 position;
        public int size;
        public int resolution;
        public DynamicBuffer<float3> points;
    }
}
