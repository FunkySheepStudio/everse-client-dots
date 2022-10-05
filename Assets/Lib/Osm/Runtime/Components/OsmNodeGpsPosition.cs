using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.OSM.Ecs
{
    [GenerateAuthoringComponent]
    public struct OsmNodeGpsPosition : IComponentData
    {
        public double2 Value;
    }
}
