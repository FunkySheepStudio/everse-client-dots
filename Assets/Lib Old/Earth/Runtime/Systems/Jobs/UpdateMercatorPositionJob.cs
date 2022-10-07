using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using FunkySheep.Transform;
using Unity.Burst;

namespace FunkySheep.Earth
{
    [BurstCompile]
    public struct UpdateMercatorPositionJob : IJobEntityBatch
    {
        public ComponentTypeHandle<MercatorPositionComponent> mercatorPositionType;
        [ReadOnly]
        public ComponentTypeHandle<DeltaTranslationComponent> deltaTranslationType;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<MercatorPositionComponent> mercatorPositions = batchInChunk.GetNativeArray(this.mercatorPositionType);
            NativeArray<DeltaTranslationComponent> deltaTranslations = batchInChunk.GetNativeArray(this.deltaTranslationType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                MercatorPositionComponent mercatorPosition = mercatorPositions[i];
                DeltaTranslationComponent deltaTranslation = deltaTranslations[i];
                mercatorPosition.Value += new float2
                {
                    x = deltaTranslation.Value.x,
                    y = deltaTranslation.Value.z,
                };

                mercatorPositions[i] = mercatorPosition;
            }
        }
    }
}
