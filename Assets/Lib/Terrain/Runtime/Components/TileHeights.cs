using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Terrain
{
    public struct TileHeights : IBufferElementData
    {
        public int2 position;
        public float height;
    }
}