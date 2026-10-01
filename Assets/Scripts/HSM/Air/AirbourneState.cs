using UnityEngine;

public class AirbourneState : PlayerState
{
    public AirbourneState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered Grounded");
    }

    public override void Update()
    {
        player.Movement.HandleMovementSetUp();
        player.Movement.HandleRotation();
        player.Movement.HandleAirGravity();
        player.Movement.HandleAirbourneMovement();
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}