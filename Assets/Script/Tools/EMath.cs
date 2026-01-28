using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Tools
{
    public struct EMath
    {
        // 二阶贝塞尔曲线（二次贝塞尔）
        public static Vector2 CalculateQuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            t = Math.Clamp(t, 0, 1);

            float u = 1 - t;
            float uu = u * u;
            float tt = t * t;

            Vector2 result = uu * p0; // (1-t)² * P0
            result += 2 * u * t * p1; // 2(1-t)t * P1
            result += tt * p2; // t² * P2

            return result;
        }

        // 三阶贝塞尔曲线（三次贝塞尔）
        public static Vector2 CalculateCubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            t = Math.Clamp(t, 0, 1);

            float u = 1 - t;
            float uu = u * u;
            float uuu = uu * u;
            float tt = t * t;
            float ttt = tt * t;

            Vector2 result = uuu * p0; // (1-t)³ * P0
            result += 3 * uu * t * p1; // 3(1-t)²t * P1
            result += 3 * u * tt * p2; // 3(1-t)t² * P2
            result += ttt * p3; // t³ * P3

            return result;
        }

        // 通用n阶贝塞尔曲线（德卡斯特里奥算法）
        public static Vector2 CalculateBezier(List<Vector2> controlPoints, float t)
        {
            if (controlPoints == null || controlPoints.Count < 2)
                Debug.Log("至少需要2个控制点");

            t = Math.Clamp(t, 0, 1);

            List<Vector2> points = new List<Vector2>(controlPoints);

            for (int i = points.Count - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    points[j] = Vector2.Lerp(points[j], points[j + 1], t);
                }
            }

            return points[0];
        }
    }
}