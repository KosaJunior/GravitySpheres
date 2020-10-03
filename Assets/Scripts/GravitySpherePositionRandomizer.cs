using UnityEngine;

namespace GravitySpheres.Scripts
{
    /// <summary>
    /// Generates a random position inside camera frustum
    /// </summary>
    public class GravitySpherePositionRandomizer : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Camera camera;

        [SerializeField]private Vector2 bottomLeftCorner;
        [SerializeField]private Vector2 topRightCorner  ;

        #endregion variables

        #region Constructor & inits

        public void Initialize()
        {
            if (ValidateCameraOrthographicProjection() == false)
                return;

            CreateCameraCorners();
        }

        private bool ValidateCameraOrthographicProjection()
        {
            if (camera.orthographic) return true;

            Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders");
            return false;
        }

        private void CreateCameraCorners()
        {
            bottomLeftCorner = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
            topRightCorner   = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, camera.nearClipPlane));
        }

        #endregion constructor & inits

        #region Public methods

        public Vector2 GetRandomPosition()
        {
            float xPos = Random.Range(bottomLeftCorner.x, topRightCorner.x);
            float yPos = Random.Range(bottomLeftCorner.y, topRightCorner.y);

            return new Vector2(xPos, yPos);
        }

        #endregion public methods
    }
}