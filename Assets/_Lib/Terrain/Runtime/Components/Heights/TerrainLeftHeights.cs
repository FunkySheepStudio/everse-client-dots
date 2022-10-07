using Unity.Entities;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainLeftBorderHeights : IBufferElementData
    {
        public float Value;
    }
}
