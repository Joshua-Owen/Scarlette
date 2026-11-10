using UnityEngine;

public class AttackState : GroundedState
{
    
    public AttackState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Anim.animator.CrossFade("Attack Movement", 0.1f);
    }
    public override void Update()
    {
        base.Update();
        player.Move();
        player.Movement.HandleRotation();
        player.Hack.HackToggle();
        if(player.Hack.isHacking)
        {
            stateMachine.ChangeState(stateMachine.hackState);
            return;
        }

        if(!player.TargetFndr.isLockedOn && player.Movement.currentSpeed == 0)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
        if(!player.TargetFndr.isLockedOn && player.isMoving)
        {
            stateMachine.ChangeState(stateMachine.moveState);
            return;
        }
        if (player.Inputs.DodgePressed)
        {
            stateMachine.ChangeState(stateMachine.dodgeState);
            return;
        }
        if(player.Inputs.JumpPressed)
        {
            stateMachine.ChangeState(stateMachine.jumpState);
            return;
        }
        
    }
}