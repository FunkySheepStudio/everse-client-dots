using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Dots
{
    [GenerateAuthoringComponent]
    public struct LastTranslation : IComponentData
    {
        public float3 Value;
    }
}
