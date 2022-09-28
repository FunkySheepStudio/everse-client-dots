using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainTilePrefab : IComponentData
    {
        public Entity Value;
    }
}
