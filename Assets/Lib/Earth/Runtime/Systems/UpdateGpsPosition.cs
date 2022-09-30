using Unity.Entities;
using FunkySheep.Dots;
using Unity.Jobs;

namespace FunkySheep.Earth
{
    [UpdateAfter(typeof(UpdateMercatorPosition))]
    public partial class UpdateGpsPosition : SystemBase
    {
        public JobHandle updateGpsPositionJobHandle { get; private set; }
        private EntityQuery query;

        protected override void OnCreate()
        {
            this.query = GetEntityQuery(typeof(GpsPosition), typeof(MercatorPosition), typeof(LastTranslation));
        }

        protected override void OnUpdate()
        {
            JobHandle UpdateMercatorPositionJobHandle = World.GetOrCreateSystem<UpdateMercatorPosition>().updateMercatorPositionJobHandle;
            JobHandle initMercatorPositionJobHandle = World.GetOrCreateSystem<InitMercatorPosition>().initMercatorPositionJobHandle;

            this.Dependency = JobHandle.CombineDependencies(UpdateMercatorPositionJobHandle, initMercatorPositionJobHandle);

            UpdateGpsPositionJob updateGpsPositionJob = new UpdateGpsPositionJob
            {
                gpsPositionType = GetComponentTypeHandle<GpsPosition>(),
                mercatorPositionType = GetComponentTypeHandle<MercatorPosition>()
            };

            updateGpsPositionJobHandle = updateGpsPositionJob.ScheduleParallel(query, this.Dependency);
            this.Dependency = updateGpsPositionJobHandle;
        }
    }
}
