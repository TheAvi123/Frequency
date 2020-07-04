using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    //State Variables
    protected float inputTimer;                           //Duration that Input is Recieved
    protected float inputDistance;                        //Used to Distinguish Click and Drag
    protected Vector2 startPos, endPos;                   //Coordinates for Mouse Start and End Positions

    //Abstract Methods
    protected abstract void InputBegan();

    protected abstract void InputStay();

    protected abstract void InputMove();

    protected abstract void InputEnd();
}
