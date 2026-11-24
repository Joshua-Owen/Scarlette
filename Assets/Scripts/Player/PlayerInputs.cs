
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    InputAction moveAction, JumpAction, lookAction, lockOnAction, switchTargetLeftAction, switchTargetRightAction, dodgeAction, hackAction, hackInputAction, HackConfirmAction, attackAction;
    
    public Vector2 MoveInput {get; private set;}
    public Vector2 LookInput {get; private set;}
    public bool JumpPressed {get; private set;}
    public bool LockOnPressed {get; private set;}
    public bool SwitchTargetInputLeft {get; private set;}
    public bool SwitchTargetInputRight {get; private set;}
    public bool DodgePressed {get; private set;}
    public bool HackPressed {get; private set;}
    public bool HackReleased {get; private set;}
    public bool HackConfirm {get; private set;}
    public Vector2 HackInput { get; private set;}
    public bool attackInput { get; private set;}
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        lockOnAction = InputSystem.actions.FindAction("LockOn");
        switchTargetLeftAction = InputSystem.actions.FindAction("SwitchTargetLeft");
        switchTargetRightAction = InputSystem.actions.FindAction("SwitchTargetRight");
        dodgeAction = InputSystem.actions.FindAction("Dodge");
        hackAction = InputSystem.actions.FindAction("Hack");
        hackInputAction = InputSystem.actions.FindAction("HackInput");
        attackAction = InputSystem.actions.FindAction("Attack");
        HackConfirmAction = InputSystem.actions.FindAction("Attack");

        moveAction?.Enable();
        JumpAction?.Enable();
        lookAction?.Enable();
        lockOnAction?.Enable();
        switchTargetLeftAction?.Enable();
        switchTargetRightAction?.Enable();
        dodgeAction?.Enable();
        hackAction?.Enable();
        hackInputAction?.Enable();
        attackAction?.Enable();
        HackConfirmAction?.Enable();
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
        ReadHackToggle();
        ReadHackInput();
        ReadAttackInput();
        ReadHackConfirmAction();
    }

    void ReadMovementInput() => MoveInput = moveAction.ReadValue<Vector2>();

    void ReadJumpInput() => JumpPressed = JumpAction.WasPressedThisFrame();

    void ReadLookInput() => LookInput = lookAction.ReadValue<Vector2>();

    void ReadLockOnInput() => LockOnPressed = lockOnAction.WasPressedThisFrame();
    
    void ReadSwitchTargetInputLeft() => SwitchTargetInputLeft = switchTargetLeftAction.WasPerformedThisFrame();
    void ReadSwitchTargetInputRight() => SwitchTargetInputRight = switchTargetRightAction.WasPressedThisFrame();
    void ReadDodgeInput() => DodgePressed = dodgeAction.WasPressedThisFrame();
    void ReadAttackInput() => attackInput = attackAction.WasPressedThisFrame();

    void ReadHackToggle()
    {
        HackPressed = hackAction.ReadValue<float>() >= 0.8;
        HackReleased = hackAction.ReadValue<float>() < 0.8;
        //HackPressed = hackAction.WasPressedThisFrame();
        //HackReleased = hackAction.WasReleasedThisFrame();
        //HackInProgress = hackAction.IsInProgress();

    }

    void ReadHackInput()
    {
        HackInput = Vector2.zero;
        if(hackInputAction.WasPressedThisFrame())
        {
            HackInput = hackInputAction.ReadValue<Vector2>();

        }

    }

    public void ReadHackConfirmAction() => HackConfirm = HackConfirmAction.WasPressedThisFrame();


}