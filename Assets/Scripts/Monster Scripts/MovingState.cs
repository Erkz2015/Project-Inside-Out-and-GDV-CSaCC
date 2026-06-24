using UnityEngine;

public class MovingState : IState
{
    private IMonsterContext monster;
    private float timer;

    public MovingState(IMonsterContext monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        timer = 0f;
        monster.RandomizeMovement();

        float r = Mathf.InverseLerp(monster.MinRotSpeed, monster.MaxRotSpeed, monster.RotationSpeed);
        float parameterValue = Mathf.Lerp(0f, 2f, r);

        monster.Audio.PlayMovement(parameterValue);
    }

    public void Update()
    {
        timer += Time.deltaTime;

        monster.Angle += monster.RotationSpeed * Time.deltaTime;
        monster.UpdatePosition();

        if (timer >= monster.MovingDuration)
        {
            if (Random.value > 0.5f)
                monster.ChangeState(new IdleState(monster));
            else
                monster.ChangeState(new AttackState(monster));
        }

        if (monster.IsHurtPending())
        {
            monster.ChangeState(new HurtState(monster));
        }
    }

    public void Exit()
    {
        monster.Audio.StopMovement();
    }
}