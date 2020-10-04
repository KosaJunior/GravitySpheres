using GravitySpheres.Scripts.Utilities;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class SphereCreator : MonoBehaviour
    {
        #region Variables

        public event System.Action<int> OnVisibleSpheresCountChange;

        [SerializeField] private GravitySpheresSettings settings;
        [Space]
        [SerializeField] private BoxCollider spawnArea;

        private GravitySphere[] spheresPool;
        private int             nextSphereIndex = 0;

        private float sphereShowDelay = 0f;

        #endregion variables

        #region Constructor & inits

        private void Awake()
        {
            if (ValidateReferences() == false)
                return;

            CreateSpheresPool();
        }

        private bool ValidateReferences()
        {
            bool areSettingsSet = settings;
            bool isAreaSet      = spawnArea;
            if (areSettingsSet && isAreaSet)
                return true;

            if (areSettingsSet == false)
                Debug.LogError("Settings are not set!");

            if (isAreaSet == false)
                Debug.LogError("SpawnArea are not set!");

            Debug.LogError("Aborting...");
            AppUtility.Quit();

            return false;
        }

        private void CreateSpheresPool()
        {
            spheresPool = new GravitySphere[settings.SpheresLimit];

            for (int i = 0; i < spheresPool.Length; i++)
                spheresPool[i] = settings.SphereBuilder.Create(spawnArea.transform, GetRandomPositionInsideArea());
        }

        private Vector3 GetRandomPositionInsideArea()
        {
            return spawnArea.center + new Vector3(
                (Random.value - 0.5f) * spawnArea.size.x,
                (Random.value - 0.5f) * spawnArea.size.y,
                (Random.value - 0.5f) * spawnArea.size.z
            );
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
            OnVisibleSpheresCountChange?.Invoke(++nextSphereIndex);

            if (AreAllSpheresVisible())
                DisableDisplaying();
        }

        private bool AreAllSpheresVisible() => nextSphereIndex == spheresPool.Length;

        private void DisableDisplaying() => enabled = false;

        private void ResetSphereShowDelay() => sphereShowDelay = 0f;

        #endregion private methods
    }
}