using UnityEngine;
using Unity.Entities;
using FunkySheep.Images;

namespace FunkySheep.Terrain
{
    public partial class UpdateTileHeights : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref DynamicBuffer<TileHeights> tileHeights, in DynamicBuffer<PixelComponent> pixelComponents) =>
            {
                for (int x = 0; x < Mathf.Sqrt(pixelComponents.Length); x++)
                {
                    for (int y = 0; y < Mathf.Sqrt(pixelComponents.Length); y++)
                    {

                    }
                }
            }).ScheduleParallel();
        }
    }
}
