using Music;

using UnityEngine;

namespace UserInterface.Buttons {
    public class MuteButton : MonoBehaviour
    {
        private void OnMouseUp() {
            FindObjectOfType<MusicPlayer>().ToggleMute();
        }
    }
}
