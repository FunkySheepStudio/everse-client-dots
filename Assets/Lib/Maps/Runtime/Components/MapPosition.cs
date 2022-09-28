using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Maps
{
    [GenerateAuthoringComponent]
    public struct MapPosition : IComponentData
    {
        public float2 Value;
    }
}
