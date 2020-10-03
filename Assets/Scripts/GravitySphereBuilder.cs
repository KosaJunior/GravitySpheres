using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphereBuilder : MonoBehaviour
    {
        [SerializeField] private GravitySphere spherePrefab;

        public GravitySphere Create(Transform parent, float xPositionLimit, float yPositionLimit)
        {
            ValidateSpherePrefabReference();

            var sphere = Instantiate(spherePrefab, parent);
            sphere.gameObject.SetActive(false);
            sphere.Initialize(xPositionLimit, yPositionLimit);

            return sphere;
        }

        private void ValidateSpherePrefabReference()
        {
            if (spherePrefab) return;

            Debug.LogError($"[{nameof(GravitySphereBuilder)}]: sphere prefab not set!");
        }
    }
}