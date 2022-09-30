/*
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
*/