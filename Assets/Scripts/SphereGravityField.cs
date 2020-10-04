using System.Collections.Generic;
using GravitySpheres.Scripts.Utilities;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class SphereGravityField : MonoBehaviour
    {
        #region Variables

        private const float G = 667.4f;

        private static List<GravitySphere> spheres = new List<GravitySphere>();

        public event System.Action<Collision> OnSphereCollision;

        [SerializeField] private LayerMask sphereGravityFieldMask;
        [SerializeField] private Rigidbody rigidbody;

        #endregion variables

        #region Public methods

        public void EnableGravity()
        {
            rigidbody.detectCollisions = true;
            enabled                    = true;
        }

        public void RegisterSphere(GravitySphere sphere)
        {
            spheres.Add(sphere);
        }

        public void UnregisterSphere(GravitySphere sphere)
        {
            spheres.Remove(sphere);
        }

        #endregion public methods

        #region Private methods

        private void OnCollisionEnter(Collision other)
        {
            if (IsCollideWithGravityField(other.gameObject.layer))
                OnCollision(other);
        }

        private void OnCollision(Collision collision)
        {
            rigidbody.detectCollisions = false;
            DisableGravity();
            OnSphereCollision.Invoke(collision);
        }

        private void DisableGravity() => enabled = false;

        private bool IsCollideWithGravityField(LayerMask otherMask)
        {
            return sphereGravityFieldMask.Contains(otherMask);
        }

        private void FixedUpdate()
        {
            UpdateGravity();
        }

        private void UpdateGravity()
        {
            for (int i = 0; i < spheres.Count; i++)
            {
                if (spheres[i].GravityField != this)
                    Attract(spheres[i]);
            }
        }

        private void Attract(GravitySphere sphereToAttract)
        {
            var     rigidbodyToAttract = sphereToAttract.Rigidbody;
            Vector3 direction          = rigidbody.position - rigidbodyToAttract.position;
            float   distance           = direction.magnitude;

            if (distance == 0) return;

            float   forceMagnitude = G * (rigidbody.mass * rigidbodyToAttract.mass) / Mathf.Pow(distance, 2);
            Vector3 force          = direction.normalized * forceMagnitude;

            rigidbodyToAttract.AddForce(force);
        }

        #endregion private methods
    }
}