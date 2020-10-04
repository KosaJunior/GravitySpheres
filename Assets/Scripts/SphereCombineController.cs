using System.Collections.Generic;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class SphereCombineController : MonoBehaviour
    {
        [Header("[ Settings ]")]
        [SerializeField] private float combineDelay = 0.5f; // Wait for all possible collision
        [SerializeField] private float combineTime = 4f;

        private float currentDelay = 0f;

        private Dictionary<int, SpheresCombineData> combineData = new Dictionary<int, SpheresCombineData>();

        public void SubscribeToCollisionEvent(GravitySphere gravitySphere)
        {
            gravitySphere.GravityField.OnSphereCollision += OnCollision;
        }

        private void OnCollision(Collision collision)
        {
            int collisionKey  = collision.GetHashCode();
            var gravitySphere = collision.gameObject.GetComponent<GravitySphere>();
            if (IsMergingRequestCreated(collisionKey))
                AddToExistingCombineData(collisionKey, gravitySphere);
            else
                CreateCombineData(collisionKey, gravitySphere);
        }

        private bool IsMergingRequestCreated(int key) => combineData.ContainsKey(key);

        private void CreateCombineData(int key, GravitySphere gravitySphere)
        {
            combineData.Add(key, new SpheresCombineData(gravitySphere, combineTime));
        }

        private void AddToExistingCombineData(int key, GravitySphere sphere)
        {
            combineData[key].AddSphereToCombine(sphere);
        }

        private void FixedUpdate()
        {
            CalculateDelayTime();
            if (IsTimeToCombine() == false)
                return;

            Combine();
            ResetDelayTime();
        }

        private void CalculateDelayTime() => currentDelay += Time.deltaTime;

        private bool IsTimeToCombine() => currentDelay >= combineDelay;

        private void Combine()
        {
            foreach (var combine in combineData.Values)
                combine.CombineSpheres();

            combineData.Clear();
        }

        private void ResetDelayTime() => currentDelay = 0f;
    }
}