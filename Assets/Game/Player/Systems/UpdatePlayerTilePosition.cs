using Unity.Entities;
using FunkySheep.Maps;

namespace Game.Player
{
    public partial class UpdatePlayerTilePosition : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref PlayerComponent player, in TilePositionComponent tilePosition) =>
            {
                player.tilePosition = tilePosition.Value;
            }).ScheduleParallel();
        }
    }
}
