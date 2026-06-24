using UnityEngine;

public class AttackState : IState
{
    private IMonsterContext monster;
    private float timer;

    public AttackState(IMonsterContext monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        timer = 0f;
        monster.RandomizeAttack();
        monster.Audio.PlayAttack();
        monster.InvokeMonsterAttack();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        monster.Angle += monster.RotationSpeed * Time.deltaTime;
        monster.UpdatePosition();

        if (timer >= monster.AttackDuration)
        {
            if (Random.value > 0.5f)
                monster.ChangeState(new IdleState(monster));
            else
                monster.ChangeState(new MovingState(monster));
        }

        if (monster.IsHurtPending())
        {
            monster.ChangeState(new HurtState(monster));
        }
    }

    public void Exit()
    {
        monster.Audio.StopAttack();
    }
}