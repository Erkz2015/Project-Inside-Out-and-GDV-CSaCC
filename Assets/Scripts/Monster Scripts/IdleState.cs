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
                monster.ChangeState(new MovingState(monster));
            else
                monster.ChangeState(new AttackState(monster));
        }
    }

    public void Exit()
    {
        monster.audioController.StopIdle();
    }
}