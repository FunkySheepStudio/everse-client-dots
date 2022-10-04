using Unity.Entities;
using FunkySheep.Maps;

namespace Game.Player
{
    public partial class ActivatePlayerTiles : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            PlayerComponent player = GetSingleton<PlayerComponent>();
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, in TilePositionComponent tilePositionComponent, in DeactivatePlayerTileTag deactivatePlayerTileTag, in Disabled disabled) =>
            {
                if (tilePositionComponent.Value.Equals(player.tilePosition))
                {
                    ecb.RemoveComponent<Disabled>(entityInQueryIndex, entity);
                }
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }

}
