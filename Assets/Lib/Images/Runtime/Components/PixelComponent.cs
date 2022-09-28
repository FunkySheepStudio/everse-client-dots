using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Images
{
    [GenerateAuthoringComponent]
    public struct PixelComponent : IBufferElementData
    {
        public int4 Value;
    }
}
