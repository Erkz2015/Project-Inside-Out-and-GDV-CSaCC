using UnityEngine;

public class MovingState : IMonsterState
{
    private MainMonster monster;
    private float timer;

    public MovingState(MainMonster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        timer = 0f;
        monster.RandomizeMovement();

        // De variable van het geluid isntellen op basis van de rotatie/loop snelheid van het monster
        float r = Mathf.InverseLerp(monster.MinRotSpeed, monster.MaxRotSpeed, monster.rotationSpeed);
        float parameterValue = Mathf.Lerp(0f, 2f, r);
        monster.audioController.PlayMovement(parameterValue);
    }

    public void Update()
    {
        timer += Time.deltaTime;

        monster.angle += monster.rotationSpeed * Time.deltaTime;
        monster.UpdatePosition();

        if (timer >= monster.movingDuration)
        {
            if (Random.value > 0.5f)
                monster.ChangeState(monster.idleState);
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
        monster.audioController.StopMovement();
    }
}