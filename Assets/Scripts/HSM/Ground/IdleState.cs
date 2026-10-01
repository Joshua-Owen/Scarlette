using UnityEngine;

public class IdleState : GroundedState
{
    public IdleState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered Idle");
    }

    public override void Update()
    {
        base.Update();
        player.Movement.HandleIdleMovement();
        //Debug.Log("Updating Idle");
        if (player.isMoving)
        {
            stateMachine.ChangeState(stateMachine.moveState);
            return;
        }
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
        if (player.isOnSlope)
        {
            stateMachine.ChangeState(stateMachine.slopeState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}