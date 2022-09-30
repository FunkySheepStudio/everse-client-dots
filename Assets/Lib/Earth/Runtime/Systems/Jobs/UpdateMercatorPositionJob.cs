using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using FunkySheep.Dots;

namespace FunkySheep.Earth
{
    public struct UpdateMercatorPositionJob : IJobEntityBatch
    {
        public ComponentTypeHandle<MercatorPosition> mercatorPositionType;
        [ReadOnly]
        public ComponentTypeHandle<Translation> translationType;
        [ReadOnly]
        public ComponentTypeHandle<LastTranslation> lastTranslationType;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<MercatorPosition> mercatorPositions = batchInChunk.GetNativeArray(this.mercatorPositionType);
            NativeArray<Translation> translations = batchInChunk.GetNativeArray(this.translationType);
            NativeArray<LastTranslation> lastTranslations = batchInChunk.GetNativeArray(this.lastTranslationType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                MercatorPosition mercatorPosition = mercatorPositions[i];
                Translation translation = translations[i];
                LastTranslation lastTranslation = lastTranslations[i];
                if (!translation.Equals(lastTranslation.Value))
                {
                    mercatorPosition.Value += new float2
                    {
                        x = translation.Value.x - lastTranslation.Value.x,
                        y = translation.Value.z - lastTranslation.Value.z,
                    };

                    mercatorPositions[i] = mercatorPosition;
                }
            }
        }
    }
}
