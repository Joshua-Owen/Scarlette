
using System.Collections.Generic;
using UnityEngine;


public class TargetFinder : MonoBehaviour
{
    [SerializeField] Transform cameraTransfrom;
    [SerializeField] PlayerInputs input;
    [SerializeField] Transform player;
    [SerializeField] float rangeRadius = 5f;
    [SerializeField] float rangeOriginOffset = 4f;
    public bool isLockedOn;
    [SerializeField] LayerMask targetLayer;
    Vector3 rangeOrigin;
    [SerializeField] List<Transform> targetList;

    public Transform currentTarget;
    //float currentTargetDistanceSqr;

    [Header("Obstruction")]
    [SerializeField] float maxObstructionTime;
    float currentObstructionTime;
    enum eSwitchTargetDirection {none, left,right};
    [Header("Switch Target")]
    [SerializeField] eSwitchTargetDirection switchTargetDirection;
    [SerializeField]GameObject lockOnIndicator;
    [SerializeField] RectTransform canvasRect;

    void Update()
    {
        HandleLockOnStatus();
        HandleCurrentTarget();
        HandleSwitchTargetInput();
        HandleLockOnIndicator();
    }

    private void HandleLockOnIndicator()
    {
        if(!isLockedOn) 
        {
            lockOnIndicator.SetActive(false);
            return;
        }
        else
        {
            lockOnIndicator.SetActive(true);
            
            var screenPosition = Camera.main.WorldToScreenPoint(currentTarget.position);
            if (screenPosition.z > 0)
            {
                lockOnIndicator.transform.position = screenPosition;
            }
        
        }
    }


    private void HandleSwitchTargetInput()
    {
        if (!isLockedOn) return;
        if (input.SwitchTargetInputRight)
        {
            switchTargetDirection = eSwitchTargetDirection.right;
            currentTarget = HandleTargetSwitch();
     
        }
        else if (input.SwitchTargetInputLeft)
        {
            switchTargetDirection = eSwitchTargetDirection.left;
            currentTarget = HandleTargetSwitch();
        }
        else
        {
            switchTargetDirection = eSwitchTargetDirection.none;
            return;
        }
        
    }

    private void HandleCurrentTarget()
    {
        if(!currentTarget) 
        {
            isLockedOn = false;
            return;
        }
        
   
        var currentTargetDistance = (cameraTransfrom.position - currentTarget.position).magnitude;
        if(currentTargetDistance > rangeRadius * 2)
        {
            
            isLockedOn = false;
            currentTarget = null;
            currentObstructionTime = 0f;
            return;
        }

        RaycastHit hit;
        var directionToTarget = currentTarget.position - cameraTransfrom.position;
        
        var obstructionCheck = Physics.Raycast(cameraTransfrom.position, directionToTarget.normalized, out hit , directionToTarget.magnitude);
        
        if (obstructionCheck && !hit.collider.tag.Equals("Enemy") && !hit.collider.tag.Equals("Player"))
        {
            currentObstructionTime += Time.deltaTime;
           
            if(currentObstructionTime > maxObstructionTime)
            {
                isLockedOn = false;
                currentTarget = null;
                currentObstructionTime = 0f;
            }
        }
        else
        {
            currentObstructionTime = 0f;
        }

    }   

    void OutOfRangeCheck()
    {
        RemoveDestroyedTargets();
        if(targetList.Count <= 0) return;

        for (var targetIndex = targetList.Count - 1; targetIndex >= 0; targetIndex--)
        {
            var target = targetList[targetIndex];
            var targetDistance = (target.position - player.position).magnitude;

            if(targetDistance > rangeRadius * 2)
            {
                targetList.RemoveAt(targetIndex);
            }
        }
    }

