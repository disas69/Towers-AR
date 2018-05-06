using System;
using Framework.Extensions;
using Game.Input;
using UnityEngine;

namespace Game.Gameplay.RingStructure
{
    public class RingInputHandler : MonoBehaviour, IInteractable
    {
        private Camera _mainCamera;
        private int _inputReceiver;

        public event Action<Vector3> PointerDown;
        public event Action<Vector3> Drag;
        public event Action PointerUp;

        public bool IsDragging { get; private set; }

        private void Start()
        {
            _mainCamera = Camera.main;
            _inputReceiver = LayerMask.GetMask("InputReceiver");
        }

        public void OnPointerDown(Vector2 position)
        {
            var ray = _mainCamera.ScreenPointToRay(position);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, _inputReceiver))
            {
                var point = hit.point;

                Debug.DrawLine(ray.origin, point);
                Debug.Log(string.Format("{0} Pointer down: {1}", transform.gameObject.name, point));

                IsDragging = true;
                PointerDown.SafeInvoke(point);
            }
        }

        public void OnDrag(Vector2 position)
        {
            var ray = _mainCamera.ScreenPointToRay(position);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, _inputReceiver))
            {
                var point = hit.point;

                Debug.DrawLine(ray.origin, point);
                Debug.Log(string.Format("{0} Drag: {1}", transform.gameObject.name, point));

                Drag.SafeInvoke(point);
            }
        }

        public void OnPointerUp(Vector2 position)
        {
            Debug.Log(string.Format("{0} Pointer up", transform.gameObject.name));

            IsDragging = false;
            PointerUp.SafeInvoke();
        }
    }
}