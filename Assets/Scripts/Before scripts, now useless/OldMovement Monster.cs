using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public enum MonsterState
{
    Idle,
    Moving,
    Attacking,
    Hurt
}

public class MovementMonster : MonoBehaviour
{
    public Transform player;
    public float radius = 40f;

    public UnityEvent monsterDefeatTrigger;
    public UnityEvent playerHeartBeatTrigger;
    public UnityEvent monsterAttackingTrigger;

    [Header("----------- Movement -----------")]
    public float circlePercent = 0f;
    public float MinRotSpeed = 20f;
    public float MaxRotSpeed = 50f;
    float rotationSpeed;
    float startPercent;
    float targetPercent;
    float timer;
    float angle;

    [Header("----------- State -----------")]
    private MonsterState currentState = MonsterState.Idle;
    float idleDuration;
    float movingDuration;
    float attackDuration;
    float hurtDuration;

    [Header("----------- Fmod -----------")]
    public string runningSpeedParameter = "Running Speed";
    public StudioEventEmitter footstepsEmitter;
    public StudioEventEmitter scrapingEmitter;
    public StudioEventEmitter breathingEmmiter;
    public StudioEventEmitter screechEmmiter;
    public StudioEventEmitter hitEmmiter;
    public StudioEventEmitter missEmmiter;
    public StudioEventEmitter FinalHitEmmitter;
    private int hurtTimes = 0;
    private int hurtGoal = 3;
    bool movingIsPlaying;
    bool idleIsPlaying;
    bool attackingIsPlaying;
    bool hurtIsPlaying;
    bool hurt = false; 


    void Start()
    {
        angle = (circlePercent / 100f) * 360f;
        ResetTime();
    }

    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                IdleState();
                break;

            case MonsterState.Moving:
                MovingState();
                break;

            case MonsterState.Attacking:
                AttackingState();
                break;

            case MonsterState.Hurt:
                MonsterHurt();
                break;
        } 
    }

    void IdleState()
    {
        timer += Time.deltaTime;

        if (!idleIsPlaying)
        {
            breathingEmmiter.Play();
            idleIsPlaying = true;
        }

        if (timer >= idleDuration || hurt)
        {
            breathingEmmiter.Stop();
            idleIsPlaying = false;
            ResetTime();

            if (hurt) currentState = MonsterState.Hurt;
            else { currentState = Random.Range(0, 2) == 0 ? MonsterState.Attacking : MonsterState.Moving; }
        }
    }

    void MovingState()
    {
        float RotSpeed;

        timer += Time.deltaTime;

        if (!movingIsPlaying)
        {
            footstepsEmitter.Play();
            scrapingEmitter.Play();
            movingIsPlaying = true;
        }

        angle += rotationSpeed * Time.deltaTime;
        angle %= 360f;

        UpdatePosition();
        RotSpeed = UpdateWalkSoundSpeed();
        float r = Mathf.InverseLerp(MinRotSpeed, MaxRotSpeed, RotSpeed);
        float parameterValue = Mathf.Lerp(0f, 2f, r);

        footstepsEmitter.SetParameter(runningSpeedParameter, parameterValue);

        if (timer >= movingDuration || hurt)
        {
            footstepsEmitter.Stop();
            scrapingEmitter.Stop();
            movingIsPlaying = false;
            ResetTime();

            if (hurt) currentState = MonsterState.Hurt;
            else { currentState = Random.Range(0, 2) == 0 ? MonsterState.Idle : MonsterState.Attacking; }
        }
    }

    void AttackingState()
    {
        timer += Time.deltaTime;
        float t = timer / attackDuration;

        if (!attackingIsPlaying)
        {
            screechEmmiter.Play();
            monsterAttackingTrigger.Invoke();
            attackingIsPlaying = true;
        }

        float currentPercent = Mathf.Lerp(startPercent, targetPercent, t);
        angle = (currentPercent / 100f) * 360f;

        UpdatePosition();

        if (t >= 1f || hurt)
        {
            screechEmmiter.Stop();
            attackingIsPlaying = false;

            circlePercent = targetPercent % 100f;
            ResetTime();

            if (hurt) currentState = MonsterState.Hurt;
            else { currentState = Random.Range(0, 2) == 0 ? MonsterState.Idle : MonsterState.Moving; }
        }
    }

    public void HurtMonster()
    {
        if (!hurtIsPlaying)
        {
            hurt = true;
            hurtTimes++;
        Debug.Log("Monster hurt " + hurtTimes + " times.");
        }
    }

    public void MissedMonster()
    {
        missEmmiter.Play();
    }

    void MonsterHurt() {
        timer += Time.deltaTime;
        float t = timer / hurtDuration;

        if (!hurtIsPlaying && hurtTimes < hurtGoal)
        {
            hitEmmiter.Play();
            hurtIsPlaying = true;
        } else if (!hurtIsPlaying && hurtTimes >= hurtGoal)
        {
            FinalHitEmmitter.Play();    
            hurtIsPlaying = true;
        }

        float currentPercent = Mathf.Lerp(startPercent, targetPercent, t);
        angle = (currentPercent / 100f) * 360f;

        UpdatePosition();

        if (t >= 1f)
        {
            hurtIsPlaying = false;
            hurt = false;
            hitEmmiter.Stop();

            circlePercent = targetPercent % 100f;
            ResetTime();

            if (hurtTimes == 1)
            {
                playerHeartBeatTrigger.Invoke();
            } 
            else if (hurtTimes == 2)
            {
                playerHeartBeatTrigger.Invoke();
            }
            else if (hurtTimes >= hurtGoal)
            {
                monsterDefeatTrigger.Invoke();
            }

            currentState = Random.Range(0, 2) == 0 ? MonsterState.Idle : MonsterState.Moving;
        }
    }

    void UpdatePosition()
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        transform.position = player.position + new Vector3(x, 0f, z);
    }

    private void ResetTime() 
    {
        idleDuration = Random.Range(2f, 6f);
        movingDuration = Random.Range(3f, 5.5f);
        attackDuration = Random.Range(0.75f, 1.25f);
        hurtDuration = 3f;

        rotationSpeed = Random.Range(0, 2) == 0 ? Random.Range(MinRotSpeed, MaxRotSpeed) : -Random.Range(MinRotSpeed, MaxRotSpeed);

        int moveDistance = Random.Range(0, 2) == 0 ? Random.Range(25, 33) : -Random.Range(25, 33);

        startPercent = (angle / 360f) * 100f;
        targetPercent = startPercent + moveDistance;

        timer = 0f;
    }

    private float UpdateWalkSoundSpeed()
    {
        if (rotationSpeed < 0f)
        {
            return rotationSpeed * -1;
        } else
        {
            return rotationSpeed;
        }
    }
}