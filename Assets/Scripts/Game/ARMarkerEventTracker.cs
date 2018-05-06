using System;
using Framework.Extensions;
using UnityEngine;

namespace Game
{
    public class ARMarkerEventTracker : MonoBehaviour
    {
        [SerializeField] private DefaultTrackableEventHandler _trackableEventHandler;

        public static event Action TrackingFound;
        public static event Action TrackingLost;

        public static bool MarkerFound { get; private set; }

        private void Start()
        {
            _trackableEventHandler.TrackingFound += OnTrackingFound;
            _trackableEventHandler.TrackingLost += OnTrackingLost;
        }

        private static void OnTrackingFound()
        {
            MarkerFound = true;
            TrackingFound.SafeInvoke();
        }

        private static void OnTrackingLost()
        {
            MarkerFound = false;
            TrackingLost.SafeInvoke();
        }

        private void OnDestroy()
        {
            _trackableEventHandler.TrackingFound -= OnTrackingFound;
            _trackableEventHandler.TrackingLost -= OnTrackingLost;
        }
    }
}