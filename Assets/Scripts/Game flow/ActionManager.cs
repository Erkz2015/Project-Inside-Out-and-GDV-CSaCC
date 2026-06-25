using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class ActionManager : MonoBehaviour
{
    [Header("State Manager")]
    [SerializeField] private GameStateManager stateManager;

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
    public GameObject dust;
    public GameObject playerLightOne;
    public GameObject playerLightTwo;

    [Header("Scripts")]
    public PlayerMovement playerMovement;
    public GunShotAnimation gunShotAnimation;
    public GunShot gunShot;

    [Header("Sound Emitters")]
    public StudioEventEmitter menuMusicEmitter;
    public StudioEventEmitter happySoundEmitter;
    public StudioEventEmitter unnervingMusicEmitter;
    public StudioEventEmitter transitionToMonsterEmitter;
    public StudioEventEmitter slowBreathingEmitter;
    public StudioEventEmitter fastBreathingEmitter;

    public UnityEvent MouseInvisSwitch;

    private float timer;
    private bool monsterIsTransitioning;

    private void OnEnable()
    {
        GameStateManager.OnGameStateChanged += HandleStateChange;
        MainMonster.OnMonsterDefeated += EndScreenEvent;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameStateChanged -= HandleStateChange;
        MainMonster.OnMonsterDefeated -= EndScreenEvent;
    }

    private void Update()
    {
        if (!monsterIsTransitioning)
            return;

        timer += Time.deltaTime;

        if (timer >= 25f)
        {
            TeleportMonster();
            monsterIsTransitioning = false;
            timer = 0f;
        }
    }

    private void HandleStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.StartScreen:
                StartScreen();
                break;

            case GameState.Playing:
                StartGame();
                break;

            case GameState.TransitionToMonster:
                StartMonsterTransition();
                break;

            case GameState.EndScreen:
                EndScreen();
                break;

            case GameState.GameOver:
                GameOverScreen();
                break;
        }
    }

    public void StartButtonPressed()
    {
        stateManager.SetState(GameState.Playing);
    }

    public void TriggerMonsterTransition()
    {
        stateManager.SetState(GameState.TransitionToMonster);
    }

    private void EndScreenEvent()
    {
        stateManager.SetState(GameState.EndScreen);
    }

    public void TriggerGameOver()
    {
        stateManager.SetState(GameState.GameOver);
    }

    private void StartScreen()
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

        dust.SetActive(true);

        playerMovement.enabled = false;
        gunShotAnimation.enabled = false;
        gunShot.enabled = false;

        menuMusicEmitter.Play();
    }

    private void EndScreen()
    {
        weapon.SetActive(false);
        monster.SetActive(false);

        menu.SetActive(true);

        TitleText.SetActive(false);
        StartButton.SetActive(false);

        EndText.SetActive(true);
        GameOverText.SetActive(false);

        playerMovement.enabled = false;

        MouseInvisSwitch.Invoke();

        gunShotAnimation.enabled = false;
        gunShot.enabled = false;

        fastBreathingEmitter.Stop();
    }

    private void GameOverScreen()
    {
        weapon.SetActive(false);
        monster.SetActive(false);

        menu.SetActive(true);

        TitleText.SetActive(false);
        StartButton.SetActive(false);

        EndText.SetActive(false);
        GameOverText.SetActive(true);

        playerMovement.enabled = false;

        MouseInvisSwitch.Invoke();

        gunShotAnimation.enabled = false;
        gunShot.enabled = false;

        fastBreathingEmitter.Stop();
    }

    private void StartGame()
    {
        flashlight.SetActive(true);
        building.SetActive(true);

        menu.SetActive(false);
        dust.SetActive(false);

        playerMovement.enabled = true;

        MouseInvisSwitch.Invoke();

        menuMusicEmitter.Stop();

        happySoundEmitter.Play();
        slowBreathingEmitter.Play();
    }

    private void StartMonsterTransition()
    {
        monsterIsTransitioning = true;

        playerMovement.enabled = false;

        transitionToMonsterEmitter.Play();

        happySoundEmitter.Stop();
        slowBreathingEmitter.Stop();

        playerLightOne.SetActive(false);
        playerLightTwo.SetActive(false);

        flashlight.SetActive(false);
        building.SetActive(false);
    }

    private void TeleportMonster()
    {
        dust.SetActive(true);

        monster.SetActive(true);
        weapon.SetActive(true);

        playerLightOne.SetActive(true);
        playerLightTwo.SetActive(true);

        gunShotAnimation.enabled = true;
        gunShot.enabled = true;

        playerMovement.enabled = true;

        unnervingMusicEmitter.Play();
        fastBreathingEmitter.Play();
    }
}