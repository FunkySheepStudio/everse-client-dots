using Unity.Entities;

namespace FunkySheep.Buildings
{
    [GenerateAuthoringComponent]
    public struct BuildingsTilePrefabComponent : IComponentData
    {
        public Entity Value;
    }
}
