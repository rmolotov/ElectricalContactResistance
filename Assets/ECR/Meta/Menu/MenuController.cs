using UnityEngine;
using UnityEngine.UI;

namespace ECR.Meta.Menu
{
    public class MenuController : MonoBehaviour
    {
        public ToggleGroup stagesTogglesContainer;
        
        private string _selectedStage;

        public void SelectStage(string stageKey) => 
            _selectedStage = stageKey;
    }
}