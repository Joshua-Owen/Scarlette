
using Unity.Mathematics;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{    
    [SerializeField] PlayerGrounded playerGrounded;
    CharacterController cc;
    PlayerInputs input;
    Transform cameraTransform;
    TargetFinder targetFinder;
    
    [Header("Movement")]
    
    public Vector3 velocity;

    
    public Vector3 DesiredDirection;
    [SerializeField] float groundGravity = -2f;

   
    Vector3 InputCameraDirection;
    public Vector3 inputDirection;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float rotationSpeed = 540f;
    [SerializeField] float airRotationSpeed = 54f;
    [SerializeField] float rotationAcceleration = 1000f;
    public float currentRotationSpeed;
    [SerializeField] float acceleration = 10f;

    [SerializeField] float deceleration = 20f;

    public float currentSpeed;
    [Header("Jump")]
    [SerializeField] float jumpHeight = 2f;
    const float GRAVITY = -9.81f * 2;
    //bool isGrounded;
    /*[Header("GroundCheck")]
    RaycastHit hit;
    [SerializeField]float distanceToGround = 1;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundGravity = -2f;
    [SerializeField] float maxSlopeAngle =  45f;*/
    [Header("Airbourne")]
    [SerializeField] float aircontrol = 0.5f;
    [Header("Slope")]
    [SerializeField] float slideSpeed = 5f;
    bool isOnSteepSlope;

    [Header("Dodge")]
    [SerializeField] float dodgeSpeed = 10;
    [SerializeField] float backStepSpeed = 3;
    [SerializeField] float dodgeDuration = 0.5f;
    public bool isDodging;
    float dodgeTimer;
    Vector3 dodgeDirection;
    void Start()
    {

        cameraTransform = Camera.main.transform;
        cc = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputs>();
        targetFinder = cameraTransform.GetComponent<TargetFinder>();
    }

   void Update()
    { 
        /*
        HandleGroundCheck();
        HandleMovement();
        HandleGravity();
        HandleJump();
        */
        
       
    }
    

    

#region movement    
    public void HandleMovementSetUp()
    {
        SetUpMovementInputs();

        if (InputCameraDirection.sqrMagnitude > 0.1f) 
        {
            DesiredDirection = InputCameraDirection;
            DesiredDirection.y  = 0;
        }
        /*
        if(isGrounded)
            HandleGroundedMovement();
        
        else if (isOnSteepSlope)
        {
            HandleSlopeSlide();
        }
        else
        {
            HandleAirbourneMovement();
        }*/
    
        Vector3 facingDirection = DesiredDirection;
        facingDirection.y = 0;

        /*if(targetFinder.currentTarget)
        {
            facingDirection =  targetFinder.currentTarget.position - transform.position;
            facingDirection.y = 0;
            HandleRotation(facingDirection);
        }
        if (InputCameraDirection.sqrMagnitude >= 0.0001f)
        {
            HandleRotation(facingDirection);
        }
        */
    }

    void SetUpMovementInputs()
    {
        inputDirection = new Vector3(input.MoveInput.x, 0, input.MoveInput.y);
        
        if (inputDirection.sqrMagnitude > 1f) inputDirection.Normalize();

        CameraMovementSetUp(inputDirection);      
    }

    private void CameraMovementSetUp(Vector3 inputDirection)
    {
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
         
        if (cameraForward.sqrMagnitude < 0.0001f)
        {
            cameraForward = transform.forward;
            cameraForward.y = 0;
        }

        if (cameraRight.sqrMagnitude < 0.0001f)
        {
            cameraRight = transform.right;
            cameraRight.y = 0;
        }

        cameraForward.Normalize();
        cameraRight.Normalize();

        InputCameraDirection = cameraRight * inputDirection.x + cameraForward * inputDirection.z;
        InputCameraDirection.y = 0;
    }  

    public void HandleRotation()
    {
        var facingDirection = DesiredDirection;
        facingDirection.y = 0;

         if(targetFinder.currentTarget)
        {
            facingDirection =  targetFinder.currentTarget.position - transform.position;
            facingDirection.y = 0;
        }
        var targetRotation = Quaternion.LookRotation(facingDirection);
        float targetRotationSpeed;

        if (playerGrounded.IsGrounded)
        {
            currentRotationSpeed = rotationSpeed; 
        }
        else
        {
            currentRotationSpeed = airRotationSpeed;
        }
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
     
         
    }
    void HandleLockOnRotation()
    {
        if (!targetFinder.currentTarget)    return;

        var playerFaceTarget = cameraTransform.rotation;
        playerFaceTarget.x = 0;
        playerFaceTarget.z = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerFaceTarget, rotationAcceleration * Time.deltaTime);
           
    }

    public void HandleIdleMovement()
    {
        var idleVelocity = new Vector3(0, velocity.y, 0);
        velocity = Vector3.MoveTowards(velocity, idleVelocity, deceleration * Time.deltaTime);
    }
    public void HandleGroundedMovement()
    {

        DesiredDirection = Vector3.ProjectOnPlane(DesiredDirection, playerGrounded.groundHit.normal).normalized;
        

        HandleSpeed();

        Vector3 movement = DesiredDirection * currentSpeed;

        velocity.x = movement.x;
        velocity.z = movement.z;
    }
    private void HandleSpeed()
    {
        float targetSpeed = InputCameraDirection.sqrMagnitude >= 0.1f ? maxSpeed : 0f;

        currentSpeed = targetSpeed > currentSpeed ?
                Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime)
                : Mathf.MoveTowards(currentSpeed, targetSpeed, deceleration * Time.deltaTime);
        if(!playerGrounded.IsGrounded){currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * aircontrol * Time.deltaTime);}
    
    }

    public void HandleAirbourneMovement()
    {
        var currentAirVelocity = velocity;
        currentAirVelocity.y = 0;

        HandleSpeed();

        var targetAirVelocity = DesiredDirection * currentSpeed;

        var newAirVelocity = Vector3.MoveTowards(currentAirVelocity,targetAirVelocity,aircontrol * Time.deltaTime);
        
        velocity.x= newAirVelocity.x;
        velocity.z = newAirVelocity.z;
    }

