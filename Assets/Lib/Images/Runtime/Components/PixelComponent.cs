using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Images
{
    public struct PixelComponent : IComponentData
    {
        public int4 Value;
    }
}
