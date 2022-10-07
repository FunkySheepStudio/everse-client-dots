using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainBottomBorderHeights : IBufferElementData
    {
        public float Value;
    }
}
