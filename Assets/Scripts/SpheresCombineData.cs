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

        public SpheresCombineData(GravitySphere sphere, float combineTime)
        {
            spheresToCombine = new List<GravitySphere> {sphere};
            this.combineTime = combineTime;
            UpdateProperties();
        }

        private void UpdateProperties()
        {
            targetPosition = Vector3.zero;
            targetScale    = Vector3.zero;

            for (int i = 0; i < spheresToCombine.Count; i++)
            {
                targetPosition += spheresToCombine[i].transform.position;
                targetScale    += spheresToCombine[i].transform.localScale;
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
            rigidbodyToMove.DOMove(targetPosition, combineTime);
        }

        private void ChangeScale(Transform transformToScale)
        {
            transformToScale.DOScale(targetScale, combineTime);
        }
    }
}