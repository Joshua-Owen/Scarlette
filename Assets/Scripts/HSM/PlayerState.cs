using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

// abstract makes a base template
public abstract class PlayerState
{
    //Every state needs access to the state machine and the player.    
    protected readonly PlayerStateMachine stateMachine;
    protected readonly PlayerController player;

    protected PlayerState(PlayerStateMachine stateMachine, PlayerController player)
    {
        this.stateMachine = stateMachine;
        this.player = player;
    }
    // Every class that inherits from this gets these methods,
    // but can override them with its own behaviour.    
    public virtual void Enter(){}

    public virtual void Update(){}

    public virtual void Exit(){}
}

