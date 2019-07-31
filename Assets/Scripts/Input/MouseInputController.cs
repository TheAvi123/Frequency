using UnityEngine;

public class MouseInputController : MonoBehaviour
{
    ///Reference Variables
    private PlayerWave player = null;                   //Player Movement Component

    [Header("Input Time")]
    private float inputTimer;                           //Duration that Input is Recieved
    [SerializeField] float maxClickTimer = 0.5f;        //Max Time for Input to be Considered a Click

    [Header("Input Distance")]
    private Vector2 startPos, endPos;                   //Coordinates for Mouse Start and End Positions
    private float inputDistance;                        //Used to Distinguish Click and Drag
    [SerializeField] float maxClickDistance = 0.1f;     //Max Distance for Input to be Considered a Click   

    private void Awake() {
        VerifyInputType();
        FindPlayer();
    }

    private void VerifyInputType() {
        //Uses TouchInputController if Touch is Supported
        if (Input.touchSupported) {
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

    private void OnMouseDown() {
        startPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        inputTimer = 0;     //Reset Timer
    }

    private void OnMouseDrag() {
        inputTimer += Time.deltaTime;   //Increase Timer per Frame the Mouse is Held Down
    }

    private void OnMouseUp() {
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

    //These Methods Are For Debugging Purposes
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Debug.Break();
        }
    }
}
