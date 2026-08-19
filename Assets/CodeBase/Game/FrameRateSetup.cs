using UnityEngine;

namespace CodeBase.Game
{
    public static class FrameRateSetup
    {
        private const int FallbackFrameRate = 60;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Apply()
        {
            if (!Application.isMobilePlatform)
                return;

            var refreshRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
            Application.targetFrameRate = refreshRate > 0 ? refreshRate : FallbackFrameRate;
        }
    }
}
