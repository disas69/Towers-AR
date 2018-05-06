using Framework.Extensions;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.UI.Screens
{
    public class PlayPage : Page<PageModel>
    {
        [SerializeField] private RectTransform _wrongMoveMessage;
        [SerializeField] private RectTransform _markerMessage;

        public override void OnEnter()
        {
            base.OnEnter();

            ARMarkerEventTracker.TrackingFound += OnTrackingFound;
            ARMarkerEventTracker.TrackingLost += OnTrackingLost;

            _wrongMoveMessage.gameObject.SetActive(false);

            if (ARMarkerEventTracker.MarkerFound)
            {
                _markerMessage.gameObject.SetActive(false);
            }
        }

        private void OnTrackingFound()
        {
            _markerMessage.gameObject.SetActive(false);
        }

        private void OnTrackingLost()
        {
            _markerMessage.gameObject.SetActive(true);
        }

        [UsedImplicitly]
        public void ShowWrongMoveMessage()
        {
            if (_wrongMoveMessage.gameObject.activeSelf)
            {
                return;
            }
            
            _wrongMoveMessage.gameObject.SetActive(true);
            this.WaitForSeconds(3f, () => _wrongMoveMessage.gameObject.SetActive(false));
        }

        public override void OnExit()
        {
            base.OnExit();

            ARMarkerEventTracker.TrackingFound -= OnTrackingFound;
            ARMarkerEventTracker.TrackingLost -= OnTrackingLost;
        }
    }
}