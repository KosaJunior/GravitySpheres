using System.Collections.Generic;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphere : MonoBehaviour
    {
        #region Variables

        [Header("[ References ]")]
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private SphereGravityField gravityField;

        [SerializeField] private List<GravitySphere> spheresInside = new List<GravitySphere>();

        private uint spheresInsideLimit;

        private float   defaultMass;
        private Vector3 defaultScale;

        #endregion variables

        #region Properties
        public Rigidbody Rigidbody => rigidbody;
        public SphereGravityField GravityField => gravityField;

        #endregion properties

        #region Constructor & inits

        public void Initialize(uint spheresInsideLimit)
        {
            this.spheresInsideLimit = spheresInsideLimit;
            CacheStartParameters();
        }

        private void CacheStartParameters()
        {
            defaultMass  = rigidbody.mass;
            defaultScale = transform.localScale;
        }

        #endregion constructor & inits

        #region Public methods

        public void ShowSphere()
        {
            gravityField.RegisterSphere(this);
            gameObject.SetActive(true);
        }

        public void DisableSphere()
        {
            gravityField.UnregisterSphere(this);
            gameObject.SetActive(false);
            ResetSphere();
        }

        public void AddSphereInside(GravitySphere sphereInside)
        {
            if (spheresInside.Contains(sphereInside)) return;

            spheresInside.Add(sphereInside);
        }

        #endregion public methods

        #region Private methods

        private void ResetSphere()
        {
            rigidbody.mass     = defaultMass;
            transform.position = defaultScale;
        }

        #endregion private methods
    }
}