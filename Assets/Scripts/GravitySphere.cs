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

        #region Public methods

        public void ShowSphere()
        {
            gravityField.RegisterSphere(this);
            gameObject.SetActive(true);
        }

        #endregion public methods
    }
}