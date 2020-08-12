using System;
using System.Diagnostics.CodeAnalysis;

using UnityEngine;
using UnityEngine.UI;

using UserInterface.Buttons;

namespace Music {
    public class MusicPlayer : MonoBehaviour
    {
        //Reference Variables
        private AudioSource audioPlayer = null;
        private Image muteButton = null;

        //Sprite References
        [SerializeField] Sprite muteSprite = null;
        [SerializeField] Sprite volumeSprite = null;

        //State Variables
        [SerializeField] float currentVolume = 0.25f;
        [SerializeField] bool muted = false;

        //Set Variables
        private float setVolume = 0f;

        //Internal Methods
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnSceneChange() {
            FindMuteButton();
        }

        private void FindMuteButton() {
            try {
                muteButton = FindObjectOfType<MuteButton>().transform.parent.GetComponent<Image>();
            } catch (NullReferenceException) {
                //No Mute Button Found in Scene... Do Nothing
            }
        }

        private void Start() {
            FindAudioSource();
            InitializeVariables();
        }

        private void FindAudioSource() {
            audioPlayer = GetComponent<AudioSource>();
            if (!audioPlayer) {
                Debug.LogError("No Audio Source Found");
                enabled = false;
            }
        }

        private void InitializeVariables() {
            muted = true;
            setVolume = 0f;
            ChangeButtonSprite(muteSprite);
        }

        private void UpdatePlayerVolume() {
            audioPlayer.volume = currentVolume;
        }

        private void ChangeButtonSprite(Sprite s) {
            if (muteButton) {
                muteButton.sprite = s;
            }
        }

        //Public Methods
        public void ToggleMute() {
            muted = !muted;
            if (muted) {
                currentVolume = 0f;
                ChangeButtonSprite(muteSprite);
                audioPlayer.Stop();
            } else {
                currentVolume = setVolume;
                ChangeButtonSprite(volumeSprite);
                audioPlayer.Play();
            }
            UpdatePlayerVolume();
        }
    }
}