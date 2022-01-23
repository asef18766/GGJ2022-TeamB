using System;
using System.Collections.Generic;
using UnityEngine;

namespace asef18766.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioSources;
        private readonly Dictionary<string, int> _n2I = new Dictionary<string, int>();
        private readonly List<AudioSource> _audioControllers = new List<AudioSource>();
        public static AudioManager Instance = null;
        private void Start()
        {
            for (var i = 0; i != audioSources.Count; ++i)
            {
                _n2I.Add(audioSources[i].name, i);
                _audioControllers.Add(gameObject.AddComponent<AudioSource>());
                _audioControllers[i].clip = audioSources[i];
                _audioControllers[i].Stop();
            }

            Instance = this;
        }

        public void SetSoundLoop(string audioName)
        {
            if (!_n2I.ContainsKey(audioName))
                throw new IndexOutOfRangeException($"can not access {audioName} in audio source");
            _audioControllers[_n2I[audioName]].loop = true;
        }
        
        public void PlaySound(string audioName)
        {
            if (!_n2I.ContainsKey(audioName))
                throw new IndexOutOfRangeException($"can not access {audioName} in audio source");
            _audioControllers[_n2I[audioName]].Play();
        }
        
        public void StopSound(string audioName)
        {
            if (!_n2I.ContainsKey(audioName))
                throw new IndexOutOfRangeException($"can not access {audioName} in audio source");
            _audioControllers[_n2I[audioName]].Stop();
        }
    }
}