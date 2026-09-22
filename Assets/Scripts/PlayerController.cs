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
    
    public bool isGrounded;
    public bool isMoving;
    public bool isNotMoving;
    public bool isOnSlope;
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
        cc.Move(Movement.velocity * Time.deltaTime);
    }

    public void Move()
    {
        Movement.HandleMovementSetUp();
        Movement.HandleGroundedMovement();
    }

    public void Gravity()
    {
       // playerMovement.HandleGravity();
    }


}