using System;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphere : MonoBehaviour
    {
        #region Variables

        [Header("[ References ]")]
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private SphereGravityField gravityField;

        #endregion variables

        #region Properties
        public Rigidbody Rigidbody => rigidbody;
        public SphereGravityField GravityField => gravityField;

        #endregion properties

        #region Constructor & inits

        private void OnEnable()
        {
            gravityField.RegisterSphere(this);
        }

        private void OnDisable()
        {
            gravityField.UnregisterSphere(this);
        }

        public void Initialize() { }

        #endregion constructor & inits

        #region Public methods

        public void ShowSphere() => gameObject.SetActive(true);

        #endregion public methods
    }
}