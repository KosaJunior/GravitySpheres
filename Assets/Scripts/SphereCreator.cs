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
        [Space]
        [SerializeField] private GravitySpherePositionRandomizer positionRandomizer;

        private GravitySphere[] spheresPool;
        private int             nextSphereIndex = 0;

        private float sphereShowDelay = 0f;

        #endregion variables

        #region Constructor & inits

        private void Awake()
        {
            if (ValidateReferences() == false)
                return;

            Initialize();
        }

        private bool ValidateReferences()
        {
            bool areSettingsSet          = settings;
            bool isPoolTransformSet      = spheresPoolTransform;
            bool isPositionRandomizerSet = positionRandomizer;
            if (areSettingsSet && isPoolTransformSet && isPositionRandomizerSet)
                return true;

            if (areSettingsSet == false)
                Debug.LogError("Settings unavailable!");

            if (isPoolTransformSet == false)
                Debug.LogError("SpheresPoolTransform unavailable!");

            if (positionRandomizer == false)
                Debug.LogError("PositionRandomizer unavailable!");

            Debug.LogError("Aborting...");
            AppUtility.Quit();

            return false;
        }

        private void Initialize()
        {
            positionRandomizer.Initialize();
            CreateSpheresPool();
        }

        private void CreateSpheresPool()
        {
            spheresPool = new GravitySphere[settings.SpheresLimit];

            for (int i = 0; i < spheresPool.Length; i++)
                spheresPool[i] = settings.SpherePrefab.Create(spheresPoolTransform, positionRandomizer.GetRandomPosition());
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