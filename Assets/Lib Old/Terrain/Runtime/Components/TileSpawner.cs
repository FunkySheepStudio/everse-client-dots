using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TileSpawnerComponent : IComponentData
    {
        public int2 currentPosition;
        //public int size;
    }
}
