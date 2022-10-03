using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct HeightPrefab : IComponentData
    {
        public Entity Value;
    }
}
