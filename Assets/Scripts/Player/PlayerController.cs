using NUnit.Framework;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    public PlayerInputs Inputs => playerInputs;
    [SerializeField] CharacterController cc;
    [SerializeField] PlayerMovement playerMovement;
    public PlayerMovement Movement => playerMovement;
    [SerializeField] PlayerGrounded playerGrounded;
    public PlayerGrounded Grounded => playerGrounded;    
    [SerializeField] PlayerStateMachine playerStateMachine;
    public PlayerStateMachine PlayerSM => playerStateMachine;
    [SerializeField] PlayerHack playerHack;
    public PlayerHack Hack => playerHack;
    [SerializeField] TargetFinder targetFinder;
    public TargetFinder TargetFndr => targetFinder;
    [SerializeField] PlayerAnim playerAnim;
    public PlayerAnim Anim => playerAnim;
    public bool isGrounded;
    public bool isMoving;
    public bool isNotMoving;
    public bool isOnSlope;
    public bool isJumping;
    public Vector3 idleVelocity;
    void Start()
    {
        idleVelocity = new Vector3(0, Movement.velocity.y, 0);
    }
    void Update()
    {
        playerGrounded.HandleGroundCheck();
       // Movement.HandleMovementSetUp();
        isMoving = Inputs.MoveInput.sqrMagnitude > 0.1f;
        isNotMoving = Movement.currentSpeed < 0.1f;
        isOnSlope = Grounded.IsOnSteepSlope;
        isGrounded = Grounded.IsGrounded;
        isJumping = PlayerSM.currentState == PlayerSM.jumpState ? true : false;   
        cc.Move(Movement.velocity * Time.deltaTime);
    }

    public void Move()
    {
        Movement.HandleMovementSetUp();
        Movement.HandleGroundedMovement();
    }

    public void Dodge()
    {
        if(!Movement.isDodging)
        {
            Movement.StartDodge();
        }
        if (Movement.isDodging)
        {
            Movement.HandleDodge();
        }
    }

    public void Gravity()
    {
       // playerMovement.HandleGravity();
    }


}