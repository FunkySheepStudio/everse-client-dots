using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Buildings
{
    [GenerateAuthoringComponent]
    public struct TileSpawnerComponent : IComponentData
    {
        public int2 currentPosition;
    }
}
