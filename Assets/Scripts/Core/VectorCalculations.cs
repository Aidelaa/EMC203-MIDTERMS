using UnityEngine;

namespace Core
{
    public class VectorCalculations : MonoBehaviour
    {
        public static float Dot(Vector3 start, Vector3 end) => start.x * end.x + start.y * end.y + start.z * end.z;

        public static float Cross(Vector3 start, Vector3 end) => (start.y - end.x) * (start.x - end.y);

        public static Vector3 DotV3(Vector3 start, Vector3 end) => new Vector3((start.x - end.x), (start.y - end.y), (start.z - end.z));

        public static Vector3 CrossV3(Vector3 start, Vector3 end)
        {
            float x = start.y * end.z - start.z * end.y;
            float y = start.z * end.x - start.x * end.z;
            float z = start.x * end.y - start.y * end.x;
            
            return new Vector3(x, y, z);
        }

        public static Vector2 DotV2(Vector2 start, Vector2 end) => new Vector2(start.x - end.x, start.y - end.y);

        public static float LookAt(Vector3 start, Vector3 end)
        {
            Vector2 direction = DotV2(start, end);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            return angle;
        }
    }
}
