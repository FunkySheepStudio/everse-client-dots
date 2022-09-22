using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;

namespace FunkySheep.Terrain
{
    public struct VerticeComponent : IBufferElementData
    {
        public float3 position;
        public float3 position1;
        public float3 position2;
    }
}
