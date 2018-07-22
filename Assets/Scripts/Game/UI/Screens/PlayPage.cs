using Framework.Localization;
using Framework.UI.Notifications;
using Framework.UI.Notifications.Model;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.UI.Screens
{
    public class PlayPage : Page<PageModel>
    {
        [SerializeField] private RectTransform _markerMessage;

        public override void OnEnter()
        {
            base.OnEnter();

            ARMarkerEventTracker.TrackingFound += OnTrackingFound;
            ARMarkerEventTracker.TrackingLost += OnTrackingLost;

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
            NotificationManager.Show(new TextNotification(LocalizationManager.GetString("WrongMove")), 2.5f);
        }

        public override void OnExit()
        {
            base.OnExit();

            ARMarkerEventTracker.TrackingFound -= OnTrackingFound;
            ARMarkerEventTracker.TrackingLost -= OnTrackingLost;
        }
    }
}