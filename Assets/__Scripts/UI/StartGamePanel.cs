using System;
using UnityEngine;

public class StartGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private bool _showed;

    public static event Action<GameState> OnStartGame;

    private void OnEnable()
    {
        GameStateManager.OnStateChanged += CheckState;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChanged -= CheckState;
    }

    private void Update()
    {
        if (_showed == true)
            return;

        if (Input.GetKeyDown(KeyCode.F) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            StartGame();
    }

    private void CheckState(GameState state)
    {
        if (state == GameState.Intro)
            ActivatePanel();
    }

    private void ActivatePanel() => _panel.SetActive(true);

    private void StartGame()
    {
        _panel.SetActive(false);

        OnStartGame?.Invoke(GameState.Start);

        _showed = true;
    }
}