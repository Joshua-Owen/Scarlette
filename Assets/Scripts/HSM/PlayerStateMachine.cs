using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState currentState;
    private PlayerController player;

    public IdleState idleState;
    public MoveState moveState;
    public GroundedState groundedState;
    public AirbourneState airbourneState;
    public FallState fallState;
    public JumpState jumpState;
    private void Awake()
    {
        player =  GetComponent<PlayerController>();

        idleState = new IdleState(this, player);
        moveState =  new MoveState(this, player);
        groundedState =  new GroundedState(this, player);
        airbourneState =  new AirbourneState(this, player);
        fallState = new FallState(this, player);
        jumpState = new JumpState(this, player);
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