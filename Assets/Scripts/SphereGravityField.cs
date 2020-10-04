using System.Collections.Generic;
using GravitySpheres.Scripts.Utilities;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class SphereGravityField : MonoBehaviour
    {
        private const float G = 667.4f;

        private static List<GravitySphere> spheres = new List<GravitySphere>();

        public event System.Action<GravitySphere> OnOtherSphereInField;

        [SerializeField] private LayerMask gravityFieldMask;
        [SerializeField] private Rigidbody rigidbody;

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (IsCollideWithGravityField(other.gameObject.layer))
        //         OnOtherSphereInField.Invoke(other.GetComponent<GravitySphere>());
        // }

        private bool IsCollideWithGravityField(LayerMask otherMask)
        {
            return gravityFieldMask.Contains(otherMask);
        }

        public void RegisterSphere(GravitySphere sphere)
        {
            spheres.Add(sphere);
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
    }
}