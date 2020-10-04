using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GravitySpheres.Scripts
{
    public class SpheresCombineData
    {
        private List<GravitySphere> spheresToCombine;
        private float               combineTime;
        private Vector3             targetPosition;
        private Vector3             targetScale;

        private float combinedMass = 0;

        private GravitySphere largestSphere;

        public SpheresCombineData(GravitySphere sphere, float combineTime)
        {
            spheresToCombine = new List<GravitySphere>();
            this.combineTime = combineTime;
            AddSphereToCombine(sphere);
        }

        private void UpdateProperties()
        {
            targetPosition = Vector3.zero;
            targetScale    = Vector3.zero;
            combinedMass   = 0;

            for (int i = 0; i < spheresToCombine.Count; i++)
            {
                targetPosition += spheresToCombine[i].transform.position;
                targetScale    += spheresToCombine[i].transform.localScale;
                combinedMass   += spheresToCombine[i].Rigidbody.mass;
            }

            targetPosition /= spheresToCombine.Count;
        }

        public void AddSphereToCombine(GravitySphere gravitySphere)
        {
            spheresToCombine.Add(gravitySphere);
            largestSphere = GetLargestGravitySphere();
            UpdateProperties();
        }

        public void CombineSpheres()
        {
            for (int i = 0; i < spheresToCombine.Count; i++)
                Move(spheresToCombine[i].Rigidbody);

            ChangeScale(largestSphere.transform);
        }

        private void Move(Rigidbody rigidbodyToMove)
        {
            rigidbodyToMove.DOMove(targetPosition, combineTime);
        }

        private void ChangeScale(Transform transformToScale)
        {
            transformToScale.DOScale(targetScale, combineTime)
                            .OnComplete(ResetAllSpheresWithoutFirst);
        }

        /// <summary>
        /// We don't need other spheres at the moment
        /// </summary>
        private void ResetAllSpheresWithoutFirst()
        {
            for (int i = 0; i < spheresToCombine.Count; i++)
            {
                if (largestSphere.GetHashCode() == spheresToCombine[i].GetHashCode()) continue;

                spheresToCombine[i].DisableSphere();
                largestSphere.AddSphereInside(spheresToCombine[i]);
            }

            largestSphere.Rigidbody.mass = combinedMass;
            largestSphere.CheckIsTimeToDivide();
            largestSphere.GravityField.EnableGravity();
        }

        private GravitySphere GetLargestGravitySphere()
        {
            var sphere = spheresToCombine[0];
            for (int i = 1; i < spheresToCombine.Count; i++)
            {
                if (sphere.Rigidbody.mass < spheresToCombine[i].Rigidbody.mass)
                    sphere = spheresToCombine[i];
            }

            return sphere;
        }
    }
}