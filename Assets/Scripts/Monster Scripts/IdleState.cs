using UnityEngine;

public class IdleState : IState
{
    private IMonsterContext monster;
    private float timer;

    public IdleState(IMonsterContext monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        timer = 0f;
        monster.RandomizeIdleDuration();
        monster.Audio.PlayIdle();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= monster.IdleDuration)
        {
            if (Random.value > 0.5f)
                monster.ChangeState(new MovingState(monster));
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
        monster.Audio.StopIdle();
    }
}