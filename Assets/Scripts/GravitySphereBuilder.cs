using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphereBuilder : MonoBehaviour
    {
        [SerializeField] private GravitySphere          spherePrefab;
        [SerializeField] private SpheresCreatorSettings settings;

        public GravitySphere Create(int i, Transform parent, Vector3 position)
        {
            if (ValidateSpherePrefabReference() == false)
                return null;

            DisablePrefabGameObject();

            var sphere = Instantiate(spherePrefab, parent);
            sphere.name                               = $"GravitySphere({i})";
            sphere.gameObject.transform.localPosition = position;
            sphere.gameObject.SetActive(false);
            sphere.Initialize();

            return sphere;
        }

        private bool ValidateSpherePrefabReference()
        {
            bool isPrefabSet = spherePrefab;
            if (isPrefabSet) return true;

            Debug.LogError($"[{nameof(GravitySphereBuilder)}]: Sphere prefab not set!");

            return false;
        }

        private void DisablePrefabGameObject()
        {
            spherePrefab.gameObject.SetActive(false);
        }
    }
}