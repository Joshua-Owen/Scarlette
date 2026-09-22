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
        player.Movement.HandleMovementSetUp();
        player.Movement.HandleAirbourneMovement();
        if (player.Grounded.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
        
    }
    
}

