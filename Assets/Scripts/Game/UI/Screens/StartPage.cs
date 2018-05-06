using System.Collections;
using Framework.Extensions;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using UnityEngine;
using Event = Framework.Events.Event;

namespace Game.UI.Screens
{
    public class StartPage : Page<PageModel>
    {
        private Coroutine _overlayTransitionCoroutine;

        [SerializeField] private float _overlayTransitionSpeed;
        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private RectTransform _title;
        [SerializeField] private RectTransform _tapMessage;
        [SerializeField] private RectTransform _markerMessage;
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

            _markerMessage.gameObject.SetActive(false);
            _tapMessage.gameObject.SetActive(false);
            this.WaitForSeconds(1f, () => _tapMessage.gameObject.SetActive(true));
        }

        private void OnTrackingFound()
        {
            _markerMessage.gameObject.SetActive(false);
        }

        protected override IEnumerator InTransition()
        {
            _overlayTransitionCoroutine = StartCoroutine(ShowOverlay());
            yield return _overlayTransitionCoroutine;
        }

        private IEnumerator ShowOverlay()
        {
            _overlay.gameObject.SetActive(true);
            _overlay.alpha = 1f;

            while (_overlay.alpha > 0f)
            {
                _overlay.alpha -= _overlayTransitionSpeed * 2f * Time.deltaTime;
                yield return null;
            }

            _overlay.alpha = 0f;
            _overlay.gameObject.SetActive(false);
            _overlayTransitionCoroutine = null;
        }

        public override void OnExit()
        {
            ARMarkerEventTracker.TrackingFound -= OnTrackingFound;

            _overlay.gameObject.SetActive(false);
            _title.gameObject.SetActive(false);
            _tapMessage.gameObject.SetActive(false);
            _markerMessage.gameObject.SetActive(false);

            this.SafeStopCoroutine(_overlayTransitionCoroutine);
            base.OnExit();
        }
    }
}