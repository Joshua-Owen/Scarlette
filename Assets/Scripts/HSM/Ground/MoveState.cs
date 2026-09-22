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

        //Debug.Log("Updating Moving");
        if (player.Movement.currentSpeed == 0)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
        if (player.isOnSlope)
        {
            
        }
    }


    public override void Exit()
    {
        base.Exit();
    }
}