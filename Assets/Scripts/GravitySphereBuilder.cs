using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphereBuilder : MonoBehaviour
    {
        [SerializeField] private GravitySphere          spherePrefab;
        [SerializeField] private GravitySpheresSettings settings;

        public GravitySphere Create(int i, Transform parent, Vector3 position)
        {
            if (ValidateSpherePrefabReference() == false)
                return null;

            DisablePrefabGameObject();

            var sphere = Instantiate(spherePrefab, parent);
            sphere.name                               = $"GravitySphere({i})";
            sphere.gameObject.transform.localPosition = position;
            sphere.gameObject.SetActive(false);
            sphere.Initialize(settings.SpheresToBreakup);

            return sphere;
        }

        private bool ValidateSpherePrefabReference()
        {
            bool isPrefabSet    = spherePrefab;
            bool areSettingsSet = settings;
            if (isPrefabSet && areSettingsSet)
                return true;

            if (isPrefabSet == false)
                Debug.LogError($"[{nameof(GravitySphereBuilder)}]: Sphere prefab not set!");

            if (areSettingsSet == false)
                Debug.LogError($"[{nameof(GravitySphereBuilder)}]: Settings are not set!");

            return false;
        }

        private void DisablePrefabGameObject()
        {
            spherePrefab.gameObject.SetActive(false);
        }
    }
}