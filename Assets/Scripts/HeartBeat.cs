using FMODUnity;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    float timer = 0f;

    public int heartBeatPhase;

    public float heartBeatInterval;

    public EventReference heartBeatOneShot;
    

    void Start()
    {
        heartBeatPhase = 0;
        resetTimer();
    }

    void OnEnable()
    {
        MainMonster.OnMonsterHit += nextHeartBeatPhase;
    }

    void OnDisable()
    {
        MainMonster.OnMonsterHit -= nextHeartBeatPhase;
    }

    void Update()
    {
        if (heartBeatPhase == 0)
        {
            return;
        }else
        {
            Beat();
        }
    }

    public void Beat()
    {
        timer += Time.deltaTime;

        if (timer >= heartBeatInterval) 
        {
            RuntimeManager.PlayOneShot(heartBeatOneShot);
            resetTimer();
        }
    }

    public void resetTimer()
    {
        timer = 0f;

        if (heartBeatPhase == 1) { heartBeatInterval = 1f; }
        if (heartBeatPhase == 2) { heartBeatInterval = 0.65f; } 
        if (heartBeatPhase == 3) { heartBeatInterval = 0.4f; }
    }

    public void nextHeartBeatPhase()
    {
        heartBeatPhase += 1;
        resetTimer();

    }

    public void resetHeartBeatPhase()
    {
        heartBeatPhase = 0;
        resetTimer();
    }
}
