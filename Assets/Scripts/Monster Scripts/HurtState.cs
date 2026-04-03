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

        if (monster.IsFinalHit())
        {
            monster.audioController.PlayDeath();
        }
        else
        {
            monster.audioController.PlayHurt();
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;

        monster.UpdatePosition();

        if (timer >= monster.hurtDuration)
        {
            monster.HandlePostHurt();

            if (!monster.IsFinalHit())
            {
                if (Random.value > 0.5f)
                    monster.ChangeState(monster.idleState);
                else
                    monster.ChangeState(monster.attackState);
            } else
            {
                monster.gameObject.SetActive(false);
            }
        }
    }

    public void Exit()
    {
        monster.audioController.StopHurt();
    }
}