using UnityEngine;

public interface IMonsterContext
{
    // movement
    float Angle { get; set; }
    float RotationSpeed { get; set; }
    float MinRotSpeed { get; }
    float MaxRotSpeed { get; }
    void UpdatePosition();

    // state switching
    void ChangeState(IState newState);

    // randomness
    void RandomizeIdleDuration();
    void RandomizeMovement();
    void RandomizeAttack();

    // timers / values
    float IdleDuration { get; set; }
    float MovingDuration { get; set; }
    float AttackDuration { get; set; }
    float HurtDuration { get; }

    // combat
    void HurtMonster();
    bool IsHurtPending();
    bool IsFinalHit();
    void HandlePostHurt();
    void InvokeMonsterAttack();

    // audio
    MonsterAudio Audio { get; }

    // player reference indirect (alleen nodig als states het gebruiken)
    Transform Player { get; }
}