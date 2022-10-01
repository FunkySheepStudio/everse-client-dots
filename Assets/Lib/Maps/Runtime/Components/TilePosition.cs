using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Maps
{
    [GenerateAuthoringComponent]
    public struct TilePositionComponent : IComponentData
    {
        public int2 Value;
    }
}
