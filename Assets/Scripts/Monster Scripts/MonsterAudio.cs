using FMODUnity;
using UnityEngine;

public class MonsterAudio : MonoBehaviour
{
    public StudioEventEmitter footsteps;
    public StudioEventEmitter scraping;
    public StudioEventEmitter breathing;
    public StudioEventEmitter attack;
    public StudioEventEmitter hurt;
    public StudioEventEmitter monsterDeath;
    public StudioEventEmitter miss;

    public void PlayMiss() => miss.Play();

    public void PlayIdle() => breathing.Play();
    public void StopIdle() => breathing.Stop();

    public void PlayMovement(float WalkingSpeedParameter)
    {
        footsteps.Play();
        footsteps.SetParameter("Running Speed", WalkingSpeedParameter);
        scraping.Play();
    }

    public void StopMovement()
    {
        footsteps.Stop();
        scraping.Stop();
    }

    public void PlayAttack() => attack.Play();
    public void StopAttack() => attack.Stop();

    public void PlayHurt() => hurt.Play();
    public void StopHurt() => hurt.Stop();

    public void PlayDeath() => monsterDeath.Play();
}