using Unity.Entities;
using Unity.Mathematics;

namespace Game.Player
{
    [GenerateAuthoringComponent]
    public struct PlayerComponent : IComponentData
    {
        public int2 tilePosition;        
    }
}
