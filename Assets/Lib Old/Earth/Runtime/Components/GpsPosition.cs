using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Earth
{
    [GenerateAuthoringComponent]
    public struct GpsPositionComponent : IComponentData
    {
        public double2 Value;
    }
}
