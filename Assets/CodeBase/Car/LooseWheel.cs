using System.Collections;
using UnityEngine;

namespace CodeBase.Car
{
    public sealed class LooseWheel : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField, Min(0f)] private float debrisLayerDelay;

        public void Launch(Transform visual, Rigidbody source)
        {
            visual.SetParent(transform, true);

            body.mass = source.mass;
            body.linearVelocity = source.linearVelocity;
            body.angularVelocity = source.angularVelocity;

            StartCoroutine(SwitchLayer(source.gameObject.layer, gameObject.layer));
        }

        // Spawns on the car's layer so it doesn't get kicked out of the chassis,
        // then returns to its own (Debris) layer once clear of the body.
        private IEnumerator SwitchLayer(int spawnLayer, int debrisLayer)
        {
            SetLayerRecursively(transform, spawnLayer);
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
