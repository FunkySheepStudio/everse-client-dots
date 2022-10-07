using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Player
{
    [AddComponentMenu("Game/Player/Manager")]
    public class Manager : MonoBehaviour, IConvertGameObjectToEntity
    {
        public FunkySheep.Types.Double initialLatitude;
        public FunkySheep.Types.Double initialLongitude;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.SetComponentData<Translation>(entity, new Translation
            {
                Value = FunkySheep.Earth.Manager.GetWorldPosition(initialLatitude.value, initialLongitude.value)
            });
        }
    }
}
