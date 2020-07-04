using UnityEngine;
using UnityEngine.Events;

public class UIMouseInputController : UIInputController
{
    //Configuration Parameters
    [Header("Max Click Parameters")]
    [SerializeField] float maxClickTimer = 0.5f;        //Max Time for Input to be Considered a Click
    [SerializeField] float maxClickDistance = 0.1f;     //Max Distance for Input to be Considered a Click   

    //Internal Methods
    private void Awake() {
        VerifyInputType();
    }

    private void VerifyInputType() {
        //Uses TouchInputController if Touch is Supported
        if (Input.touchSupported) {
            enabled = false;    //Disable this component
        }
    }

    private void OnMouseDown() {
        InputBegan();
    }

    private void OnMouseDrag() {
        InputMove();
    }

    private void OnMouseUp() {
        InputEnd();
    }

    //Inherited Methods
    protected override void InputBegan() {
        startPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        inputTimer = 0;     //Reset Timer
    }

    protected override void InputStay() {
        //No Functionality Required for MouseController
    }

    protected override void InputMove() {
        inputTimer += Time.deltaTime;   //Increase Timer per Frame the Mouse is Held Down
    }

    protected override void InputEnd() {
        endPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        inputDistance = Vector2.Distance(startPos, endPos);     //Calculate Distance

        if (inputTimer <= maxClickTimer && inputDistance <= maxClickDistance) {
            //Player Tapped
            tapAction.Invoke();
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

    //Debugging Methods
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Debug.Break();
        }
    }
}
