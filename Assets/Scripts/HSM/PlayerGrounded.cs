using UnityEngine;

public class PlayerGrounded :MonoBehaviour
{
    public bool IsGrounded {get; private set;}
    public RaycastHit groundHit;
    public float distanceToGround = 1;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float maxSlopeAngle =  45f;
    public bool IsOnSteepSlope {get; private set;}


    public void HandleGroundCheck()
    { 
        
        var groundCastHit = Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out groundHit, distanceToGround, groundLayer);
        var minGroundNormalY = Mathf.Cos(maxSlopeAngle * Mathf.Deg2Rad); 


        if(groundCastHit)
        {
            if(groundHit.normal.y >= minGroundNormalY)
            {
                IsGrounded = true;
                IsOnSteepSlope = false;
            }
            else
            {
                IsGrounded = true;
                IsOnSteepSlope = true;
            }
        }
        else
        {
            IsGrounded = false;
            IsOnSteepSlope = false;
        }
    }


     void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;

        var gizmosPosition = new Vector3(transform.position.x,transform.position.y - distanceToGround,transform.position.z);
        
        Gizmos.DrawWireSphere(gizmosPosition, groundCheckRadius);
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}