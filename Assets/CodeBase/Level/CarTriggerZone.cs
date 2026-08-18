using System;
using CodeBase.Car;
using UnityEngine;

namespace CodeBase.Level
{
    [RequireComponent(typeof(Collider))]
    public sealed class CarTriggerZone : MonoBehaviour
    {
        public event Action CarEntered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null)
                return;

            if (other.attachedRigidbody.GetComponentInParent<CarMover>() != null)
                CarEntered?.Invoke();
        }
    }
}
