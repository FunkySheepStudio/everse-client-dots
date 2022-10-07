using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Maps
{
    [GenerateAuthoringComponent]
    public struct MapPositionComponent : IComponentData
    {
        public int2 Value;
    }
}
