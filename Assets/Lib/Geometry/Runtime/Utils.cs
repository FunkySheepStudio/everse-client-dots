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

        public static bool PointsAreAligned(double2 pointA, double2 pointB, double2 pointC)
        {
            double2 vectAB = new double2
            {
                x = pointB.x - pointA.x,
                y = pointB.y - pointA.y
            };

            double2 vectAC = new double2
            {
                x = pointC.x - pointA.x,
                y = pointC.y - pointA.y
            };

            if (vectAB.x * vectAC.y - vectAC.x * vectAB.y == 0)
            {
                return true;
            } else
            {
                Debug.Log((180 / math.PI) * math.atan2(vectAB.y - vectAC.y, vectAB.x - vectAC.x));
                return false;
            }
        }

        public static double AngleBetween(double2 pointA, double2 pointB, double2 pointC)
        {
            double2 vectAB = new double2
            {
                x = pointB.x - pointA.x,
                y = pointB.y - pointA.y
            };

            double2 vectBC = new double2
            {
                x = pointB.x - pointC.x,
                y = pointB.y - pointC.y
            };


            return (180 / math.PI) * math.atan2(vectAB.y - vectBC.y, vectAB.x - vectBC.x);
        }
    }
}
