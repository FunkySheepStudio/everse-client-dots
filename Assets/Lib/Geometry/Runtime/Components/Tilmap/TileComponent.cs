using System;
using Unity.Entities;

namespace FunkySheep.Geometry
{
    [GenerateAuthoringComponent]
    public struct TileComponent : IComponentData
    {
        public int size;
        public int count;
    }
}
