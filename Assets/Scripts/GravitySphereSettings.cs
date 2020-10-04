using UnityEngine;

namespace GravitySpheres.Scripts
{
    [CreateAssetMenu(fileName = "new GravitySphereSettings", menuName = "Settings/Create new GravitySphereSettings", order = 0)]
    public class GravitySphereSettings : ScriptableObject
    {
        [SerializeField] private uint  spheresToBreakup = 50;

        [SerializeField] private float timeWithCollisionDisabledAfterBreakup = 0.5f;

        public uint SpheresToBreakup => spheresToBreakup;

        public float TimeWithCollisionDisabledAfterBreakup => timeWithCollisionDisabledAfterBreakup;
    }
}