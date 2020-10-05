﻿using GravitySpheres.Scripts.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GravitySpheres.Scripts
{
    public class SphereCreator : MonoBehaviour
    {
        #region Variables

        public event  System.Action<int> OnVisibleSpheresCountChange;
        private event System.Action      OnReverseGravityStarted;

        [SerializeField] private GravitySphereBuilder builder;
        [Space]
        [SerializeField] private SpheresCreatorSettings  settings;
        [SerializeField] private SphereCombineController sphereCombineController;
        [Space]
        [SerializeField] private float zOffsetFromCameraFarClip = 250f;
        [SerializeField] private Camera camera;

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
            bool areBuilderSet                = builder;
            bool areSettingsSet               = settings;
            bool isSphereCombineControllerSet = sphereCombineController != null;
            bool isCameraSet                  = camera;
            if (builder && areSettingsSet && isSphereCombineControllerSet && isCameraSet)
                return true;

            if (areBuilderSet == false)
                Debug.LogError("Builder are not set!");

            if (areSettingsSet == false)
                Debug.LogError("Settings are not set!");

            if (isSphereCombineControllerSet == false)
                Debug.LogError("SphereCombineController are not set!");

            if (isCameraSet == false)
                Debug.LogError("Camera are not set!");

            Debug.LogError("Aborting...");
            AppUtility.Quit();

            return false;
        }

        private void CreateSpheresPool()
        {
            spheresPool = new GravitySphere[settings.SpheresLimit];

            for (int i = 0; i < spheresPool.Length; i++)
            {
                var gravitySphere = builder.Create(i, transform, GetRandomPositionInsideArea());
                if (gravitySphere == false) return;

                OnReverseGravityStarted += gravitySphere.GravityField.InvertGravity;
                sphereCombineController.SubscribeToCollisionEvent(gravitySphere);
                spheresPool[i] = gravitySphere;
            }
        }

        private Vector3 GetRandomPositionInsideArea()
        {
            float cameraFarClipPlane = camera.farClipPlane;
            return camera.ScreenToWorldPoint(
                new Vector3(
                    Random.Range(0, Screen.width),
                    Random.Range(0, Screen.height),
                    Random.Range(cameraFarClipPlane - zOffsetFromCameraFarClip, cameraFarClipPlane)
                ));
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
            if (IsAnySphereCreated() == false) return;

            var sphere = spheresPool[nextSphereIndex];
            sphere.ShowSphere();
            sphere.GravityField.RegisterSphere(sphere);

            OnVisibleSpheresCountChange?.Invoke(++nextSphereIndex);

            if (AreAllSpheresVisible() == false) return;

            StopSpawning();
            SendInvertGravityEvent();
        }

        private void SendInvertGravityEvent() => OnReverseGravityStarted.Invoke();

        private bool IsAnySphereCreated() => spheresPool[0];

        private bool AreAllSpheresVisible() => nextSphereIndex == spheresPool.Length;

        private void StopSpawning() => enabled = false;

        private void ResetSphereShowDelay() => sphereShowDelay = 0f;

        #endregion private methods
    }
}