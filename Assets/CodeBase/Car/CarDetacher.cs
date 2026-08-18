using UnityEngine;

namespace CodeBase.Car
{
    public sealed class CarDetacher : MonoBehaviour
    {
        [SerializeField] private CarMover mover;
        [SerializeField] private Wheel frontWheel;
        [SerializeField] private Wheel rearWheel;
        [SerializeField] private LayerMask debrisLayer;

        public bool IsDetached { get; private set; }

        public void Detach()
        {
            if (IsDetached)
                return;

            IsDetached = true;
            mover.enabled = false;

            var layer = FirstLayerIndex(debrisLayer);
            frontWheel.Detach(layer);
            rearWheel.Detach(layer);
        }

        private static int FirstLayerIndex(LayerMask mask)
        {
            for (var i = 0; i < 32; i++)
                if ((mask.value & (1 << i)) != 0)
                    return i;
            return 0;
        }
    }
}
