using UnityEngine;

namespace CodeBase.Car
{
    public sealed class CarDetacher : MonoBehaviour
    {
        [SerializeField] private CarMover mover;
        [SerializeField] private Wheel frontWheel;
        [SerializeField] private Wheel rearWheel;

        public bool IsDetached { get; private set; }

        public void Detach()
        {
            if (IsDetached)
                return;

            IsDetached = true;
            mover.enabled = false;

            frontWheel.Detach();
            rearWheel.Detach();
        }
    }
}
