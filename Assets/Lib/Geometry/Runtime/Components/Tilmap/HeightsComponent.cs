using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Geometry
{
    public struct HeightsComponent : IBufferElementData
    {
        float2 position;
        float height;
    }
}
