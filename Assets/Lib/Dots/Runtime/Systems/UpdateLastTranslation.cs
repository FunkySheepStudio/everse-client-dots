using Unity.Entities;
using Unity.Transforms;

namespace FunkySheep.Dots
{
    public partial class UpdateLastTranslation : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref LastTranslation lastTranslation, in Translation translation) =>
            {
                if (!translation.Value.Equals(lastTranslation.Value))
                {
                    lastTranslation.Value = translation.Value;
                }
            }).ScheduleParallel();
        }
    }

}
