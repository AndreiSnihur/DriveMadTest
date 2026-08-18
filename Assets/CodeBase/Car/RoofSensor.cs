using System;
using UnityEngine;

namespace CodeBase.Car
{
    [RequireComponent(typeof(Collider))]
    public sealed class RoofSensor : MonoBehaviour
    {
        [SerializeField] private LayerMask groundMask;

        public event Action Hit;

        private void OnTriggerEnter(Collider other)
        {
            if ((groundMask.value & (1 << other.gameObject.layer)) != 0)
                Hit?.Invoke();
        }
    }
}
