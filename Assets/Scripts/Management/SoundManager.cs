using System;
using UnityEngine;
using UnityEngine.Audio;

namespace BEST.BomberMan.Management
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] 
        private AudioMixer mainVolume;
        
        [Header("Audio sources")]
        [SerializeField]
        private AudioSource sfxAudioSource;

        [SerializeField]
        private AudioSource bgmAudioSource;
        
        [Header("Audio clips")]
        [SerializeField]
        private AudioClip explosionSound;
        
        [SerializeField]
        private AudioClip bgmTrack;

        [Header("Audio control")]
        [SerializeField]
        //[Range(-80.0f, 0)] //This is to demo why this is the wrong way to do things
        [Range(0.0001f, 1f)]
        private float volume;
        
        private void Update()
        {
            //mainVolume.SetFloat("MasterVolume", volume); //This is to demo why this is the wrong way to do things
            mainVolume.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }

        public void PlayExplosionSound()
        {
            sfxAudioSource.clip = explosionSound;
            sfxAudioSource.Play();
        }

        public void PlayBGMTrack()
        {
            bgmAudioSource.clip = bgmTrack;
            bgmAudioSource.Play();
        }
    }
}
