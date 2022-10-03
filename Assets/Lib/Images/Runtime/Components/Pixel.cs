using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace FunkySheep.Images
{
    [GenerateAuthoringComponent]
    public struct Pixel : IComponentData
    {
        public Color32 Value;
    }
}
