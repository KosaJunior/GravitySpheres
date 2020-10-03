using UnityEngine;

namespace GravitySpheres.Scripts.UI
{
    [RequireComponent(typeof(SphereCreator))]
    public class VisibleSpheresCounter : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI spheresCounter;

        private void Awake()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            GetComponent<SphereCreator>().OnVisibleSpheresCountChange += SetSpheresCount;
        }

        private void OnDestroy()
        {
            DisposeEvents();
        }

        private void DisposeEvents()
        {
            GetComponent<SphereCreator>().OnVisibleSpheresCountChange -= SetSpheresCount;
        }

        private void SetSpheresCount(int visibleSpheresCount)
        {
            spheresCounter.SetText(visibleSpheresCount.ToString());
        }
    }
}