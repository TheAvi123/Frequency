using UnityEngine;

public class MouseInputController : InputController
{
    //Configuration Parameters
    [Header("Max Click Parameters")]
    [SerializeField] float maxClickTimer = 0.5f;        //Max Time for Input to be Considered a Click
    [SerializeField] float maxClickDistance = 0.1f;     //Max Distance for Input to be Considered a Click   

    //Internal Methods
    private new void Awake() {
        VerifyInputType();
        base.Awake();
    }

    private void VerifyInputType() {
        //Uses TouchInputController if Touch is Supported
        if (Input.touchSupported) {
            gameObject.SetActive(false);    //Disable Object
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
            //Player Clicked
            player.Flip();
        } else {
            inputDistance = endPos.y - startPos.y;    //Vertical Distance
            if (inputDistance > 0) {
                //Player Dragged Up
                player.Dash();
            } else if (inputDistance < 0) {
                //Player Dragged Down
                player.Delay();
            } else {
                Debug.LogWarning("Input Parameters Unclear");
            }
        }
        ///For Testing Purposes
        //Debug.Log("Input Time: " + inputTimer);
        //Debug.Log("Input Distance: " + inputDistance);
    }

    //Debugging Methods
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Debug.Break();
        }
    }
}
