using UnityEngine;

public class TrailColorChanger : MonoBehaviour
{
    ///Reference Variables
    private TrailRenderer trailRenderer = null;

    private void Awake() {
        FindPlayerTrail();
    }

    private void FindPlayerTrail() {
        trailRenderer = GetComponent<TrailRenderer>();
        if (!trailRenderer) {
            Debug.LogError("No Trail Renderer Found On The Player");
        }
    }
}
