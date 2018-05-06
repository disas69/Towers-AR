using UnityEngine;

namespace Game.Input
{
    public interface IInteractable
    {
        void OnPointerDown(Vector2 position);
        void OnDrag(Vector2 position);
        void OnPointerUp(Vector2 position);
    }
}