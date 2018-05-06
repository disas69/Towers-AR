using UnityEngine;

namespace Game.Gameplay.RingStructure
{
    [RequireComponent(typeof(AudioSource))]
    public class RingView : MonoBehaviour
    {
        private AudioSource _audioSource;
        private bool _isInteractive;

        [SerializeField] private float _rotarionSpeed = 5f;
        [SerializeField] private TrailRenderer _trailRenderer;
        [Header("SFX")] [SerializeField] private AudioClip _interaction1AudioClip;
        [SerializeField] private AudioClip _interaction2AudioClip;
        [SerializeField] private AudioClip _blockedAudioClip;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void SetInteractive(bool value)
        {
            _isInteractive = value;
            _trailRenderer.enabled = value;
            PlayAudioClip(value ? _interaction1AudioClip : _interaction2AudioClip);
        }

        public void SetBlocked()
        {
            PlayAudioClip(_blockedAudioClip);
        }

        private void Update()
        {
            if (_isInteractive)
            {
                transform.Rotate(Vector3.up, _rotarionSpeed);
            }
        }

        private void PlayAudioClip(AudioClip clip)
        {
            if (gameObject.activeSelf)
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }

                _audioSource.PlayOneShot(clip);
            }
        }
    }
}