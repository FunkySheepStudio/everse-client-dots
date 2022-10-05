using Unity.Entities;

namespace FunkySheep.Buildings
{
    [GenerateAuthoringComponent]
    public struct BuildingsComponent : IBufferElementData
    {
        public BuildingComponent building;
    }
}
