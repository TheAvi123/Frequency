using UnityEngine;

public class TouchInputController : MonoBehaviour
{
    ///Reference Variables
    private PlayerWave player = null;                   //Player Movement Component

    [Header("Input Time")]
    private float inputTimer;                           //Duration that Input is Recieved
    [SerializeField] float maxTapTimer = 0.5f;          //Max Time for Input to be Considered a Tap
    private float stayTimer;                            //Duration that Input is Stationary
    [SerializeField] float maxStayTimer = 0.1f;         //Max Time for Input to be Stationary

    [Header("Input Distance")]
    private Vector2 startPos, endPos;                   //Coordinates for Touch Start and End Positions
    private float inputDistance;                        //Used to Distinguish Tap and Swipe
    [SerializeField] float maxTapDistance = 0.1f;       //Max Distance for Input to be Considered a Tap

    [Header("Touch Input")]
    private Touch currentTouch;
    private bool sameTouch;

    private void Awake() {
        VerifyInputType();
        FindPlayer();
    }

    private void VerifyInputType() {
        //Uses MouseInputController if Touch is Not Supported
        if (!Input.touchSupported) {
            gameObject.SetActive(false);    //Disable Object
        }
    }

    private void FindPlayer() {
        player = FindObjectOfType<PlayerWave>();
        if (!player) {
            Debug.LogError("No Player Object Found");
            gameObject.SetActive(false);    //Disable Object
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
                    TouchMoving();
                    break;
                case TouchPhase.Stationary:
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
        stayTimer = 0;      //Reset Timer
        sameTouch = false;
    }

    private void TouchMoving() {
        inputTimer += Time.deltaTime;   //Increase Timer per Frame the Touch is Moving
    }

    private void TouchStay() {
        stayTimer += Time.deltaTime;   //Increase Timer per Frame the Touch is Stationary
        if (!sameTouch && stayTimer >= maxStayTimer) {
            TouchEnd();
            sameTouch = true;
        }
    }

    private void TouchEnd() {
        if (sameTouch) {
            return;
        }
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
