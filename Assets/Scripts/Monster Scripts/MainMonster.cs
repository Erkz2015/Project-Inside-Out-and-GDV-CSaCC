using UnityEngine;
using System;

public class MainMonster : MonoBehaviour, IMonsterContext
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private MonsterAudio audioController;

    [Header("Movement")]
    [SerializeField] private float radius = 40f;

    [SerializeField] private float minRotSpeed = 20f;
    [SerializeField] private float maxRotSpeed = 50f;

    [Header("Durations")]
    public float idleDuration;
    public float movingDuration;
    public float attackDuration;
    public float hurtDuration = 3f;

    public static event Action OnMonsterDefeated;
    public static event Action OnMonsterHit;
    public static event Action OnMonsterAttack;

    private StateMachine stateMachine;

    public IdleState idleState;
    public MovingState movingState;
    public AttackState attackState;
    public HurtState hurtState;

    private float angle;
    private float rotationSpeed;

    private int hurtTimes = 0;
    private int hurtGoal = 4;
    private bool hurtPending;

    private void Awake()
    {
        stateMachine = new StateMachine();

        idleState = new IdleState(this);
        movingState = new MovingState(this);
        attackState = new AttackState(this);
        hurtState = new HurtState(this);
    }

    private void Start()
    {
        ChangeState(movingState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void OnEnable()
    {
        GunShot.leftmouseActionHit += HurtMonster;
        GunShot.leftmouseActionMis += MissMonster;
    }

    private void OnDisable()
    {
        GunShot.leftmouseActionHit -= HurtMonster;
        GunShot.leftmouseActionMis -= MissMonster;
    }

    // STATE MACHINE WRAPPER
    public void ChangeState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

    // POSITION
    public void UpdatePosition()
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        transform.position = player.position + new Vector3(x, 0f, z);
    }

    // RANDOMNESS
    public void RandomizeMovement()
    {
        movingDuration = UnityEngine.Random.Range(3f, 5.5f);
        rotationSpeed = UnityEngine.Random.Range(minRotSpeed, maxRotSpeed);

        if (UnityEngine.Random.value > 0.5f)
            rotationSpeed *= -1;
    }

    public void RandomizeIdleDuration()
    {
        idleDuration = UnityEngine.Random.Range(2f, 6f);
    }

    public void RandomizeAttack()
    {
        attackDuration = UnityEngine.Random.Range(0.75f, 1.25f);

        rotationSpeed = 180f / attackDuration;

        if (UnityEngine.Random.value > 0.5f)
            rotationSpeed *= -1;
    }

    // COMBAT
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

    public bool IsFinalHit()
    {
        return hurtTimes >= hurtGoal;
    }

    public void HandlePostHurt()
    {
        if (hurtTimes == hurtGoal / 4 ||
            hurtTimes == (hurtGoal / 4) * 2)
        {
            OnMonsterHit?.Invoke();
        }
        else if (hurtTimes >= hurtGoal)
        {
            OnMonsterDefeated?.Invoke();
        }
    }

    public void InvokeMonsterAttack()
    {
        OnMonsterAttack?.Invoke();
    }

    public void MissMonster()
    {
        audioController.PlayMiss();
    }

    // ===== IMonsterContext =====

    public float MinRotSpeed => minRotSpeed;
    public float MaxRotSpeed => maxRotSpeed;

    public float Angle
    {
        get => angle;
        set => angle = value;
    }

    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }

    public float IdleDuration
    {
        get => idleDuration;
        set => idleDuration = value;
    }

    public float MovingDuration
    {
        get => movingDuration;
        set => movingDuration = value;
    }

    public float AttackDuration
    {
        get => attackDuration;
        set => attackDuration = value;
    }

    public float HurtDuration => hurtDuration;

    public MonsterAudio Audio => audioController;

    public Transform Player => player;
}