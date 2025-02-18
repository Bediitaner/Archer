using System.Collections.Generic;
using ArcheroClone.Model;
using UnityEngine;

namespace ArcheroClone.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [System.Serializable]
        public class SoundEffect
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)]
            public float volume = 1f;
            [Range(0.1f, 3f)]
            public float pitch = 1f;
            public bool loop = false;
            
            [HideInInspector]
            public AudioSource source;
        }
        
        [Header("Audio Settings")]
        public float masterVolume = 1f;
        public float musicVolume = 0.5f;
        public float sfxVolume = 0.8f;
        public bool muteWhenPaused = true;
        
        [Header("Sound Effects")]
        public List<SoundEffect> soundEffects;
        
        [Header("Music")]
        public AudioClip mainMenuMusic;
        public AudioClip gameplayMusic;
        public AudioClip bossMusic;
        public AudioClip gameOverMusic;
        
        private AudioSource musicSource;
        private string currentlyPlayingMusic = "";
        private GameModel gameModel;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Create music source
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.parent = transform;
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume * masterVolume;
            
            // Create individual audio sources for each sound effect
            foreach (SoundEffect sound in soundEffects)
            {
                GameObject obj = new GameObject("Sound_" + sound.name);
                obj.transform.parent = transform;
                
                sound.source = obj.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume * sfxVolume * masterVolume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
            
            // Find game model
            gameModel = GameModel.Instance;
            if (gameModel != null)
            {
                gameModel.OnGamePaused += OnGamePaused;
                gameModel.OnGameResumed += OnGameResumed;
                gameModel.OnGameOver += OnGameOver;
            }
        }
        
        private void Start()
        {
            PlayMusic("MainMenu");
        }
        
        public void PlayMusic(string type)
        {
            if (currentlyPlayingMusic == type) return;
            
            AudioClip clip = null;
            switch (type)
            {
                case "MainMenu":
                    clip = mainMenuMusic;
                    break;
                case "Gameplay":
                    clip = gameplayMusic;
                    break;
                case "Boss":
                    clip = bossMusic;
                    break;
                case "GameOver":
                    clip = gameOverMusic;
                    break;
            }
            
            if (clip != null)
            {
                musicSource.clip = clip;
                musicSource.Play();
                currentlyPlayingMusic = type;
            }
        }
        
        public void PlaySFX(string name)
        {
            SoundEffect sound = soundEffects.Find(s => s.name == name);
            if (sound != null && sound.source != null)
            {
                sound.source.Play();
            }
        }
        
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateAllVolumes();
        }
        
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            if (musicSource != null)
            {
                musicSource.volume = musicVolume * masterVolume;
            }
        }
        
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            foreach (SoundEffect sound in soundEffects)
            {
                if (sound.source != null)
                {
                    sound.source.volume = sound.volume * sfxVolume * masterVolume;
                }
            }
        }
        
        private void UpdateAllVolumes()
        {
            // Update music volume
            if (musicSource != null)
            {
                musicSource.volume = musicVolume * masterVolume;
            }
            
            // Update all SFX volumes
            foreach (SoundEffect sound in soundEffects)
            {
                if (sound.source != null)
                {
                    sound.source.volume = sound.volume * sfxVolume * masterVolume;
                }
            }
        }
        
        private void OnGamePaused()
        {
            if (muteWhenPaused)
            {
                musicSource.volume = 0;
                foreach (SoundEffect sound in soundEffects)
                {
                    if (sound.source != null)
                    {
                        sound.source.volume = 0;
                    }
                }
            }
        }
        
        private void OnGameResumed()
        {
            if (muteWhenPaused)
            {
                UpdateAllVolumes();
            }
        }
        
        private void OnGameOver()
        {
            PlayMusic("GameOver");
        }
        
        private void OnDestroy()
        {
            if (gameModel != null)
            {
                gameModel.OnGamePaused -= OnGamePaused;
                gameModel.OnGameResumed -= OnGameResumed;
                gameModel.OnGameOver -= OnGameOver;
            }
        }
    }
}