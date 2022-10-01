using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Transform
{
    [GenerateAuthoringComponent]
    public struct DeltaTranslationComponent : IComponentData
    {
        public float3 LastTranslation;
        public float3 Value;
    }
}
