using UnityEngine;

public class SlopeState : GroundedState
{
    public SlopeState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }

    public override void Update()
    {
        base.Update();
        player.Movement.HandleMovementSetUp();
        player.Movement.HandleSlopeSlide();
        if (!player.isOnSlope)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
    }
}