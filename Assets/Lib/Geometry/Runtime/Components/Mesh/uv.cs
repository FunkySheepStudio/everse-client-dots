using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Geometry
{
    /// <summary>
    ///     The buffer of mesh uvs.
    /// </summary>
    [InternalBufferCapacity(0)]
    [GenerateAuthoringComponent]
    public struct Uv : IBufferElementData
    {
        /// <summary>
        ///     The uv.
        /// </summary>
        public float3 Value;
    }
}
