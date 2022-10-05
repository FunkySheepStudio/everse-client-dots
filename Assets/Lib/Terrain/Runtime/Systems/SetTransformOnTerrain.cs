using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using FunkySheep.Geometry;
using Unity.Jobs;

namespace FunkySheep.Terrain
{
    public partial class SetTransformOnTerrain : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        }


        protected override void OnUpdate()
        {
            NativeArray<Translation> heightsTranslations = GetEntityQuery(typeof(HeightComponentTag), typeof(Translation)).ToComponentDataArray<Translation>(Allocator.TempJob);
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, in SetTransformOnTerrainComponentTag setTransformOnTerrainComponentTag) =>
            {
                int closeHeightAIndex = 0;
                int closeHeightBIndex = 0;
                int closeHeightCIndex = 0;

                float2 currentPoint = new float2
                {
                    x = translation.Value.x,
                    y = translation.Value.z,
                };

                for (int i = 1; i < heightsTranslations.Length; i++)
                {
                    float2 currentClosest = new float2
                    {
                        x = heightsTranslations[closeHeightAIndex].Value.x,
                        y = heightsTranslations[closeHeightAIndex].Value.z,
                    };

                    float2 tryClosest = new float2
                    {
                        x = heightsTranslations[i].Value.x,
                        y = heightsTranslations[i].Value.z,
                    };

                    // Insert the newest point if closer tha the old one
                    if (!Utils.ClosestPoint(currentPoint, currentClosest, tryClosest).Equals(currentClosest))
                    {
                        closeHeightCIndex = closeHeightBIndex;
                        closeHeightBIndex = closeHeightAIndex;
                        closeHeightAIndex = i;
                    }

                }

                translation.Value.y = heightsTranslations[closeHeightAIndex].Value.y + 10;

                ecb.RemoveComponent<SetTransformOnTerrainComponentTag>(entityInQueryIndex, entity);

            })
            .Schedule();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);

            Dependency = JobHandle.CombineDependencies(Dependency, heightsTranslations.Dispose(Dependency));
        }

        
    }
}
