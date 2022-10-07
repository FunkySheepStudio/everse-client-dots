using Unity.Entities;
using UnityEngine;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainBottomBorder : IBufferElementData
    {
        public Color32 Value;
    }
}
