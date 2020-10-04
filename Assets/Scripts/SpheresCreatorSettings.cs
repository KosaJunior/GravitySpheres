using UnityEngine;

namespace GravitySpheres.Scripts
{
    [CreateAssetMenu(fileName = "new SpheresCreatorSettings", menuName = "Settings/Create new SpheresCreatorSettings", order = 0)]
    public class SpheresCreatorSettings : ScriptableObject
    {
        [SerializeField] private float createDelay  = 0.25f;
        [SerializeField] private uint  spheresLimit = 250;

        public float CreateDelay  => createDelay;
        public uint  SpheresLimit => spheresLimit;
    }
}