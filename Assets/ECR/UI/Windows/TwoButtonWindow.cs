using RSG;
using UnityEngine;
using UnityEngine.UI;

namespace ECR.UI.Windows
{
    public class TwoButtonWindow : OneButtonWindow
    {
        [SerializeField] private Button cancelButton;

        public override Promise<bool> InitAndShow<T>(T data, string titleText = "")
        {
            cancelButton.onClick.AddListener(Deny);
            cancelButton.onClick.AddListener(PlaySoundEffect);
            cancelButton.onClick.AddListener(PlayHapticEffect);

            return base.InitAndShow(data, titleText);
        }

        protected override void Close()
        {
            cancelButton.onClick.RemoveAllListeners();
            base.Close();
        }

        private void Deny()
        {
            UserAccepted = false;
            Close();
        }
    }
}