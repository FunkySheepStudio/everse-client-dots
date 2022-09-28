using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using FunkySheep.Dots;

namespace FunkySheep.Earth
{
    [UpdateBefore(typeof(UpdateLastTranslation))]
    public partial class UpdateMercatorPosition : SystemBase
    {
        protected override void OnStartRunning()
        {
            Entities.ForEach((ref MercatorPosition mercatorPosition, in GpsPosition gpsPosition) =>
            {
                float2 newMercatorPosition = Utils.toCartesianFloat2(gpsPosition.Value);
                if (!mercatorPosition.Value.Equals(newMercatorPosition))
                {
                    mercatorPosition.Value = newMercatorPosition;
                }
                mercatorPosition.Initial = mercatorPosition.Value;
            }).ScheduleParallel();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((ref MercatorPosition mercatorPosition, in Translation translation, in LastTranslation lastTranslation) =>
            {
                if (!translation.Value.Equals(lastTranslation.Value))
                {
                    mercatorPosition.Value += new float2
                    {
                        x = translation.Value.x - lastTranslation.Value.x,
                        y = translation.Value.z - lastTranslation.Value.z,
                    };
                }

            }).ScheduleParallel();
        }

        void SetMercatorPosition(MercatorPosition mercatorPosition, GpsPosition gpsPosition)
        {
            
        }
    }
}
