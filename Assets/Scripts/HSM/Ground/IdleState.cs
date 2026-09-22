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
    }

    public override void Exit()
    {
        base.Exit();
    }
}