using Unity.Entities;
using Unity.Transforms;
using FunkySheep.Transforms;

namespace FunkySheep.Earth
{
    public partial class UpdateGridPosition : SystemBase
    {
        protected override void OnUpdate()
        {
            float tileSize = Manager.Instance.tileSize.value;
            Entities.ForEach((ref EarthGridPosition earthGridPosition, in Translation translation, in TransformDynamicTranslationTag transformDynamicTranslationTag) =>
            {
                earthGridPosition.Value =  Manager.GetTilePosition(translation.Value, tileSize);
            }).ScheduleParallel();
        }
    }
}
