using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainTilePrefabComponent : IComponentData
    {
        public Entity Value;
    }
}
