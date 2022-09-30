using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;

namespace FunkySheep.Dots
{
    [BurstCompile]
    public struct UpdateLastPositionJob : IJobEntityBatch
    {
        [ReadOnly]
        public ComponentTypeHandle<Translation> translationType;
        public ComponentTypeHandle<LastTranslation> lastTranslationType;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<Translation> translations = batchInChunk.GetNativeArray(this.translationType);
            NativeArray<LastTranslation> lastTranslations = batchInChunk.GetNativeArray(this.lastTranslationType);

            for (int i = 0; i < batchInChunk.Count; ++i)
            {
                LastTranslation lastTranslation = lastTranslations[i];
                Translation translation = translations[i];

                // Modify
                if (!translation.Value.Equals(lastTranslation.Value))
                {
                    lastTranslation.Value = translation.Value;
                    lastTranslations[i] = lastTranslation;
                }
            }
        }
    }
}
