using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ECR.UI.CustomComponents
{
    public class ButtonForPromised : Button
    {
        private PointerEventData _eventData;

        public override void OnPointerUp(PointerEventData eventData) => 
            _eventData = eventData;

        public void OnPromisedResolve()
        {
            _eventData.button = PointerEventData.InputButton.Left;
            base.OnPointerUp(_eventData);
        }
    }
}