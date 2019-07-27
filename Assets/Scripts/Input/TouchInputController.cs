using UnityEngine;

public class TouchInputController : MonoBehaviour
{
    ///Reference Variables
    private PlayerWave player = null;                   //Player Movement Component

    [Header("Input Time")]
    private float inputTimer;                           //Duration that Input is Recieved
    [SerializeField] float maxTapTimer = 0.5f;          //Max Time for Input to be Considered a Tap

    [Header("Input Distance")]
    private Vector2 startPos, endPos;                   //Coordinates for Touch Start and End Positions
    private float inputDistance;                        //Used to Distinguish Tap and Swipe
    [SerializeField] float maxTapDistance = 0.1f;       //Max Distance for Input to be Considered a Tap

    [Header("Touch Input")]
    private Touch currentTouch;

    private void Awake() {
        VerifyInputType();
        FindPlayer();
    }

    private void VerifyInputType() {
        //Uses MouseInputController if Touch is Not Supported
        if (!Input.touchSupported) {
            gameObject.SetActive(false);
        }
    }

    private void FindPlayer() {
        player = FindObjectOfType<PlayerWave>();
        if (!player) {
            Debug.LogError("No Player Object Found");
        }
    }

    private void Update() {
        UseTouchInput();
    }

    private void UseTouchInput() {
        if (Input.touchCount > 0) {             //Only Execute if Any Touch Input is Detected

            currentTouch = Input.touches[0];    //Set First Touch as Input Touch to be used Later

            switch (currentTouch.phase) {       //Determine Which Phase the Input Touch is in
                case TouchPhase.Began:
                    TouchBegan();
                    break;
                case TouchPhase.Moved:
                    TouchStay();
                    break;
                case TouchPhase.Ended:
                    TouchEnd();
                    break;
            }
        }
    }

    private void TouchBegan() {
        startPos = Camera.main.ScreenToViewportPoint(currentTouch.position);
        inputTimer = 0;     //Reset Timer
    }

    private void TouchStay() {
        inputTimer += Time.deltaTime;   //Increase Timer per Frame the Touch is Detected
    }

    private void TouchEnd() {
        endPos = Camera.main.ScreenToViewportPoint(currentTouch.position);
        inputDistance = Vector2.Distance(startPos, endPos);     //Calculate Distance

        if (inputTimer <= maxTapTimer && inputDistance <= maxTapDistance) {
            //Player Tapped
            player.Flip();
        } else {
            inputDistance = endPos.y - startPos.y;    //Vertical Swipe Distance
            if (inputDistance > 0) {
                //Player Swiped Up
                player.Dash();
            } else if (inputDistance < 0) {
                //Player Swiped Down
                player.Delay();
            } else {
                Debug.LogWarning("Input Parameters Unclear");
            }
        }
        ///For Testing Purposes
        //Debug.Log("Input Time: " + inputTimer);
        //Debug.Log("Input Distance: " + inputDistance);
    }
}
