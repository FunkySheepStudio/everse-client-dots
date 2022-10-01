using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;

namespace FunkySheep.Transform
{
    [BurstCompile]
    public struct UpdateDeltaTranslationJob : IJobEntityBatch
    {
        [ReadOnly]
        public ComponentTypeHandle<Translation> translationType;
        public ComponentTypeHandle<DeltaTranslationComponent> deltaTranslationType;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<Translation> translations = batchInChunk.GetNativeArray(this.translationType);
            NativeArray<DeltaTranslationComponent> deltaTranslations = batchInChunk.GetNativeArray(this.deltaTranslationType);

            for (int i = 0; i < batchInChunk.Count; ++i)
            {
                DeltaTranslationComponent deltaTranslation = deltaTranslations[i];
                Translation translation = translations[i];


                deltaTranslation.Value = translation.Value - deltaTranslation.LastTranslation;
                deltaTranslation.LastTranslation = translation.Value;
                deltaTranslations[i] = deltaTranslation;
            }
        }
    }
}
