using Unity.Entities;
using FunkySheep.Transform;
using Unity.Jobs;
using Unity.Transforms;

namespace FunkySheep.Earth
{
    [UpdateBefore(typeof(UpdateDeltaTranslation))]
    public partial class UpdateMercatorPosition : SystemBase
    {
        public JobHandle updateMercatorPositionJobHandle { get; private set; }
        private EntityQuery query;

        protected override void OnCreate()
        {
            this.query = GetEntityQuery(typeof(MercatorPositionComponent), typeof(DeltaTranslationComponent));
        }

        protected override void OnUpdate()
        {
            JobHandle updateLastTranslationJobHandle = World.GetOrCreateSystem<UpdateDeltaTranslation>().updateDeltaTranslationJobHandle;
            JobHandle initMercatorPositionJobHandle = World.GetOrCreateSystem<InitMercatorPosition>().initMercatorPositionJobHandle;
            JobHandle updateGpsPositionJobHandle = World.GetOrCreateSystem<UpdateGpsPosition>().updateGpsPositionJobHandle;

            this.Dependency = JobHandle.CombineDependencies(updateLastTranslationJobHandle, initMercatorPositionJobHandle, updateGpsPositionJobHandle);

            UpdateMercatorPositionJob updateMercatorPositionJob = new UpdateMercatorPositionJob
            {
                mercatorPositionType = GetComponentTypeHandle<MercatorPositionComponent>(),
                deltaTranslationType = GetComponentTypeHandle<DeltaTranslationComponent>()
            };

            updateMercatorPositionJobHandle = updateMercatorPositionJob.ScheduleParallel(query, this.Dependency);
            this.Dependency = updateMercatorPositionJobHandle;
        }
    }
}
