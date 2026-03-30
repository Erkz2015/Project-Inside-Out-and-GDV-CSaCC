using UnityEngine;

public class AttackState : IMonsterState
{
    private MainMonster monster;
    private float timer;

    public AttackState(MainMonster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        timer = 0f;
        monster.RandomizeAttack();
        monster.audioController.PlayAttack();
        monster.InvokeMonsterAttack();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        monster.angle += monster.rotationSpeed * Time.deltaTime;
        monster.UpdatePosition();

        if (timer >= monster.attackDuration)
        {
            if (Random.value > 0.5f)
                monster.ChangeState(monster.idleState);
            else
                monster.ChangeState(monster.movingState);
        }

        if (monster.IsHurtPending())
        {
            monster.ChangeState(monster.hurtState);
            return;
        }
    }

    public void Exit()
    {
        monster.audioController.StopAttack();
    }
}