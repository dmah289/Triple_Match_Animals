using System;
using Controller;
using Framework;
using UnityEngine;

namespace Manager
{
    public enum SoundType
    {
        Collect,
        Click
    }
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] AudioClip[] audioClips;
        [SerializeField] AudioSource audioSource;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            audioSource = GetComponent<AudioSource>();
            audioClips = Resources.LoadAll<AudioClip>("Sounds/");
        }
#endif

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            MatchController.OnTilesPoppedSFX += PlaySFX;
            SelectionController.OnTileSelectedSFX += PlaySFX;
        }

        private void OnDisable()
        {
            MatchController.OnTilesPoppedSFX -= PlaySFX;
            SelectionController.OnTileSelectedSFX -= PlaySFX;
        }

        public void PlaySFX(SoundType soundType)
        {
            audioSource.PlayOneShot(audioClips[(int)soundType]);
        }
    }
}