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
            UpdateProperties();
        }

        public void CombineSpheres()
        {
            for (int i = 0; i < spheresToCombine.Count; i++)
            {
                Move(spheresToCombine[i].Rigidbody);
                ChangeScale(spheresToCombine[i].transform);
            }
        }

        private void Move(Rigidbody rigidbodyToMove)
        {
            rigidbodyToMove.DOMove(targetPosition, combineTime)
                           .OnComplete(ResetAllSpheresWithoutFirst);
        }

        /// <summary>
        /// We don't need other spheres at the moment
        /// </summary>
        private void ResetAllSpheresWithoutFirst()
        {
            for (int i = 1; i < spheresToCombine.Count; i++)
            {
                spheresToCombine[i].DisableSphere();
                spheresToCombine[0].AddSphereInside(spheresToCombine[i]);
            }

            spheresToCombine[0].Rigidbody.mass = combinedMass;
            spheresToCombine[0].GravityField.EnableGravity();
        }

        private void ChangeScale(Transform transformToScale)
        {
            transformToScale.DOScale(targetScale, combineTime);
        }
    }
}