using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ActionManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject weapon;
    public GameObject flashlight;
    public GameObject building;
    public GameObject monster;
    public GameObject menu;
    public GameObject TitleText;
    public GameObject EndText;
    public GameObject GameOverText;
    public GameObject StartButton;
    //public GameObject ResetButton;
    public GameObject dust;
    public GameObject playerLightOne;
    public GameObject playerLightTwo;
    [Header("Scripts")]
    public PlayerMovement playerMovement;
    public GunShotAnimation gunShotAnimation;
    public GunShot gunShot;
    [Header("SoundEmmitters")]
    public StudioEventEmitter menuMusicEmitter;
    public StudioEventEmitter happySoundEmitter;
    public StudioEventEmitter UnnervingMusicEmmiter;
    public StudioEventEmitter TransitionToMonsterEmmiter;
    public StudioEventEmitter SlowBreathinhgEmmiter;
    public StudioEventEmitter FastBreathingEmmiter;

    public UnityEvent MouseInvisSwitch;

    float timer = 0f;
    bool monsterIsTransitioning = false;

    void Start()
    {
        StartScreen();
    }

    void OnEnable()
    {
      MainMonster.OnMonsterDefeated += EndScreen;
    }

    void OnDisable()
    {
        MainMonster.OnMonsterDefeated -= EndScreen;
    }

    void Update()
    {

        if(monsterIsTransitioning)
        {
            timer += Time.deltaTime;
            if (timer >= 25f)
            {
                TeleportMonster();
                monsterIsTransitioning = false;
                timer = 0f;
            }

        }
    }

    public void StartScreen()
    {
        weapon.SetActive(false);
        flashlight.SetActive(false);
        building.SetActive(false);
        monster.SetActive(false);
        menu.SetActive(true);
        EndText.SetActive(false);
        GameOverText.SetActive(false);
        TitleText.SetActive(true);
        StartButton.SetActive(true);
        //ResetButton.SetActive(false);
        dust.SetActive(true);

        playerMovement.enabled = false;
        gunShotAnimation.enabled = false;
        gunShot.enabled = false;

        menuMusicEmitter.Play();
    }

    public void EndScreen()
    {
        weapon.SetActive(false);
        monster.SetActive(false);
        menu.SetActive(true);
        TitleText.SetActive(false);
        StartButton.SetActive(false);
        //ResetButton.SetActive(true);
        EndText.SetActive(true);
        GameOverText.SetActive(false);

        playerMovement.enabled = false;
        MouseInvisSwitch.Invoke();
        gunShotAnimation.enabled = false;
        gunShot.enabled = false;

        FastBreathingEmmiter.Stop();
    }

    public void GameOverScreen()
    {
        weapon.SetActive(false);
        monster.SetActive(false);
        menu.SetActive(true);
        TitleText.SetActive(false);
        StartButton.SetActive(false);
        //ResetButton.SetActive(true);
        GameOverText.SetActive(false);
        GameOverText.SetActive(true);

        playerMovement.enabled = false; 
        MouseInvisSwitch.Invoke();
        gunShotAnimation.enabled = false;
        gunShot.enabled = false;

        FastBreathingEmmiter.Stop();
    }

    public void StartGame()
    {
        flashlight.SetActive(true);
        building.SetActive(true);
        menu.SetActive(false);
        dust.SetActive(false);

        playerMovement.enabled = true;
        MouseInvisSwitch.Invoke();

        menuMusicEmitter.Stop();
        happySoundEmitter.Play();
        SlowBreathinhgEmmiter.Play();
    }

    public void StartMonsterTransition()
    {
        monsterIsTransitioning = true;

        playerMovement.enabled = false;

        TransitionToMonsterEmmiter.Play();
        happySoundEmitter.Stop();
        SlowBreathinhgEmmiter.Stop();

        playerLightOne.SetActive(false);
        playerLightTwo.SetActive(false);
        flashlight.SetActive(false);
        building.SetActive(false);

    }

    public void TeleportMonster()
    {
        dust.SetActive(true);
        monster.SetActive(true);
        weapon.SetActive(true);
        playerLightOne.SetActive(true);
        playerLightTwo.SetActive(true); 

        gunShotAnimation.enabled = true;
        gunShot.enabled = true;
        playerMovement.enabled = true;

        UnnervingMusicEmmiter.Play();
        FastBreathingEmmiter.Play();
    }
}
