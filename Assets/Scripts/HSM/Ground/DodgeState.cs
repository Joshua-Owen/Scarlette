using UnityEngine;

public class DodgeState : GroundedState
{
    public DodgeState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Update();
        player.Anim.animator.CrossFade("Dodge Movement", 0.1f);
    }
    public override void Update()
    {
        base.Update();
        player.Dodge();
        if(!player.Movement.isDodging && player.TargetFndr.isLockedOn)
        {
            stateMachine.ChangeState(stateMachine.attackState);
            return;
        }
        if(!player.Movement.isDodging && !player.TargetFndr.isLockedOn)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
    }
}