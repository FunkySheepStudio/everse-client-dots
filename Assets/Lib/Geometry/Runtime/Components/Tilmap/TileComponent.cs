using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Geometry
{
    [GenerateAuthoringComponent]
    public struct TileComponent : IComponentData
    {
        public float step;
        public int2 gridPosition;
    }
}
