﻿using UnityEngine;
using UnityEngine.UI;
using RSG;
using Sirenix.OdinInspector;

namespace ECR.UI.Windows
{
    public class OneButtonWindow : WindowBase
    {
        [Title("Buttons")]
        [SerializeField] private Button okButton;

        public override Promise<bool> InitAndShow<T>(T data, string titleText = "")
        {
            okButton.onClick.AddListener(Accept);
            okButton.onClick.AddListener(PlaySoundEffect);
            okButton.onClick.AddListener(PlayHapticEffect);

            return base.InitAndShow(data, titleText);
        }

        protected override void Close()
        {
            okButton.onClick.RemoveAllListeners();
            base.Close();
        }

        private void Accept()
        {
            UserAccepted = true;
            Close();
        }
    }
}