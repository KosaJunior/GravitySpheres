using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphere : MonoBehaviour
    {
        public GravitySphere Create(Transform parent, Vector3 worldPosition)
        {
            var sphere = Instantiate(this, parent);
            sphere.gameObject.transform.position = worldPosition;
            sphere.gameObject.SetActive(false);

            return sphere;
        }
    }
}