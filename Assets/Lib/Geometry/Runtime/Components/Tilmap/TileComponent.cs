using System;
using Unity.Entities;

namespace FunkySheep.Geometry
{
    [GenerateAuthoringComponent]
    public struct TileComponent : IComponentData
    {
        public float step;
    }
}
