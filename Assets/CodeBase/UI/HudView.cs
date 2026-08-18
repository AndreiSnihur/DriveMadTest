using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public sealed class HudView : MonoBehaviour
    {
        [SerializeField] private Text statusLabel;
        [SerializeField] private string winText;
        [SerializeField] private string loseText;

        private void Awake() =>
            statusLabel.gameObject.SetActive(false);

        public void ShowWin() =>
            Show(winText);

        public void ShowLose() =>
            Show(loseText);

        private void Show(string text)
        {
            statusLabel.text = text;
            statusLabel.gameObject.SetActive(true);
        }
    }
}
