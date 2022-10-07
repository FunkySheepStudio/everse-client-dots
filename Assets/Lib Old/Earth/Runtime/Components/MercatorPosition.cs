using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Earth
{
    [GenerateAuthoringComponent]
    public struct MercatorPositionComponent : IComponentData
    {
        public float2 Initial;
        public float2 Value;
    }
}
