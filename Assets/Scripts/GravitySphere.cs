using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphere : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float movingSpeed = 2f;

        private float xPositionLimit;
        private float yPositionLimit;

        private Vector3 targetDirection;

        #endregion variables

        #region Constructor & inits

        public void Initialize(float xPositionLimit, float yPositionLimit)
        {
            SetPositionLimits(xPositionLimit, yPositionLimit);
            SetRandomPosition();
            SetRandomMoveDirection();
        }

        private void SetPositionLimits(float xPositionLimit, float yPositionLimit)
        {
            this.xPositionLimit = xPositionLimit;
            this.yPositionLimit = yPositionLimit;
        }

        private void SetRandomPosition()
        {
            transform.position = GetRandomPosition();
        }

        #endregion constructor & inits

        #region Private methods

        private void SetRandomMoveDirection()
        {
            var randomPosition = GetRandomPosition();
            targetDirection = (randomPosition - transform.position).normalized;
        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(
                Random.Range(-xPositionLimit, xPositionLimit),
                Random.Range(-yPositionLimit, yPositionLimit),
                0f
            );
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += targetDirection * (movingSpeed * Time.deltaTime);
        }

        #endregion private methods
    }
}