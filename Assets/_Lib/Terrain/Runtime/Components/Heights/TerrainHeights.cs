using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainHeights : IBufferElementData
    {
        public float Value;
    }
}
