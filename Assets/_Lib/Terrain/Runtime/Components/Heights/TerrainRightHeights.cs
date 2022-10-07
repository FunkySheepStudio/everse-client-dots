using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainRightBorderHeights : IBufferElementData
    {
        public float Value;
    }
}
