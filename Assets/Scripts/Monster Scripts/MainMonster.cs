using UnityEngine;
using System;

public class MainMonster : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public MonsterAudio audioController;

    [Header("Movement")]
    public float radius = 40f;
    public float MinRotSpeed = 20f;
    public float MaxRotSpeed = 50f;

    [Header("Durations")]
    public float idleDuration;
    public float movingDuration;
    public float attackDuration;
    public float hurtDuration = 3f;

    public static event Action OnMonsterDefeated;
    public static event Action OnMonsterHit;
    public static event Action OnMonsterAttack;

    [HideInInspector] public IdleState idleState;
    [HideInInspector] public MovingState movingState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public HurtState hurtState;

    private IMonsterState currentState;

    [HideInInspector] public float angle;
    [HideInInspector] public float rotationSpeed;

    private int hurtTimes = 0;
    private int hurtGoal = 6;//alleen stapjes van 3 graag!

    private bool hurtPending = false;

    void Awake()
    {
        idleState = new IdleState(this);
        movingState = new MovingState(this);
        attackState = new AttackState(this);
        hurtState = new HurtState(this);
    }

    void Start()
    {
        ChangeState(movingState);
    }

    void OnEnable()
    {
        GunShot.leftmouseActionHit += HurtMonster;
        GunShot.leftmouseActionMis += MissMonster;
    }

    void OnDisable()
    {
        GunShot.leftmouseActionHit -= HurtMonster;
        GunShot.leftmouseActionMis -= MissMonster;
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IMonsterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdatePosition()
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        transform.position = player.position + new Vector3(x, 0f, z);
    }

    public void RandomizeMovement()
    {
        movingDuration = UnityEngine.Random.Range(3f, 5.5f);

        rotationSpeed = UnityEngine.Random.Range(MinRotSpeed, MaxRotSpeed);
        if (UnityEngine.Random.value > 0.5f) rotationSpeed *= -1;
    }

    public void RandomizeIdleDuration()
    {
        idleDuration = UnityEngine.Random.Range(2f, 6f);
    }

    public void RandomizeAttack()
    {
        attackDuration = UnityEngine.Random.Range(0.75f, 1.25f);

        rotationSpeed = 180f / attackDuration;
        if (UnityEngine.Random.value > 0.5f) rotationSpeed *= -1;
    }

    public void MissMonster()
    {
        audioController.PlayMiss();
    }

    public void HandlePostHurt()
    {
        if (hurtTimes == hurtGoal / 3 || hurtTimes == (hurtGoal / 3) * 2)
        {
            OnMonsterHit?.Invoke();
        }
        else if (hurtTimes >= hurtGoal)
        {
            OnMonsterDefeated?.Invoke();
        }
    }

    public bool IsFinalHit()
    {
        return hurtTimes >= hurtGoal;
    }

    public void HurtMonster()
    {
        hurtTimes++;
        hurtPending = true;
    }

    public bool IsHurtPending()
    {
        if (hurtPending)
        {
            hurtPending = false;
            return true;
        }
        return false;
    }

    public void InvokeMonsterAttack()
    {
        OnMonsterAttack?.Invoke();
    }
}