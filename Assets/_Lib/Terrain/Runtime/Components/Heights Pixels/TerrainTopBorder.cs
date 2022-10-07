using Unity.Entities;
using UnityEngine;

namespace FunkySheep.Terrain
{
    public struct TerrainTopBorder : IBufferElementData
    {
        public Color32 Value;
    }
}
