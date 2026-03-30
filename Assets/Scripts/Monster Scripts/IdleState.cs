using UnityEngine;

public class IdleState : IMonsterState
{
    private MainMonster monster;
    private float timer;

    public IdleState(MainMonster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        timer = 0f;
        monster.RandomizeIdleDuration();
        monster.audioController.PlayIdle();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= monster.idleDuration)
        {
            if (Random.value > 0.5f)
                monster.ChangeState(monster.movingState);
            else
                monster.ChangeState(monster.attackState);
        }

        if (monster.IsHurtPending())
        {
            monster.ChangeState(monster.hurtState);
            return;
        }
    }

    public void Exit()
    {
        monster.audioController.StopIdle();
    }
}