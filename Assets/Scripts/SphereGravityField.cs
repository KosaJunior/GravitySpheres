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

        [SerializeField] private LayerMask      sphereGravityFieldMask;
        [SerializeField] private Rigidbody      rigidbody;

        private bool isGravityInverted = false;

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

        public void InvertGravity() => isGravityInverted =  true;


        #endregion public methods

        #region Private methods

        private void OnCollisionEnter(Collision other)
        {
            if (isGravityInverted)
                return;

            if (IsCollideWithGravityField(other.gameObject.layer))
                PrepareToCombine(other);
        }

        private void PrepareToCombine(Collision other)
        {
            rigidbody.detectCollisions = false;
            DisableGravity();
            OnSphereCollision.Invoke(other);
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
                    if (isGravityInverted)
                        Repel(spheres[i]);
                    else
                        Attract(spheres[i]);
            }
        }

        private void Repel(GravitySphere sphereToAttract)
        {
            var     rigidbodyToRepel = sphereToAttract.Rigidbody;
            Vector3 direction        = rigidbody.position - rigidbodyToRepel.position;
            direction.Normalize();
            rigidbody.AddForce(direction * rigidbody.mass);
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