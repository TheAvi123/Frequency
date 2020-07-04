using UnityEngine;
using UnityEngine.Events;

public abstract class UIInputController : InputController
{
    //Configuration Variables
    [Header("Input Actions")]
    protected UnityAction tapAction;
    protected UnityAction leftSwipeAction;
    protected UnityAction rightSwipeAction;
    protected UnityAction upSwipeAction;
    protected UnityAction downSwipeAction;

    //Abstract Methods
    protected override abstract void InputBegan();

    protected override abstract void InputStay();

    protected override abstract void InputMove();

    protected override abstract void InputEnd();

    //Public Abstract Methods
    public abstract void SetTapAction(UnityAction action);

    public abstract void SetLeftSwipeAction(UnityAction action);

    public abstract void SetRightSwipeAction(UnityAction action);

    public abstract void SetUpSwipeAction(UnityAction action);

    public abstract void SetDownSwipeAction(UnityAction action);
}
