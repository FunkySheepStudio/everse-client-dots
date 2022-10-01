using Unity.Entities;
using Unity.Jobs;

namespace FunkySheep.Earth
{
    [UpdateBefore(typeof(UpdateMercatorPosition))]
    public partial class InitMercatorPosition : SystemBase
    {
        public JobHandle initMercatorPositionJobHandle { get; private set; }
        private EntityQuery query;
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            this.query = GetEntityQuery(typeof(MercatorPositionComponent), typeof(GpsPositionComponent), typeof(InitMercatorPositionComponentTag));
        }

        protected override void OnUpdate()
        {
            JobHandle updateGpsPositionJobHandle = World.GetOrCreateSystem<UpdateGpsPosition>().updateGpsPositionJobHandle;
            JobHandle updateMercatorPositionJobHandle = World.GetOrCreateSystem<UpdateMercatorPosition>().updateMercatorPositionJobHandle;

            this.Dependency = JobHandle.CombineDependencies(updateMercatorPositionJobHandle, updateMercatorPositionJobHandle);

            InitMercatorPositionJob initMercatorPosition = new InitMercatorPositionJob
            {
                mercatorPositionType = GetComponentTypeHandle<MercatorPositionComponent>(),
                gpsPositionType = GetComponentTypeHandle<GpsPositionComponent>()
            };

            initMercatorPositionJobHandle = initMercatorPosition.ScheduleParallel(query, this.Dependency);

            // Remove the init component
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
            this.Dependency = Entities.ForEach((Entity entity, int entityInQueryIndex, in InitMercatorPositionComponentTag initMercatorPositionComponentTag) =>
            {
                ecb.RemoveComponent<InitMercatorPositionComponentTag>(entityInQueryIndex, entity);
            }).ScheduleParallel(initMercatorPositionJobHandle);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);

            this.Dependency = JobHandle.CombineDependencies(this.Dependency, initMercatorPositionJobHandle);
        }
    }
}
