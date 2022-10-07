using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Earth
{
    [GenerateAuthoringComponent]
    public struct EarthGridPosition : IComponentData
    {
        public int2 Value;
    }
}
