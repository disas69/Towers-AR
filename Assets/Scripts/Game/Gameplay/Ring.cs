using Game.Gameplay.RingStructure;
using Game.Gameplay.RingStructure.Configuration;
using UnityEngine;
using Event = Framework.Events.Event;

namespace Game.Gameplay
{
    [RequireComponent(typeof(RingInputHandler), typeof(RingContactHandler))]
    public class Ring : MonoBehaviour
    {
        private RingInputHandler _inputHandler;
        private RingContactHandler _contactHandler;
        private RingView _ringView;

        private bool _isAnchoredToTower;
        private bool _isTopRing;
        private Vector3 _anchoredPosition;
        private Vector3 _dragPosition;
        private Vector3 _velocity;
        private Tower _currentTower;
        private Tower _nextTower;

        [SerializeField] private int _sizeIndex;
        [SerializeField] private RingSettings _ringSettings;
        [SerializeField] private Event _onWrongMoveEvent;

        public int SizeIndex
        {
            get { return _sizeIndex; }
        }

        private void Start()
        {
            _inputHandler = GetComponent<RingInputHandler>();
            _inputHandler.PointerDown += OnPointerDown;
            _inputHandler.Drag += OnDrag;
            _inputHandler.PointerUp += OnPointerUp;

            _contactHandler = GetComponent<RingContactHandler>();
            _contactHandler.Entered += OnTowerEntered;
            _contactHandler.Exited += OnTowerExited;

            _ringView = GetComponent<RingView>();
            _ringView.SetInteractive(false);
            _isAnchoredToTower = true;
        }

        public void AttachTo(Tower tower, bool isCountable)
        {
            tower.Add(this, isCountable);
            _currentTower = tower;
            _anchoredPosition = tower.GetTopPosition();
        }

        private void Update()
        {
            UpdatePosition();
            UpdateView();
        }

        private void UpdatePosition()
        {
            Vector2 targetPosition;

            var anchoredPosition = _currentTower.Bottom.position + _anchoredPosition;

            if (_inputHandler.IsDragging && _isTopRing)
            {
                targetPosition = _dragPosition;

                if (_isAnchoredToTower)
                {
                    if (_nextTower != null)
                    {
                        anchoredPosition = _nextTower.Bottom.position + _nextTower.GetTopPosition();
                    }

                    targetPosition.x = anchoredPosition.x;

                    if (targetPosition.y < anchoredPosition.y)
                    {
                        targetPosition.y = anchoredPosition.y;
                    }
                }
                else
                {
                    var anchorPosition = _currentTower.Anchor.transform.position;
                    if (_nextTower != null)
                    {
                        anchorPosition = _nextTower.Anchor.transform.position;
                    }

                    if (Vector2.Distance(transform.position, anchorPosition) < 0.1f)
                    {
                        _isAnchoredToTower = true;
                    }
                }
            }
            else
            {
                targetPosition = anchoredPosition;

                if (!_isAnchoredToTower)
                {
                    var anchorPosition = _currentTower.Anchor.transform.position;
                    if (Vector2.Distance(transform.position, anchorPosition) > 0.1f)
                    {
                        targetPosition = anchorPosition;
                    }
                    else
                    {
                        _isAnchoredToTower = true;
                    }
                }
            }

            var newPosition = Vector3.SmoothDamp(transform.position, targetPosition,
                ref _velocity, _ringSettings.MoveSmoothing);

            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }

        private void UpdateView()
        {

        }

        private void OnPointerDown(Vector3 position)
        {
            _isTopRing = _currentTower.GetTopRingSizeIndex() == SizeIndex;
            _dragPosition = position;

            if (_isTopRing)
            {
                _ringView.SetInteractive(true);
            }
            else
            {
                _ringView.SetBlocked();
            }
        }

        private void OnDrag(Vector3 position)
        {
            _dragPosition = position;
        }

        private void OnPointerUp()
        {
            if (_nextTower != null)
            {
                var topSizeIndex = _nextTower.GetTopRingSizeIndex();
                if (topSizeIndex < _sizeIndex)
                {
                    _currentTower.Remove();
                    AttachTo(_nextTower, true);
                }
                else
                {
                    _onWrongMoveEvent.Fire();
                }

                _nextTower = null;
            }

            if (_isTopRing)
            {
                _ringView.SetInteractive(false);
            }
        }

        private void OnTowerEntered(Tower tower)
        {
            if (tower != _currentTower)
            {
                _nextTower = tower;
            }
        }

        private void OnTowerExited(Tower tower)
        {
            if (tower == _currentTower)
            {
                _isAnchoredToTower = false;
            }
            else if (tower == _nextTower)
            {
                _nextTower = null;
                _isAnchoredToTower = false;
            }
        }

        private void OnDestroy()
        {
            _inputHandler.PointerDown -= OnPointerDown;
            _inputHandler.Drag -= OnDrag;
            _inputHandler.PointerUp -= OnPointerUp;

            _contactHandler.Entered -= OnTowerEntered;
            _contactHandler.Exited -= OnTowerExited;
        }
    }
}