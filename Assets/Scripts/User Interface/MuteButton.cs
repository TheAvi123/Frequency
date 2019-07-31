using UnityEngine;

public class MuteButton : MonoBehaviour
{
    private void OnMouseDown() {
        FindObjectOfType<MusicPlayer>().ToggleMute();
    }
}
