using UnityEngine;

public class HurtState : IState
{
    private IMonsterContext monster;
    private float timer;

    public HurtState(IMonsterContext monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        timer = 0f;

        if (monster.IsFinalHit())
            monster.Audio.PlayDeath();
        else
            monster.Audio.PlayHurt();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        monster.UpdatePosition();

        if (timer >= monster.HurtDuration)
        {
            monster.HandlePostHurt();

            if (!monster.IsFinalHit())
            {
                if (Random.value > 0.5f)
                    monster.ChangeState(new IdleState(monster));
                else
                    monster.ChangeState(new AttackState(monster));
            }
            else
            {
                ((MainMonster)monster).gameObject.SetActive(false);
            }
        }
    }

    public void Exit()
    {
        monster.Audio.StopHurt();
    }
}