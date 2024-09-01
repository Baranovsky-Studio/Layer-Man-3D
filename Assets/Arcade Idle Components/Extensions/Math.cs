using UnityEngine;

namespace Extensions
{
    public class Math
    {
        public static Quaternion LookAt(Vector3 point, Transform transform)
        {
            //Get the direction to point
            var direction = (point - transform.position).normalized;
            //Create a face direction, without changing y-axis
            var faceDirection = new Vector3(direction.x, point.y, direction.z);
            //Convert direction to quaternion
            return Quaternion.LookRotation(faceDirection);
        }
    }
}