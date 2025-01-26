using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public EntityStats stats { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    #endregion

    protected virtual void Awake() 
    {
    
    }
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<EntityStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnDestroy()
    {
        
    }

    #region Velocity
    public void SetZeroVelocity() 
    {
        if (stats.isKnocked)
            return;

        rb.velocity = Vector3.zero;
        stats.rollSpeed.SetValue(stats.moveSpeed.GetValue());
    }

    public void SetMoveVelocity(float _x,float _y) 
    {
        if (stats.isKnocked)
            return;

        Vector3 vector = (transform.right * _x + transform.up * _y).normalized;

        rb.velocity = vector * stats.moveSpeed.GetValue();
        stats.rollSpeed.SetValue(stats.moveSpeed.GetValue());
    }

    public void SetRunVelocity(float _x, float _y) 
    {
        if (stats.isKnocked)
            return;

        Vector3 vector = (transform.right * _x + transform.up * _y).normalized;
        
        rb.velocity = vector * stats.runSpeed.GetValue();
        stats.rollSpeed.SetValue(stats.runSpeed.GetValue());
    }

    public void SetRollVelocity()
    {
        if (stats.isKnocked)
            return;

        rb.velocity = stats.currentVector * stats.rollSpeed.GetValue();
    }

    public void AdjustCurrentVector(float _x, float _y)
    {
        Vector3 vector = (transform.right * _x + transform.up * _y);

        if (vector != Vector3.zero)
            stats.currentVector = vector.normalized;
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quaternion rotation = Quaternion.Euler(0, 0, -45);

            Vector3 adjustedVector = rotation * stats.currentVector;
            stats.currentVector = adjustedVector.normalized;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 45);

            Vector3 adjustedVector = rotation * stats.currentVector;
            stats.currentVector = adjustedVector.normalized;
        }
    }
    #endregion

    #region Sprite Effect
    public void SetTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
    #endregion

    #region Die
    public virtual void Die()
    {
    }
    #endregion
}
