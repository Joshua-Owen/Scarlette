using Unity.VisualScripting;
using UnityEngine;

public class FallState : AirbourneState
{
    public FallState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }  

    public override void  Update()
    {
        base.Update();
        
        if (player.Grounded.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
        
    }
    
}

