using UnityEngine;
using System.Collections;

namespace Utility
{

    public static class VectorHelp
    {

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }


    }

    public class transformData
    {
        public transformData(Vector3 _position, Vector3 _rotation)
        {
            position = _position;
            rotation = _rotation;
        }

        public Vector3 position;
        public Vector3 rotation;
    }

}