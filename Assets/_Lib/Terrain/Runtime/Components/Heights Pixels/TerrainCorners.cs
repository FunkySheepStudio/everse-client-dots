using Unity.Entities;
using UnityEngine;

namespace FunkySheep.Terrain
{
    [GenerateAuthoringComponent]
    public struct TerrainCorners : IComponentData
    {
        public Color32 TopLeft;
        public Color32 TopRight;
        public Color32 BottomRight;
        public Color32 BottomLeft;
    }
}
