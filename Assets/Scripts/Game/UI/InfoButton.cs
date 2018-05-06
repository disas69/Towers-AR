using UnityEngine;
using UnityEngine.UI;
using Event = Framework.Events.Event;

namespace Game.UI
{
    [RequireComponent(typeof(Button))]
    public class InfoButton : MonoBehaviour
    {
        private Button _button;

        [SerializeField] private Event _buttonClickEvent;

        private void Start()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(() => { _buttonClickEvent.Fire(); });
        }
    }
}