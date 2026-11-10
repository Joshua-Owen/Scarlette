using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState;
    private PlayerController player;

    public IdleState idleState;
    public MoveState moveState;
    public GroundedState groundedState;
    public AirbourneState airbourneState;
    public FallState fallState;
    public JumpState jumpState;
    public SlopeState slopeState;
    public AttackState attackState;
    public DodgeState dodgeState;
    public HackState hackState;
    private void Awake()
    {
        player =  GetComponent<PlayerController>();

        idleState = new IdleState(this, player);
        moveState =  new MoveState(this, player);
        groundedState =  new GroundedState(this, player);
        airbourneState =  new AirbourneState(this, player);
        fallState = new FallState(this, player);
        jumpState = new JumpState(this, player);
        slopeState = new SlopeState(this, player);
        attackState = new AttackState(this, player);
        dodgeState = new DodgeState(this, player);
        hackState = new HackState(this, player);
    }

    private void Start()
    {
        ChangeState(idleState);
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(PlayerState nextState)
    {
        currentState?.Exit();

        currentState = nextState;

        currentState.Enter();
        Debug.Log($"Current State : {currentState}");
    }
}

