using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;

namespace FunkySheep.Terrain
{
    public partial class SetTransformOnTerrain : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        private EntityQuery query;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            this.query = GetEntityQuery(typeof(Translation), typeof(SetTransformOnTerrainComponentTag));
        }


        protected override void OnUpdate()
        {
            NativeArray<Translation> heightsTranslations = GetEntityQuery(typeof(HeightComponentTag), typeof(Translation)).ToComponentDataArray<Translation>(Allocator.TempJob);

            SetTransformOnTerrainJob setTransformOnTerrainJob = new SetTransformOnTerrainJob
            {
                translationType = GetComponentTypeHandle<Translation>(),
                heightTranslations = heightsTranslations
            };

            JobHandle setHeightsHandle = setTransformOnTerrainJob.ScheduleParallel(this.query, this.Dependency);
            Dependency = JobHandle.CombineDependencies(Dependency, setHeightsHandle, heightsTranslations.Dispose(setHeightsHandle));

            // Remove the init component
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
            this.Dependency = Entities.ForEach((Entity entity, int entityInQueryIndex, in SetTransformOnTerrainComponentTag setTransformOnTerrainComponentTag) =>
            {
                ecb.RemoveComponent<SetTransformOnTerrainComponentTag>(entityInQueryIndex, entity);
            }).ScheduleParallel(setHeightsHandle);


            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}
