using UnityEngine;
using UnityEngine.Events;

public abstract class InputController : MonoBehaviour
{
    //Configuration Variables
    [Header("Input Actions")]
    protected UnityAction tapAction;
    protected UnityAction leftSwipeAction;
    protected UnityAction rightSwipeAction;
    protected UnityAction upSwipeAction;
    protected UnityAction downSwipeAction;

    //State Variables
    protected float inputTimer;                           //Duration that Input is Recieved
    protected float inputDistance;                        //Used to Distinguish Click and Drag
    protected Vector2 startPos, endPos;                   //Coordinates for Mouse Start and End Positions

    protected void Awake() {
        SetDefaultActions();
    }

    protected void SetDefaultActions() {
        tapAction = TapAction;
        leftSwipeAction = LeftSwipeAction;
        rightSwipeAction = RightSwipeAction;
        upSwipeAction = UpSwipeAction;
        downSwipeAction = DownSwipeAction;
    }

    //Abstract Methods
    protected abstract void InputBegan();

    protected abstract void InputStay();

    protected abstract void InputMove();

    protected abstract void InputEnd();

    //Default Input Actions
    protected abstract void TapAction();

    protected abstract void LeftSwipeAction();

    protected abstract void RightSwipeAction();

    protected abstract void UpSwipeAction();

    protected abstract void DownSwipeAction();

    //Public Abstract Methods
    public void SetTapAction(UnityAction action) {
        tapAction = action;
    }

    public void SetLeftSwipeAction(UnityAction action) {
        leftSwipeAction = action;
    }

    public void SetRightSwipeAction(UnityAction action) {
        rightSwipeAction = action;
    }

    public void SetUpSwipeAction(UnityAction action) {
        upSwipeAction = action;
    }

    public void SetDownSwipeAction(UnityAction action) {
        downSwipeAction = action;
    }
}
