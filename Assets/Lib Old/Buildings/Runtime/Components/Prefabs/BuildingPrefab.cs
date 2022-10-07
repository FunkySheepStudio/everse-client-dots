using Unity.Entities;

namespace FunkySheep.Buildings
{
    [GenerateAuthoringComponent]
    public struct BuildingPrefab : IComponentData
    {
        public Entity Value;
    }
}
