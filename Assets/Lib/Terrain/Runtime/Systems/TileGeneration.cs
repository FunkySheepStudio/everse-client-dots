using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Terrain
{
    public partial class TileGeneration : SystemBase
    {
        protected override void OnStartRunning()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entities.ForEach((Entity entity, ref TileComponent tileComponent) =>
            {
                for (int x = 0; x < tileComponent.resolution; x++)
                {
                    for (int z = 0; z < tileComponent.resolution; z++)
                    {
                        DynamicBuffer<VerticeComponent> buffer = entityManager.GetBuffer<VerticeComponent>(entity);
                        buffer.Add(new VerticeComponent()
                        {
                            position = new float3(x, 0, z),
                            position1 = new float3(x, 0, z + 1),
                            position2 = new float3(x + 1, 0, z)
                        });
                    }
                }
            }).Schedule();

            this.CompleteDependency();
        }


        protected override void OnUpdate()
        {
            
        }
    }
}