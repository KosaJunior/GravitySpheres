using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphereBuilder : MonoBehaviour
    {
        [SerializeField] private GravitySphere spherePrefab;

        public GravitySphere Create(Transform parent, Vector3 position)
        {
            ValidateSpherePrefabReference();

            var sphere = Instantiate(spherePrefab, parent);
            sphere.gameObject.transform.localPosition = position;
            sphere.gameObject.SetActive(false);

            return sphere;
        }

        private void ValidateSpherePrefabReference()
        {
            if (spherePrefab) return;

            Debug.LogError($"[{nameof(GravitySphereBuilder)}]: sphere prefab not set!");
        }
    }
}