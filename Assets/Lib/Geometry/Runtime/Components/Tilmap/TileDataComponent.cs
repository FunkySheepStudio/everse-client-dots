using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Geometry
{
    public struct TileDataComponent : IBufferElementData
    {
        public float3 Value;
    }
}