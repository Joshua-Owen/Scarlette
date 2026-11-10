using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator animator;
    [SerializeField] PlayerMovement playerMovement;
    public PlayerMovement Movement => playerMovement;
    [SerializeField] PlayerInputs playerInputs;
    public PlayerInputs Inputs => playerInputs;

    [SerializeField] PlayerController playerController;
    public PlayerController Controller => playerController;
    public float currentSpeed;
    public bool isJumping;
    public Vector3 moveDirection;
    void Start()
    {
        currentSpeed = 0;
    }
    void Update()
    {
        currentSpeed = Movement.currentSpeed;
        animator.SetFloat("currentSpeed", currentSpeed);

        moveDirection = new Vector3(Inputs.MoveInput.x, Movement.velocity.y, Inputs.MoveInput.y);
        animator.SetFloat("velocityX", moveDirection.x);
        animator.SetFloat("velocityY", moveDirection.y);
        animator.SetFloat("velocityZ", moveDirection.z);
        
        isJumping = Controller.isJumping;
        animator.SetBool("isJumping", isJumping);


    }
}