using Framework.Extensions;
using Framework.Localization;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using Framework.Variables;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Event = Framework.Events.Event;

namespace Game.UI.Screens
{
    public class ReplayPage : Page<PageModel>
    {
        [SerializeField] private RectTransform _title;
        [SerializeField] private RectTransform _tapMessage;
        [SerializeField] private RectTransform _markerMessage;
        [SerializeField] private Text _movesCountMessage;
        [SerializeField] private IntVariable _movesCountVariable;
        [SerializeField] private Event _onTapEvent;

        [UsedImplicitly]
        public void FireEvent()
        {
            if (ARMarkerEventTracker.MarkerFound)
            {
                _onTapEvent.Fire();
            }
            else
            {
                _markerMessage.gameObject.SetActive(true);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            ARMarkerEventTracker.TrackingFound += OnTrackingFound;

            _movesCountMessage.text = string.Format(LocalizationManager.GetString("MovesCountMessage"), _movesCountVariable.Value);
            _title.gameObject.SetActive(false);
            _tapMessage.gameObject.SetActive(false);
            _markerMessage.gameObject.SetActive(false);

            this.WaitForSeconds(1f, () =>
            {
                _title.gameObject.SetActive(true);
                _tapMessage.gameObject.SetActive(true);
            });
        }

        private void OnTrackingFound()
        {
            _markerMessage.gameObject.SetActive(false);
        }

        public override void OnExit()
        {
            ARMarkerEventTracker.TrackingFound -= OnTrackingFound;

            _title.gameObject.SetActive(false);
            _tapMessage.gameObject.SetActive(false);

            base.OnExit();
        }
    }
}