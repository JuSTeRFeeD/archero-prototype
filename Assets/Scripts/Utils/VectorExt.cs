using UnityEngine;

namespace Utils
{
    public static class VectorExt
    {
        public static Vector3 NormalizedDirectionTo(this Vector3 from, Vector3 to)
        {
            var vec = (to - from).normalized;
            vec.y = 0;
            return vec;
        }
        
        public static Vector3 NormalizedDirectionTo(this Transform from, Transform to)
        {
            var vec = (to.position - from.position).normalized;
            vec.y = 0;
            return vec;
        }
    }
}