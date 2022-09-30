using Unity.Entities;
using FunkySheep.Dots;
using Unity.Jobs;
using Unity.Transforms;

namespace FunkySheep.Earth
{
    [UpdateBefore(typeof(UpdateLastTranslation))]
    public partial class UpdateMercatorPosition : SystemBase
    {
        public JobHandle updateMercatorPositionJobHandle { get; private set; }
        private EntityQuery query;

        protected override void OnCreate()
        {
            this.query = GetEntityQuery(typeof(MercatorPosition), typeof(Translation), typeof(LastTranslation));
        }

        protected override void OnUpdate()
        {
            JobHandle updateLastTranslationJobHandle = World.GetOrCreateSystem<UpdateLastTranslation>().updateLastTranslationJobHandle;
            JobHandle initMercatorPositionJobHandle = World.GetOrCreateSystem<InitMercatorPosition>().initMercatorPositionJobHandle;
            JobHandle updateGpsPositionJobHandle = World.GetOrCreateSystem<UpdateGpsPosition>().updateGpsPositionJobHandle;

            this.Dependency = JobHandle.CombineDependencies(updateLastTranslationJobHandle, initMercatorPositionJobHandle, updateGpsPositionJobHandle);

            UpdateMercatorPositionJob updateMercatorPositionJob = new UpdateMercatorPositionJob
            {
                mercatorPositionType = GetComponentTypeHandle<MercatorPosition>(),
                translationType = GetComponentTypeHandle<Translation>(),
                lastTranslationType = GetComponentTypeHandle<LastTranslation>()
            };

            updateMercatorPositionJobHandle = updateMercatorPositionJob.ScheduleParallel(query, this.Dependency);
            this.Dependency = updateMercatorPositionJobHandle;
        }
    }
}
