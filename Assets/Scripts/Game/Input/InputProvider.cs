using System;
using Framework.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    public class InputProvider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private Camera _mainCamera;
        private int _interactableMask;
        private IInteractable _pickedObject;

        public event Action<PointerEventData> PointerDown;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData> PointerUp;

        private void Start()
        {
            _mainCamera = Camera.main;
            _interactableMask = LayerMask.GetMask("Interactable");

            UnityEngine.Input.multiTouchEnabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown.SafeInvoke(eventData);

            var ray = _mainCamera.ScreenPointToRay(eventData.position);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, _interactableMask))
            {
                _pickedObject = hit.transform.GetComponent<IInteractable>();

                if (_pickedObject != null)
                {
                    _pickedObject.OnPointerDown(eventData.position);
                }
            }
            else
            {
                _pickedObject = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag.SafeInvoke(eventData);

            if (_pickedObject != null)
            {
                _pickedObject.OnDrag(eventData.position);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp.SafeInvoke(eventData);

            if (_pickedObject != null)
            {
                _pickedObject.OnPointerUp(eventData.position);
            }

            _pickedObject = null;
        }
    }
}