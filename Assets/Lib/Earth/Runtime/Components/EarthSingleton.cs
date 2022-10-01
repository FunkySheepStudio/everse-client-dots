using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Earth
{
    [GenerateAuthoringComponent]
    public struct EarthSingletonComponent : IComponentData
    {
        public double2 initialGpsPosition;
    }
}
