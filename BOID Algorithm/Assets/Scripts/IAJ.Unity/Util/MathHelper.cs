﻿using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Util
{
    public static class MathHelper
    {
        public static Vector3 ConvertOrientationToVector(float orientation)
        {
            return new Vector3((float)Math.Sin(orientation), 0, (float)Math.Cos(orientation));
        }

        public static float ConvertVectorToOrientation(Vector3 vector)
        {
            return Mathf.Atan2(vector.x, vector.z);
        }

        public static Vector3 Rotate2D(Vector3 vector, float angle)
        {
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);

            var x = vector.x*cos - vector.z*sin;
            var z = vector.x*sin + vector.z*cos;
            return new Vector3(x,vector.y,z);
        }

        public static float ShortestAngleDifference(float source, float target)
        {
            var delta = source - target;
            if (delta > MathConstants.MATH_PI)
                delta -= 360;
            else if (delta < -MathConstants.MATH_PI)
                delta += 360;
            return delta;
        }

    }
}
