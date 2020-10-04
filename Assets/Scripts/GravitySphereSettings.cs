using UnityEngine;

namespace GravitySpheres.Scripts
{
    [CreateAssetMenu(fileName = "new GravitySphereSettings", menuName = "Settings/Create new GravitySphereSettings", order = 0)]
    public class GravitySphereSettings : ScriptableObject
    {
        [SerializeField] private uint  massModifierToBreakup = 50;
        [SerializeField] private float maxSpeedAfterBreakup  = 2000f;
        [SerializeField] private float minSpeedAfterBreakup  = 500f;

        [SerializeField] private float timeWithCollisionDisabledAfterBreakup = 0.5f;

        public uint  MassModifierToBreakup   => massModifierToBreakup;
        public float RandomSpeedAfterBreakup => Random.Range(minSpeedAfterBreakup, maxSpeedAfterBreakup);

        public float TimeWithCollisionDisabledAfterBreakup => timeWithCollisionDisabledAfterBreakup;
    }
}