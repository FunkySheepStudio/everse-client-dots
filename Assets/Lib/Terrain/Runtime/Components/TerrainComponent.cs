using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainComponent : IComponentData
    {
        public int size;
        public TileComponent tile;
    }
}
