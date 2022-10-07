using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;

namespace FunkySheep.Transform
{
    [DisableAutoCreation]
    public partial class UpdateDeltaTranslation : SystemBase
    {
        public JobHandle updateDeltaTranslationJobHandle;
        private EntityQuery query;

        protected override void OnCreate()
        {
            this.query = GetEntityQuery(typeof(DeltaTranslationComponent), typeof(Translation));
        }

        protected override void OnUpdate()
        {
            UpdateDeltaTranslationJob job = new UpdateDeltaTranslationJob()
            {
                translationType = GetComponentTypeHandle<Translation>(),
                deltaTranslationType = GetComponentTypeHandle<DeltaTranslationComponent>(),
            };

            updateDeltaTranslationJobHandle = job.ScheduleParallel(this.query, this.Dependency);
            this.Dependency = updateDeltaTranslationJobHandle;
        }
    }
}
