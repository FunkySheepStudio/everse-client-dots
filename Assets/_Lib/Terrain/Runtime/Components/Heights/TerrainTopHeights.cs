using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainTopBorderHeights : IBufferElementData
    {
        public float Value;
    }
}
