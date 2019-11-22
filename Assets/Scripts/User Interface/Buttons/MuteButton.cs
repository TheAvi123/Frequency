using UnityEngine;

public class MuteButton : MonoBehaviour
{
    private void OnMouseUp() {
        FindObjectOfType<MusicPlayer>().ToggleMute();
    }
}
