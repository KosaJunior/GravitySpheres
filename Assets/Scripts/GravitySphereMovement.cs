using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace GravitySpheres.Scripts
{
    public class GravitySphereMovement : MonoBehaviour
    {
        private enum MovingDirection
        {
            ToEnd, ToStart, Disabled
        }

        #region Variables

        [Header("[ Settings ]")]
        [SerializeField] private float movingSpeed = 2f;

        [Header("[ Components References ]")]
        [SerializeField] private Rigidbody rigidbody;

        private float areaWidth;
        private float areaHeight;

        private float           lastSqrMag;
        private MovingDirection currentDirection;

        private Vector3 startPosition;
        private Vector3 endPosition;
        private Vector3 desiredVelocity;

        #endregion variables

        #region Constructor & inits

        public void Initialize(float areaWidth, float areaHeight)
        {
            this.areaWidth  = areaWidth;
            this.areaHeight = areaHeight;

            lastSqrMag = Mathf.Infinity;

            SetRandomStartPosition();
            SetRandomEndPosition();
            SwitchMovingState(MovingDirection.ToEnd);
        }

        private void SetRandomStartPosition()
        {
            startPosition      = GetRandomPosition();
            transform.position = startPosition;
        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(
                Random.Range(-areaWidth, areaWidth),
                Random.Range(-areaHeight, areaHeight),
                0f
            );
        }

        private void SetRandomEndPosition()
        {
            endPosition = GetRandomPosition();
        }

        private void SwitchMovingState(MovingDirection movingDirection)
        {
            switch (movingDirection)
            {
                case MovingDirection.ToEnd:
                    SetDesiredVelocity(endPosition);
                    break;
                case MovingDirection.ToStart:
                    SetDesiredVelocity(startPosition);
                    break;
                case MovingDirection.Disabled:
                    SetDesiredVelocity(Vector3.zero);
                    break;
            }

            currentDirection = movingDirection;
        }

        private void SetDesiredVelocity(Vector3 velocityToSet)
        {
            rigidbody.velocity = desiredVelocity = (velocityToSet - transform.position).normalized * movingSpeed;
        }

        #endregion constructor & inits

        #region Private methods

        private void Update()
        {
            CheckIsTargetPositionReached();
        }

        private void CheckIsTargetPositionReached()
        {
            float sqrMag          = (desiredVelocity - transform.position).sqrMagnitude;
            bool  isTargetReached = sqrMag > lastSqrMag;
            if (isTargetReached)
                SwitchDesiredVelocity();

            lastSqrMag = sqrMag;
        }

        private void SwitchDesiredVelocity()
        {
            bool isEndReached = currentDirection == MovingDirection.ToEnd;
            SwitchMovingState(isEndReached ? MovingDirection.ToStart : MovingDirection.ToEnd);
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            rigidbody.velocity = desiredVelocity;
        }

#if UNITY_EDITOR
        public bool drawGizmos = false;
        private void OnDrawGizmos()
        {
            if (drawGizmos)
                Gizmos.DrawLine(transform.position, desiredVelocity);
        }
#endif

        #endregion private methods
    }
}