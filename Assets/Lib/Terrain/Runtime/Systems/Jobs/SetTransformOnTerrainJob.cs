using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using FunkySheep.Geometry;
using Unity.Burst;

namespace FunkySheep.Terrain
{
    [BurstCompile]
    public struct SetTransformOnTerrainJob : IJobEntityBatch
    {
        public ComponentTypeHandle<Translation> translationType;
        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<Translation> heightTranslations;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<Translation> translations = batchInChunk.GetNativeArray(this.translationType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                Translation translation = translations[i];

                int closeHeightAIndex = 0;
                int closeHeightBIndex = 0;
                int closeHeightCIndex = 0;

                float2 currentPoint = new float2
                {
                    x = translation.Value.x,
                    y = translation.Value.z,
                };

                for (int j = 1; j < heightTranslations.Length; j++)
                {
                    float2 currentClosest = new float2
                    {
                        x = heightTranslations[closeHeightAIndex].Value.x,
                        y = heightTranslations[closeHeightAIndex].Value.z,
                    };

                    float2 tryClosest = new float2
                    {
                        x = heightTranslations[j].Value.x,
                        y = heightTranslations[j].Value.z,
                    };

                    // Insert the newest point if closer tha the old one
                    if (!Utils.ClosestPoint(currentPoint, currentClosest, tryClosest).Equals(currentClosest))
                    {
                        closeHeightCIndex = closeHeightBIndex;
                        closeHeightBIndex = closeHeightAIndex;
                        closeHeightAIndex = j;
                    }
                }

                translation.Value.y = heightTranslations[closeHeightAIndex].Value.y + 10;
                translations[i] = translation;
            }
        }
    }
}
