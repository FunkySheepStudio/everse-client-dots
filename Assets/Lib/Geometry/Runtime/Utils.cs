using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

namespace FunkySheep.Geometry
{
    [BurstCompile]
    public class Utils
    {
        /// <summary>
        /// Find the closest point from a point between 2 points
        /// </summary>
        /// <param name="point"></param>
        /// <param name="comparisontPointA"></param>
        /// <param name="comparisontPointB"></param>
        /// <returns></returns>
        public static float2 ClosestPoint(float2 point, float2 comparisontPointA, float2 comparisontPointB)
        {
            float distA = math.distance(point, comparisontPointA);
            float distB = math.distance(point, comparisontPointB);

            if (distA < distB)
            {
                return comparisontPointA;
            }
            else
            {
                return comparisontPointB;
            }

        }

        /// <summary>
        /// Find the closest point from a point between 2 points
        /// </summary>
        /// <param name="point"></param>
        /// <param name="comparisontPointA"></param>
        /// <param name="comparisontPointB"></param>
        /// <returns></returns>
        public static float3 ClosestPoint(float3 point, float3 comparisontPointA, float3 comparisontPointB)
        {
            float distA = math.distance(point, comparisontPointA);
            float distB = math.distance(point, comparisontPointB);

            if (distA < distB)
            {
                return comparisontPointA;
            }
            else
            {
                return comparisontPointB;
            }

        }
    }
}
