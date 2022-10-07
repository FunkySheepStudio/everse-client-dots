using Unity.Entities;

namespace FunkySheep.Buildings
{
    [GenerateAuthoringComponent]
    public struct BuildingComponent : IComponentData
    {
        public uint id;
    }
}
