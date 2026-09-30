using Unity.VisualScripting;
using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered Grounded");
    }

    public override void Update()
    {
        base.Update();
        
        if(player.Inputs.JumpPressed)
        {
            stateMachine.ChangeState(stateMachine.jumpState);
            return;
        }
        if (!player.Grounded.IsGrounded)
        {
           stateMachine.ChangeState(stateMachine.fallState);
           return;
        }
        if(player.Movement.currentSpeed == 0)
        {
            stateMachine.ChangeState(stateMachine.idleState);
           return;
        }
        player.Movement.HandleGroundGravity();
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}