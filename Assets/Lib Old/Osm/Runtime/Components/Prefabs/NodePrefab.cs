using Unity.Entities;

namespace FunkySheep.OSM.Ecs
{
    [GenerateAuthoringComponent]
    public struct NodePrefab : IComponentData
    {
        public Entity Value;
    }
}
