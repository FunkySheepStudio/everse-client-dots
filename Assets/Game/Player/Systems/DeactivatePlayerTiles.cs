using Unity.Entities;
using FunkySheep.Maps;

namespace Game.Player
{
    public partial class DeactivatePlayerTiles : SystemBase
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

            Entities.ForEach((Entity entity, int entityInQueryIndex, in TilePositionComponent tilePositionComponent, in DeactivatePlayerTileTag deactivatePlayerTileTag) =>
            {
                if (!tilePositionComponent.Value.Equals(player.tilePosition))
                {
                    ecb.AddComponent<Disabled>(entityInQueryIndex, entity);
                } else
                {
                    ecb.RemoveComponent<Disabled>(entityInQueryIndex, entity);
                }
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }

}
