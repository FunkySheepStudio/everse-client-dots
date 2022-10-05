using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Maps;
using Unity.Transforms;

namespace FunkySheep.OSM.Ecs
{
    public partial class ConvertNodeGpsPositionToWorldPosition : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            MapSingletonComponent mapSingleton = GetSingleton<MapSingletonComponent>();
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, in OsmNodeGpsPosition nodeGpsPosition) =>
            {

                float2 worldPosition = CalculateWorldPositionFromGpsCoordinates(
                    mapSingleton.zoomLevel,
                    mapSingleton.tileSize,
                    nodeGpsPosition.Value.x,
                    nodeGpsPosition.Value.y,
                    mapSingleton.initialMapPosition
                );

                ecb.SetComponent<Translation>(entityInQueryIndex, entity, new Translation {
                    Value = new float3
                    {
                        x = worldPosition.x,
                        y = 0,
                        z = worldPosition.y
                    }
                });

                ecb.RemoveComponent<OsmNodeGpsPosition>(entityInQueryIndex, entity);
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }

        private static float2 CalculateWorldPositionFromGpsCoordinates(int zoomLevel, float tileSize, double latitude, double longitude, float2 initialMapPosition)
        {
            float2 position = Utils.GpsToMapReal(
              zoomLevel,
              latitude,
              longitude,
              initialMapPosition
            );

            position.y = -position.y;
            position *= tileSize;

            return position;
        }
    }
}
