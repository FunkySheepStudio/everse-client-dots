using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace FunkySheep.Images
{
    [GenerateAuthoringComponent]
    public struct PixelComponent : IBufferElementData
    {
        public Color32 Value;
    }
}
