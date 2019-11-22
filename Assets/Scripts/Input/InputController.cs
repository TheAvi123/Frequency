using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    //Reference Variables
    protected PlayerWave player = null;   //Player Movement Component

    //State Variables
    protected float inputTimer;                           //Duration that Input is Recieved
    protected float inputDistance;                        //Used to Distinguish Click and Drag
    protected Vector2 startPos, endPos;                   //Coordinates for Mouse Start and End Positions

    //Internal Methods
    protected void Awake() {
        FindPlayer();
    }

    private void FindPlayer() {
        player = FindObjectOfType<PlayerWave>();
        if (!player) {
            Debug.LogError("No Player Object Found");
            gameObject.SetActive(false);    //Disable Object
        }
    }

    //Abstract Methods
    protected abstract void InputBegan();

    protected abstract void InputStay();

    protected abstract void InputMove();

    protected abstract void InputEnd();
}
