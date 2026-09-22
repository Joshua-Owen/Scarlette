using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    InputAction moveAction, JumpAction, lookAction, lockOnAction, switchTargetLeftAction, switchTargetRightAction, dodgeAction;
    
    public Vector2 MoveInput {get; private set;}
    public Vector2 LookInput {get; private set;}
    public bool JumpPressed {get; private set;}
    public bool LockOnPressed {get; private set;}
    public bool SwitchTargetInputLeft {get; private set;}
    public bool SwitchTargetInputRight {get; private set;}
    public bool DodgePressed {get; private set;}
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        lockOnAction = InputSystem.actions.FindAction("LockOn");
        switchTargetLeftAction = InputSystem.actions.FindAction("SwitchTargetLeft");
        switchTargetRightAction = InputSystem.actions.FindAction("SwitchTargetRight");
        dodgeAction = InputSystem.actions.FindAction("Dodge");

        moveAction?.Enable();
        JumpAction?.Enable();
        lookAction?.Enable();
        lockOnAction?.Enable();
        switchTargetLeftAction?.Enable();
        switchTargetRightAction?.Enable();
        dodgeAction?.Enable();
    }

    void Update()
    {
        ReadMovementInput();
        ReadJumpInput();
        ReadLookInput();
        ReadLockOnInput();
        ReadSwitchTargetInputLeft();
        ReadSwitchTargetInputRight();
        ReadDodgeInput();
    }

    void ReadMovementInput() => MoveInput = moveAction.ReadValue<Vector2>();

    void ReadJumpInput() => JumpPressed = JumpAction.WasPressedThisFrame();

    void ReadLookInput() => LookInput = lookAction.ReadValue<Vector2>();

    void ReadLockOnInput() => LockOnPressed = lockOnAction.WasPressedThisFrame();
    
    void ReadSwitchTargetInputLeft() => SwitchTargetInputLeft = switchTargetLeftAction.WasPerformedThisFrame();
    void ReadSwitchTargetInputRight() => SwitchTargetInputRight = switchTargetRightAction.WasPressedThisFrame();
    void ReadDodgeInput() => DodgePressed = dodgeAction.WasPressedThisFrame();

}