using UnityEngine;
using System;

public class GameStateManager : Singleton<GameStateManager>
{
    private GameState _currentState;
    public GameState CurrentState => _currentState;

    public static event Action<GameState> OnStateChanged;
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameEnded;
    public static event Action OnGameOver;

    private void Start()
    {
        ChangeState(GameState.Intro);
    }

    private void OnEnable()
    {
        StartGamePanel.OnStartGame += ChangeState;
        EnemySpawner.OnAllWavesEnded += () => ChangeState(GameState.End);
        Player.OnPlayerDied += () => ChangeState(GameState.Over);
    }

    private void OnDisable()
    {
        StartGamePanel.OnStartGame -= ChangeState;
        EnemySpawner.OnAllWavesEnded -= () => ChangeState(GameState.End);
        Player.OnPlayerDied -= () => ChangeState(GameState.Over);
    }

    private void ChangeState(GameState newState)
    {
        _currentState = newState;
        print("Game state changed to " + _currentState.ToString());

        OnStateChanged?.Invoke(_currentState);

        switch (_currentState)
        {
            case GameState.Start:
                OnGameStarted?.Invoke();
                ChangeState(GameState.Playing);
                break;
            case GameState.Over:
                OnGameOver?.Invoke();
                break;
            case GameState.End:
                OnGameEnded?.Invoke();
                break;
            case GameState.Paused:
                OnGamePaused?.Invoke();
                break;
        }
    }
}