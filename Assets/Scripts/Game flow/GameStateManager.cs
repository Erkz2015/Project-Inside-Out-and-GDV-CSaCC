using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; } = (GameState)(-1);

    private void Start()
    {
        SetState(GameState.StartScreen);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        Debug.Log($"State changed to {newState}");

        OnGameStateChanged?.Invoke(newState);
    }
}