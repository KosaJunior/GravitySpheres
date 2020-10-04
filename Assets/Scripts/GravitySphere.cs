using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphere : MonoBehaviour
    {
        #region Variables

        [Header("[ References ]")]
        [SerializeField] private GravitySphereMovement movement;
        // [SerializeField] private GravitySphereCollider collider;

        #endregion variables

        #region Constructor & inits

        public void Initialize(float areaWidth, float areaHeight)
        {
            movement.Initialize(areaWidth, areaHeight);
            SubscribeToColliderEvents();
        }

        private void SubscribeToColliderEvents()
        {
            // edgeDetector.OnCollisionWithScreenEdge += StartMovingBackward;
        }

        private void OnDestroy()
        {
            DisposeColliderEvents();
        }

        private void DisposeColliderEvents()
        {
            // edgeDetector.OnCollisionWithScreenEdge -= StartMovingBackward;
        }

        #endregion constructor & inits
    }
}