using UnityEngine;
using UnityEngine.Events;

public class UITouchInputController : UIInputController
{
    //Configuration Parameters
    [Header("Max Tap Parameters")]
    [SerializeField] float maxTapTimer = 0.5f;          //Max Time for Input to be Considered a Tap
    [SerializeField] float maxStayTimer = 0.1f;         //Max Time for Stationary Input before Evaluation
    [SerializeField] float maxTapDistance = 0.1f;       //Max Distance for Input to be Considered a Tap

    //Input State Variables
    private float stayTimer;                            //Duration that Input is Stationary

    //Touch State Variables
    private Touch currentTouch;
    private bool sameTouch;

    //Internal Methods
    private void Awake() {
        VerifyInputType();
    }

    private void VerifyInputType() {
        //Uses MouseInputController if Touch is Not Supported
        if (!Input.touchSupported) {
            enabled = false;    //Disable this component
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
                    InputBegan();
                    break;
                case TouchPhase.Moved:
                    InputMove();
                    break;
                case TouchPhase.Stationary:
                    InputStay();
                    break;
                case TouchPhase.Ended:
                    InputEnd();
                    break;
            }
        }
    }

    //Inherited Methods
    protected override void InputBegan() {
        startPos = Camera.main.ScreenToViewportPoint(currentTouch.position);
        inputTimer = 0;     //Reset Timer
        stayTimer = 0;      //Reset Timer
        sameTouch = false;
    }

    protected override void InputStay() {
        stayTimer += Time.deltaTime;   //Increase Timer per Frame the Touch is Stationary
        if (!sameTouch && stayTimer >= maxStayTimer) {
            InputEnd();
            sameTouch = true;
        }
    }

    protected override void InputMove() {
        inputTimer += Time.deltaTime;   //Increase Timer per Frame the Touch is Moving
    }

    protected override void InputEnd() {
        if (sameTouch) {
            return;
        }
        endPos = Camera.main.ScreenToViewportPoint(currentTouch.position);
        inputDistance = Vector2.Distance(startPos, endPos);     //Calculate Distance

        if (inputTimer <= maxTapTimer && inputDistance <= maxTapDistance) {
            //Player Tapped
            tapAction();
        } else {
            float inputDistanceY = endPos.y - startPos.y;    //Vertical Swipe Distance
            float inputDistanceX = endPos.x - startPos.x;    //Horizontal Swipe Distance
            if (Mathf.Abs(inputDistanceY) > Mathf.Abs(inputDistanceX)) {
                //Vertical Swipe
                if (inputDistanceY > 0) {
                    //Player Swiped Up
                    upSwipeAction();
                } else {
                    //Player Swiped Down
                    downSwipeAction();
                }
            } else {
                //Horizontal Swipe
                if (inputDistanceX > 0) {
                    //Player Swiped Right
                    rightSwipeAction();
                } else {
                    //Player Swiped Left
                    leftSwipeAction();
                }
            }
        }
    }

    //Public Methods
    public override void SetTapAction(UnityAction action) {
        tapAction = action;
    }

    public override void SetLeftSwipeAction(UnityAction action) {
        leftSwipeAction = action;
    }

    public override void SetRightSwipeAction(UnityAction action) {
        rightSwipeAction = action;
    }

    public override void SetUpSwipeAction(UnityAction action) {
        upSwipeAction = action;
    }

    public override void SetDownSwipeAction(UnityAction action) {
        downSwipeAction = action;
    }
}
