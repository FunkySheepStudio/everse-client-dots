using Unity.Entities;
using UnityEngine;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainCornersHeights : IComponentData
    {
        public float TopLeft;
        public float TopRight;
        public float BottomRight;
        public float BottomLeft;
    }
}
