using Unity.Entities;

namespace FunkySheep.Maps
{
    [GenerateAuthoringComponent]
    public struct ZoomLevel : IComponentData
    {
        public int Value;
    }
}
