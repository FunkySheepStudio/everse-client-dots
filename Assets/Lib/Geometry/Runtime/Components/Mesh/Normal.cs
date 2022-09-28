using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Geometry
{
    /// <summary>
    ///     The buffer of mesh normals.
    /// </summary>
    [InternalBufferCapacity(0)]
    [GenerateAuthoringComponent]
    public struct Normal : IBufferElementData
    {
        /// <summary>
        ///     The normal.
        /// </summary>
        public float3 Value;
    }
}
