using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource player = null;

    [SerializeField] float currentVolume = 0.25f;
    [SerializeField] bool muted = false;
    private float setVolume = 0f;

    private void Start() {
        setVolume = currentVolume;
        muted = false;
        player = GetComponent<AudioSource>();
        if (!player) {
            Debug.LogError("No Audio Source Found");
        }
    }

    private void UpdatePlayerVolume() {
        player.volume = currentVolume;
    }

    public void ToggleMute() {
        muted = !muted;
        if (muted) {
            currentVolume = 0f;
            player.Stop();
        } else {
            currentVolume = setVolume;
            player.Play();
        }
        UpdatePlayerVolume();
    }
}
