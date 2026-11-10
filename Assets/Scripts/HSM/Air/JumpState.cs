using UnityEngine;

public class JumpState : AirbourneState
{
    public JumpState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Anim.animator.CrossFade("Jump", 0f);
        player.Movement.HandleJump();
    }
    public override void Update()
    {

        base.Update();
        
        if(player.isGrounded && player.Movement.velocity.y < 0)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            
        }
        if(player.isGrounded && player.Movement.velocity.y < 0 && player.TargetFndr.isLockedOn)
        {
            stateMachine.ChangeState(stateMachine.attackState);
            
        }
    }
}