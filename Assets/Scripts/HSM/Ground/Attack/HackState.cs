
public class HackState : GroundedState
{
    public HackState(PlayerStateMachine stateMachine, PlayerController player) : base(stateMachine, player)
    {
    }
    public override void Enter()
    {
        player.HackController.hasMatch = false;
        player.TargetFndr.canSwitchTargets = false;
        player.HackController.SetCurrentTarget();
        player.Hack.ShowComboListUI();

        player.Hack.HackUI.SetActive(true);

        //player.HackController.hasTimedOut = false;
        //player.Hack.ShowComboList();
        player.HackController.inputTimer = player.HackController.inputTimeLimit;
        player.HackController.timerBar.value = player.HackController.inputTimeLimit;
    }
    public override void Update()
    {
        player.HackController.StartHackingTimer();
        //player.HackController.listen();
        //player.HackController.UpdateInputTimer();
        player.Move();
        player.Movement.HandleRotation();
        player.Hack.HandleHackValidation();
        //player.Hack.HackToggle();
        //if (!player.Hack.isShowingResult)
        //{
            if(player.Inputs.HackReleased || !player.TargetFndr.isLockedOn )
            {
            //player.HackController.MatchPair();
            /*if(!player.Hack.isHacking)
            {*/
            if (!player.Hack.isShowingResult)
            {
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }
            //}
            }
            if(player.Inputs.HackReleased && player.isMoving  || !player.TargetFndr.isLockedOn && player.isMoving )
            {
            //player.HackController.MatchPair();
            //if(!player.Hack.isHacking)
            //{
            if (!player.Hack.isShowingResult)
            {
                stateMachine.ChangeState(stateMachine.moveState);
                return;
            }   
            //}
            }
        //}
        
    }
    public override void Exit()
    {
        //if()
        //{
            player.Hack.HackUI.SetActive(false);
            //player.Hack.RemoveComboList();
            //player.HackController.ResetInput();
            player.HackController.ClearSequence();
            player.Hack.ClearSequenceUI(player.Hack.sequenceContainer);
            player.Hack.ClearSequenceUI(player.Hack.comboListContainer);
            player.HackController.StopHackingTimer();
            player.TargetFndr.canSwitchTargets = true;

        //}
        
    }
}