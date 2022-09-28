using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Earth
{
    [GenerateAuthoringComponent]
    public struct GpsPosition : IComponentData
    {
        public double2 Value;
    }
}
