using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TilePrefab : IComponentData
    {
        public Entity Value;
    }
}
