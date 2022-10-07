using Unity.Entities;
using UnityEngine;

namespace FunkySheep.Images
{
    [GenerateAuthoringComponent]
    public struct PixelComponent : IBufferElementData
    {
        public Color32 Value;
    }
}
