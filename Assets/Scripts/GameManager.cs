using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChange;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.InGame);

        if (PlayerPrefs.GetInt("LoadSave") == 1)
        {
            Player.Instance.LoadPlayer();
            PlayerPrefs.DeleteKey("LoadSave");
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.InInitialScreen:
                break;
            case GameState.NpcDialogue:
                HandleNPCDialogue();
                break;
            case GameState.InGame:
                break;
            case GameState.PauseMenu:
                break;
            case GameState.InHabilitieMenu:
                break;
            case GameState.Death:
                break;
            case GameState.EndGame:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }

    private void HandleNPCDialogue()
    {
        
    }
}

public enum GameState
{
    InInitialScreen,
    NpcDialogue,
    InGame,
    PauseMenu,
    InHabilitieMenu,
    Death, 
    EndGame
}