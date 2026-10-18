using JetBrains.Annotations;
using UnityEngine;

public class MoveState : GroundedState
{
    public MoveState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }          
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered Moving");
    }

    public override void Update()
    {
        base.Update();  
        player.Move();
        player.Movement.HandleRotation();
        //Debug.Log("Updating Moving");
        if (player.Movement.currentSpeed == 0)
        {
            stateMachine.ChangeState(stateMachine.idleState);
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

       player.Dodge();
    }


    public override void Exit()
    {
        base.Exit();
    }
}