#endregion

    public void HandleGroundGravity()
    {
        velocity.y = groundGravity;
    }
    public void HandleAirGravity()
    {
        float maxGravity = GRAVITY * 6f;

        velocity.y += GRAVITY * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, maxGravity, velocity.y);
        
    }
    public void HandleSlopeSlide()
    {
        var slideDirection = Vector3.ProjectOnPlane(Vector3.down, playerGrounded.groundHit.normal).normalized;
       
       Debug.DrawRay(transform.position, slideDirection * 3f, Color.red);

        Vector3 slopeInput = Vector3.ProjectOnPlane(DesiredDirection, playerGrounded.groundHit.normal);

        float slideDot = Vector3.Dot(slopeInput, slideDirection);
       
        if (slideDot < 0f)
        {
            slopeInput -= slideDirection * slideDot;
        }

        Vector3 slideMovement = slideDirection * slideSpeed;
       
        Vector3 finalSlopeMovment = slopeInput + slideMovement;
      
        //velocity.x = finalSlopeMovment.x;
        //velocity.z = finalSlopeMovment.z;
        velocity = finalSlopeMovment;
    }
    public void HandleJump()
    {
        // if(input.JumpPressed && isGrounded)
        // {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
        //}
    }

    public void HandleDodge()
    {
        Vector2 moveInput = input.MoveInput;
        dodgeTimer += Time.deltaTime;
        dodgeDirection.y = -99f;
        //cc.Move(dodgeDirection * dodgeSpeed * Time.deltaTime);

        
        velocity.x = dodgeDirection.x * dodgeSpeed;
        velocity.z = dodgeDirection.z * dodgeSpeed;

        if(dodgeTimer >= dodgeDuration)
        {
            playerGrounded.distanceToGround -= 0.5f; 
            isDodging = false;
            dodgeDirection.y = groundGravity;
            velocity = Vector3.zero;
        }
    }

    public void StartDodge()
    {
        isDodging = true;
        dodgeTimer = 0f;
        var backStep = -transform.forward;

        playerGrounded.distanceToGround += 0.5f; 

        Vector2 moveInput = input.MoveInput;

        if (moveInput.sqrMagnitude > 0.1f)
        {
            dodgeDirection = cameraTransform.right * moveInput.x + cameraTransform.forward * moveInput.y;
        }
        else
        {
            dodgeDirection = backStep;
        }
    }


}
