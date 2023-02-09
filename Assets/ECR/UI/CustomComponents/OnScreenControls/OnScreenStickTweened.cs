using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using static UnityEngine.RectTransformUtility;

namespace ECR.UI.CustomComponents.OnScreenControls
{
    [AddComponentMenu("Input/On-Screen Stick Tweened")]
    public class OnScreenStickTweened : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [InputControl(layout = "Vector2")]
        [SerializeField] private string m_ControlPath;
        [SerializeField] private float m_MovementRange = 50;

        private Vector3 _startPos;
        private Vector2 _pointerDownPos;

        private void Start()
        {
            // uncomment for set real gamepad to receive haptic fx (using Lofelt.NiceVibrations)
            // GamepadRumbler.SetCurrentGamepad(0);
            InputSystem.ResumeHaptics();
            
            _startPos = ((RectTransform) transform).anchoredPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponentInParent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out _pointerDownPos
            );
        }

        public void OnDrag(PointerEventData eventData)
        {
            ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponentInParent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out var position
            );
            var delta = Vector2.ClampMagnitude(position - _pointerDownPos, m_MovementRange);
            
            ((RectTransform) transform).anchoredPosition = _startPos + (Vector3) delta;
            SendValueToControl(delta / m_MovementRange);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ((RectTransform) transform).anchoredPosition = _startPos;
            SendValueToControl(Vector2.zero);
        }

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }
    }
}