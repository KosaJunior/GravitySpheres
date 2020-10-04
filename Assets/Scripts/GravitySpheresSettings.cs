using UnityEngine;

namespace GravitySpheres.Scripts
{
    [CreateAssetMenu(fileName = "new GravitySpheresSettings", menuName = "Settings/Create new GravitySpheresSettings", order = 0)]
    public class GravitySpheresSettings : ScriptableObject
    {
        [SerializeField] private float createDelay  = 0.25f;
        [SerializeField] private uint  spheresLimit = 250;

        public float CreateDelay  => createDelay;
        public uint  SpheresLimit => spheresLimit;
    }
}