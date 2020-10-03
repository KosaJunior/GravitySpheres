using GravitySpheres.Scripts.Utilities;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class SphereCreator : MonoBehaviour
    {
        #region Variables

        public event System.Action<int> OnVisibleSpheresCountChange;

        [SerializeField] private GravitySpheresSettings settings;
        [SerializeField] private Transform              spheresPoolTransform;

        private GravitySphere[] spheresPool;
        private int             nextSphereIndex = 0;

        private float sphereShowDelay = 0f;

        #endregion variables

        #region Constructor & inits

        private void Awake()
        {
            if (ValidateReferences())
                CreateSpheresPool();
        }

        private bool ValidateReferences()
        {
            bool areSettingsSet      = settings;
            bool arePoolTransformSet = spheresPoolTransform;
            if (areSettingsSet && arePoolTransformSet)
                return true;

            if (areSettingsSet == false)
                Debug.LogError("Settings unavailable!");

            if (arePoolTransformSet == false)
                Debug.LogError("SpheresPoolTransform unavailable!");

            Debug.LogError("Aborting...");
            AppUtility.Quit();

            return false;
        }

        private void CreateSpheresPool()
        {
            spheresPool = new GravitySphere[settings.SpheresLimit];

            for (int i = 0; i < spheresPool.Length; i++)
                spheresPool[i] = CreateSphere();
        }

        private GravitySphere CreateSphere()
        {
            var sphere = Instantiate(settings.SpherePrefab, spheresPoolTransform);
            sphere.gameObject.SetActive(false);

            return sphere;
        }

        #endregion constructor & inits

        #region Private methods

        private void Update()
        {
            TryShowSphere();
        }

        private void TryShowSphere()
        {
            CalculateSphereShowDelay();

            if (IsTimeToShowSphere() == false)
                return;

            ShowSphere();
            ResetSphereShowDelay();
        }

        private void CalculateSphereShowDelay() => sphereShowDelay += Time.deltaTime;

        private bool IsTimeToShowSphere() => sphereShowDelay >= settings.CreateDelay;

        private void ShowSphere()
        {
            spheresPool[nextSphereIndex].gameObject.SetActive(true);
            OnVisibleSpheresCountChange?.Invoke(nextSphereIndex++);

            if (AreAllSpheresVisible())
                DisableDisplaying();
        }

        private bool AreAllSpheresVisible() => nextSphereIndex == spheresPool.Length;

        private void DisableDisplaying() => enabled = false;

        private void ResetSphereShowDelay() => sphereShowDelay = 0f;

        #endregion private methods
    }
}