    void HandleLockOnStatus()
    {
        RemoveDestroyedTargets();

        if(input.LockOnPressed && !isLockedOn)
        {
            FindTargets();

        }
        else if(input.LockOnPressed && isLockedOn )
        {
            targetList.Clear();
            currentTarget = null;
            isLockedOn = false;

        }
        if(!isLockedOn)
        {

            targetList.Clear();
        }
        if(isLockedOn)
        {
            FindTargets();
            OutOfRangeCheck();
        }
    }
    void RemoveDestroyedTargets()
    {
        targetList.RemoveAll(target => target == null);

        if (currentTarget == null)
        {

            isLockedOn = false;
            currentObstructionTime = 0f;
        }
    }
    private void FindTargets()
    {
        RaycastHit hit;
        rangeOrigin = cameraTransfrom.position + cameraTransfrom.forward * rangeOriginOffset;
        rangeOrigin.y = 0;
        var searchRange = Physics.OverlapSphere(rangeOrigin, rangeRadius, targetLayer);

        if (searchRange.Length > 0)
        {
            foreach (var target in searchRange)
            {
                var targetDirection = target.transform.position - transform.position;
                //stops enemies behind walls being pick up
                var obstructionCheck = Physics.Raycast(transform.position, targetDirection,out hit , rangeRadius * 2f);
               
                if (obstructionCheck && hit.collider.tag.Equals("Enemy"))
                {
                    

                    if (!targetList.Contains(target.transform))
                    {
                        targetList.Add(target.transform);
                    }
                }
                else
                {
                    continue;
                }


            }
        }

        if( targetList.Count > 0 && !currentTarget)
        {
            currentTarget = FindClosestTarget();
            isLockedOn = true;
            
        }
    }

    Transform FindClosestTarget()
    {
        
        Debug.Log("beep boop");
        RemoveDestroyedTargets();
        if(targetList.Count <= 0)
        {
            currentTarget = null;
            return null;
        } 

        var closestTarget = targetList[0];
        var currentTargetDistanceSqr = float.MaxValue;
        
        foreach (var target in targetList)
        {
            var targetDistanceSqr = (target.position - player.position).sqrMagnitude ;
            
            if( targetDistanceSqr < currentTargetDistanceSqr)
            {
                currentTargetDistanceSqr = targetDistanceSqr;
                closestTarget = target;
            }
        }
        
        return closestTarget;
    }
    Transform HandleTargetSwitch()
    {
        if(!currentTarget && switchTargetDirection != eSwitchTargetDirection.none) return null;
        RemoveDestroyedTargets();
        var currentClosestRightTarget = float.MaxValue;
        var currentClosestLeftTarget = float.MinValue;

          
        var closestLeftPosition = currentTarget;
        var closestRightPosition = currentTarget;
        foreach (var target in targetList)
        {
            if (!target.Equals(currentTarget))
            {
                              
                var directionToTarget = target.position - currentTarget.position;
                directionToTarget.y  = 0;
            
                var TargetPos = Vector3.Dot(transform.right, directionToTarget);
        
                if ( TargetPos < 0 && TargetPos > currentClosestLeftTarget && switchTargetDirection == eSwitchTargetDirection.left)
                {
                    currentClosestLeftTarget = TargetPos;
                    closestLeftPosition = target;
                }
                else if ( TargetPos > 0 && TargetPos < currentClosestRightTarget && switchTargetDirection == eSwitchTargetDirection.right)
                {
                    currentClosestRightTarget = TargetPos;
                    closestRightPosition = target;
                }
            } 
        }

        if(switchTargetDirection == eSwitchTargetDirection.left)
        {
            Debug.Log($"closest : {closestLeftPosition}");

            return closestLeftPosition;
        }
        else
        {
            Debug.Log($"closest : {closestRightPosition}");

            return closestRightPosition;
        }
    }
            
        
    
    void OnDrawGizmos()
    {
        rangeOrigin = cameraTransfrom.position + cameraTransfrom.forward * rangeOriginOffset;

        Gizmos.DrawWireSphere(rangeOrigin, rangeRadius);
        if(currentTarget != null)
        Gizmos.DrawLine(transform.position, currentTarget.position);
    }
}
