using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphereBuilder : MonoBehaviour
    {
        [SerializeField] private GravitySphere spherePrefab;

        public GravitySphere Create(int i, Transform parent, Vector3 position)
        {
            ValidateSpherePrefabReference();
            DisablePrefabGameObject();

            var sphere = Instantiate(spherePrefab, parent);
            sphere.name                               = $"GravitySphere({i})";
            sphere.gameObject.transform.localPosition = position;
            sphere.gameObject.SetActive(false);
            sphere.Initialize();

            return sphere;
        }

        private void ValidateSpherePrefabReference()
        {
            if (spherePrefab) return;

            Debug.LogError($"[{nameof(GravitySphereBuilder)}]: sphere prefab not set!");
        }

        private void DisablePrefabGameObject()
        {
            spherePrefab.gameObject.SetActive(false);
        }
    }
}