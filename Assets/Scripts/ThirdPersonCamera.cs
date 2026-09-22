
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    PlayerInputs input;
    TargetFinder targetFinder;
    [SerializeField] Transform player;
    [Header("Follow Player")]
    [SerializeField] float distanceToPlayer = 5f;
    [SerializeField] float heightOfCamera = 2f;
    [Header("Rotate Camera")]
    [Range(0.1f, 500f)]
    [SerializeField] float XSensitivity = 20f;
    [Range(0.1f, 500f)]
    [SerializeField] float YSensitivity = 20f;
    [SerializeField] float cameraSmoothTime = 0.08f;
    [SerializeField] float pitchMin = -30f;
    [SerializeField] float pitchMax = 70;
    [Header("Obstacle ")]
    [SerializeField] float cameraRadius = 0.3f;
    [SerializeField] float minCameraDistance = 0.5f;
    float desiredCameraDistance;
    [Header("LockOn")]
    [SerializeField] float lockOnCameraRotationX = 0f;
    [SerializeField] float lockOnCameraRotationY = 0f;
    [SerializeField] Vector3 lockOnCameraPositionOffset;

    Vector3 currentVelocity;
    Quaternion cameraRotation;
    bool wasLockedOn;

    float yaw = 0;
    float pitch = 0;

    void Start()
    {
                
        input = player.GetComponent<PlayerInputs>();   
        targetFinder = GetComponent<TargetFinder>();

    }
    void LateUpdate()
    {
    
        if (wasLockedOn && !targetFinder.isLockedOn)
        {
            SyncFreeLookRotation();
        }

        HandleCameraRotation();
        HandleLockOnRotation();
        HandleCameraDistance();
        HandleCameraCollision();
        HandlePlayerFollow();

        wasLockedOn = targetFinder.isLockedOn;
    }

    private void HandleCameraRotation()
    {
         if (targetFinder.currentTarget) return;
        


        Vector2 lookInput = input.LookInput;

        float mouseX = lookInput.x * XSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * YSensitivity * Time.deltaTime;

        yaw += mouseX;

        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        cameraRotation = Quaternion.Euler(pitch, yaw, 0f);


        if (targetFinder.currentTarget)
        { 
            cameraRotation = Quaternion.Euler(lockOnCameraRotationX, lockOnCameraRotationY, 0f);
        }
        transform.rotation = cameraRotation;

       

    }

    private void HandleLockOnRotation()
    {
        if (!targetFinder.currentTarget) return;
                
        var currentTargetPosition = targetFinder.currentTarget.position;
           
        var midPoint = player.position + (currentTargetPosition - player.position) / 2f;


        var lockOnCameraDirection = midPoint - transform.position;

        var lockOnRotation = Quaternion.LookRotation(lockOnCameraDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation,lockOnRotation, 5f * Time.deltaTime);
        cameraRotation = transform.rotation;

        
    }

    private void SyncFreeLookRotation()
    {
        var eulerAngles = transform.eulerAngles;
        yaw = eulerAngles.y;
        pitch = eulerAngles.x > 180f ? eulerAngles.x - 360f : eulerAngles.x;
        cameraRotation = transform.rotation;
    }

    private void HandlePlayerFollow()
    {
        Vector3 cameraPosition = player.position + Vector3.up * heightOfCamera - cameraRotation * Vector3.forward * desiredCameraDistance;
        
        transform.position = Vector3.SmoothDamp(transform.position, cameraPosition, ref currentVelocity, cameraSmoothTime);
    }

    private void HandleCameraDistance()
    {
        float desiredLockOnCameraDistance;
        desiredLockOnCameraDistance = distanceToPlayer;
        
        desiredCameraDistance = desiredLockOnCameraDistance;
    }

    void HandleCameraCollision()
    {
        
        Vector3 directionToCamera = -(cameraRotation * Vector3.forward);

        directionToCamera = directionToCamera.normalized;

        RaycastHit hit;
        if(Physics.SphereCast(player.position,cameraRadius,directionToCamera, out hit, desiredCameraDistance))
        {
            var collisionCameraDistance = Mathf.Max(hit.distance  - cameraRadius, minCameraDistance);

            desiredCameraDistance = collisionCameraDistance;
        }
      
      
        
    }
}
