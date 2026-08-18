using System.Collections;
using UnityEngine;

namespace CodeBase.Car
{
    public sealed class LooseWheel : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField, Min(0f)] private float debrisLayerDelay;

        public void Launch(Transform visual, Rigidbody source, int debrisLayer)
        {
            visual.SetParent(transform, true);

            body.mass = source.mass;
            body.linearVelocity = source.linearVelocity;
            body.angularVelocity = source.angularVelocity;

            StartCoroutine(SwitchLayer(debrisLayer));
        }

        private IEnumerator SwitchLayer(int debrisLayer)
        {
            yield return new WaitForSeconds(debrisLayerDelay);
            SetLayerRecursively(transform, debrisLayer);
        }

        private static void SetLayerRecursively(Transform target, int layer)
        {
            target.gameObject.layer = layer;
            foreach (Transform child in target)
                SetLayerRecursively(child, layer);
        }
    }
}
