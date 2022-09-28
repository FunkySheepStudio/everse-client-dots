using Unity.Entities;
using Unity.Transforms;
using FunkySheep.Dots;

namespace FunkySheep.Earth
{
    [UpdateAfter(typeof(UpdateMercatorPosition))]
    public partial class UpdateGpsPosition : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref GpsPosition gpsPosition, in Translation translation, in LastTranslation lastTranslation, in MercatorPosition mercatorPosition) =>
            {
                gpsPosition.Value = Utils.toGeoCoordDouble2(mercatorPosition.Value);

            }).ScheduleParallel();
        }
    }
}
