using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    protected Vector2 input;
    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _playerStateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        InputUpdate();

        RollingLogic();
    }

    private void InputUpdate()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        input = new Vector2 (xInput, yInput).normalized;

        if (input != Vector2.zero)
        {
            player.anim.SetFloat("xInput", xInput);
            player.anim.SetFloat("yInput", yInput);

        }

        player.AdjustCurrentVector(input.x, input.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public virtual void RollingLogic() 
    {
        if (!player.stats.isBusy && Input.GetKeyDown(KeyCode.Space)) 
            player.stateMachine.ChangeState(player.rollState);
    }


}
