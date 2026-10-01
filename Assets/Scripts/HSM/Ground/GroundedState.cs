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
        
        
        
        
        player.Movement.HandleGroundGravity();
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}