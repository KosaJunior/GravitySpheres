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
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private SphereGravityField gravityField;

        private List<GravitySphere> spheresInside = new List<GravitySphere>();

        private float   defaultMass;
        private Vector3 defaultScale;

        #endregion variables

        #region Properties
        public Rigidbody Rigidbody => rigidbody;
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
            CheckIsTimeToBreakUp();
        }

        private void CheckIsTimeToBreakUp()
        {
            bool isLimitReached = spheresInside.Count >= settings.SpheresToBreakup;
            if (isLimitReached == false) return;

            ReleaseSpheres();
        }

        private void ReleaseSpheres()
        {
            for (int i = 0; i < spheresInside.Count; i++)
                StartCoroutine(spheresInside[i].ReleaseSphere());
        }

        private IEnumerator ReleaseSphere()
        {


            yield return new WaitForSeconds(settings.TimeWithCollisionDisabledAfterBreakup);
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