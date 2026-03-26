using UnityEngine;

public class HurtState : IMonsterState
{
    private MainMonster monster;
    private float timer;

    public HurtState(MainMonster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        timer = 0f;
        monster.audioController.PlayHurt();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        monster.UpdatePosition();

        if (timer >= monster.hurtDuration)
        {
            monster.ChangeState(new IdleState(monster));
        }
    }

    public void Exit()
    {
        monster.audioController.StopHurt();
    }
}