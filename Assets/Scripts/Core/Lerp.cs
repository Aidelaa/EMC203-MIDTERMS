using UnityEngine;

namespace Core
{
    public abstract class Lerp
    {
        public static Vector3 QuadraticBezier(Vector3 start, Vector3 control,Vector3 end, float t) => Mathf.Pow(1-t,2) * start + (1 - t) * control * 2 + t * t * end;

        public static Vector3 CubicBezier(Vector3 start, Vector3 control1,Vector3 control2, Vector3 end, float t) => Mathf.Pow(1-t,3) * start + (1 - t) * control1 * 3 + Mathf.Pow(1-t,2) * control2 * 3 + t * t * t * end;
    }
}
