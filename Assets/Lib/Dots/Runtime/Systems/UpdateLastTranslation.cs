using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;

namespace FunkySheep.Dots
{
    public partial class UpdateLastTranslation : SystemBase
    {
        public JobHandle updateLastTranslationJobHandle;
        private EntityQuery query;

        protected override void OnCreate()
        {
            this.query = GetEntityQuery(typeof(LastTranslation), typeof(Translation));
        }

        protected override void OnUpdate()
        {
            UpdateLastPositionJob job = new UpdateLastPositionJob()
            {
                translationType = GetComponentTypeHandle<Translation>(),
                lastTranslationType = GetComponentTypeHandle<LastTranslation>(),
            };

            updateLastTranslationJobHandle = job.ScheduleParallel(this.query, this.Dependency);
            this.Dependency = updateLastTranslationJobHandle;
        }
    }
}
