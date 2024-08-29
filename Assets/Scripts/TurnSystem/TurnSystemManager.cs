using System;
using UnityEngine;

public class TurnSystemManager : MonoBehaviour
{
    public static TurnSystemManager Instance;

    //#TODO: currently not in use, used when enemy has their own turn
    private Player HumanPlayer { get => GameManager.Instance.humanPlayer; }
    private Player AiPlayer { get => GameManager.Instance.computerPlayer; }

    //Current values of the game.
    [HideInInspector]
    public Player currentPlayer;
    public Turn currentTurn;

    //declaration of possible turns
    PlayerTurn playerTurn = new PlayerTurn();

    //declaration of events
    public event Action<TurnSystemManager, Turn, Turn> OnChangedTurnEvent;

    //flag
    private bool isTurnHasCompletedEventBinded = false;
    private bool isAllDataLoadedEventBinded = false;

    private void OnEnable()
    {
        if (!isTurnHasCompletedEventBinded)
        {
            BindTurnHasCompletedEvent();
        }

        if (!isAllDataLoadedEventBinded)
        {
            BindAllDataLoadedEvent();
        }
    }

    private void OnDisable()
    {
        if (isTurnHasCompletedEventBinded)
        {
            UnbindStartNewTurnEvent();
        }

        if (isAllDataLoadedEventBinded)
        {
            UnbindAllDataLoadedEvent();
        }
    }

    private void Awake()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        //To avoid racing condition
        if (!isTurnHasCompletedEventBinded)
        {
            BindTurnHasCompletedEvent();
        }

        if (!isAllDataLoadedEventBinded)
        {
            BindAllDataLoadedEvent();
        }

        currentPlayer = HumanPlayer;
    }

    void Update()
    {
        if (currentTurn != null)
        {
            currentTurn.OnUpdateTurn(currentPlayer);
        }
    }

    [ContextMenu("TurnSystemManager/StartRound")]
    public void HandleTurnHasCompleted()
    {
        ChangeTurn(playerTurn);
    }

    public void ChangeTurn(Turn newTurn)
    {
        if (currentPlayer == null)
        {
            Debug.LogError("current player is not assigned");
            return;
        }

        if (currentTurn != null)
        {
            currentTurn.OnEndTurn(currentPlayer);
        }

        //#TODO: Should Swap Player?

        //broadcast event, primarily binded to PlayerHandUIManager
        OnChangedTurnEvent?.Invoke(this, currentTurn, newTurn);
        currentTurn = newTurn;

        currentTurn.OnStartTurn(this, currentPlayer);
    }

    #region Bind StartNewTurnEvent

    private void BindTurnHasCompletedEvent()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTurnCompleted += HandleTurnHasCompleted;
            isTurnHasCompletedEventBinded = true;
        }
    }

    private void UnbindStartNewTurnEvent()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTurnCompleted -= HandleTurnHasCompleted;
            isTurnHasCompletedEventBinded = false;
        }
    }
    #endregion

    #region BindAllDataLoadedEvent
    private void BindAllDataLoadedEvent()
    {
        if (SaveSystemManager.Instance != null)
        {
            SaveSystemManager.Instance.OnAllSaveDataLoaded += HandleTurnHasCompleted;
            isAllDataLoadedEventBinded = true;
        }
    }
    private void UnbindAllDataLoadedEvent()
    {
        if (SaveSystemManager.Instance != null)
        {
            SaveSystemManager.Instance.OnAllSaveDataLoaded -= HandleTurnHasCompleted;
            isAllDataLoadedEventBinded = false;
        }
    }
    #endregion
}
