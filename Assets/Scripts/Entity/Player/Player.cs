using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerRollState rollState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerRunState runState { get; private set; }
    #endregion
    protected override void Awake() 
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        rollState = new PlayerRollState(this, stateMachine, "Roll");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        runState  = new PlayerRunState(this, stateMachine, "Run");
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
