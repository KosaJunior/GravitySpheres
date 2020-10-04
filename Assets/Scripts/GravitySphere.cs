using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class GravitySphere : MonoBehaviour
    {
        #region Variables

        [Header("[ References ]")]
        [SerializeField] private GravitySphereSettings settings;

        [SerializeField] private Rigidbody          rigidbody;
        [SerializeField] private SphereGravityField gravityField;

        private List<GravitySphere> spheresInside = new List<GravitySphere>();

        private float   defaultMass;
        private Vector3 defaultScale;

        #endregion variables

        #region Properties

        public Rigidbody          Rigidbody    => rigidbody;
        public SphereGravityField GravityField => gravityField;

        #endregion properties

        #region Constructor & inits

        public void Initialize()
        {
            CacheStartParameters();
        }

        private void CacheStartParameters()
        {
            defaultMass  = rigidbody.mass;
            defaultScale = transform.lossyScale;
        }

        #endregion constructor & inits

        #region Public methods

        public void ShowSphere() => gameObject.SetActive(true);

        public void DisableSphere()
        {
            gravityField.UnregisterSphere(this);
            gameObject.SetActive(false);
            ResetSphere();
        }

        public void AddSphereInside(GravitySphere sphereInside)
        {
          if (spheresInside.Contains(sphereInside) == false)
              spheresInside.Add(sphereInside);
        }

        public void CheckIsTimeToDivide()
        {
            if (IsTimeToDivide() == false) return;

            ReleaseSpheres();
            ResetSphere();
        }

        private bool IsTimeToDivide() => rigidbody.mass >= settings.MassModifierToBreakup * defaultMass;

        private void ReleaseSpheres()
        {
            for (int i = 0; i < spheresInside.Count; i++)
                spheresInside[i].ReleaseSphere();

            spheresInside.Clear();
        }

        private void ReleaseSphere()
        {
            ShowSphere();
            StartCoroutine(ReleaseSphereCoroutine());
        }

        private IEnumerator ReleaseSphereCoroutine()
        {
            FireSphereToRandomDirection();
            yield return new WaitForSeconds(settings.TimeWithCollisionDisabledAfterBreakup);
            gravityField.EnableGravity();
            gravityField.RegisterSphere(this);
        }

        private void FireSphereToRandomDirection()
        {
            rigidbody.AddForce(Random.onUnitSphere * settings.RandomSpeedAfterBreakup, ForceMode.VelocityChange);
        }

        #endregion public methods

        #region Private methods

        private void ResetSphere()
        {
            rigidbody.mass       = defaultMass;
            transform.localScale = defaultScale;
        }

        #endregion private methods
    }
}