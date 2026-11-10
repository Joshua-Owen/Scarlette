
using JetBrains.Annotations;
using Unity.Mathematics;

public class HackState : AttackState
{
    public HackState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }
    public override void Enter()
    {
        player.Hack.HackUI.SetActive(true);
    }
    public override void Update()
    {
        player.Move();
        player.Movement.HandleRotation();
        player.Hack.HackToggle();
        if(!player.Hack.isHacking)
        {
            player.Hack.HackUI.SetActive(false);
            stateMachine.ChangeState(stateMachine.attackState);
            return;
        }
    }
}