using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    private bool isPlayerTurn = true;
    private EnemyTurnManager enemyTurnManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[TurnManager] Initialized.");
        }
        else
        {
            Destroy(gameObject);
        }

        enemyTurnManager = GetComponent<EnemyTurnManager>();
        if (enemyTurnManager == null)
        {
            Debug.LogError("[TurnManager] EnemyTurnManager not found! Please attach the EnemyTurnManager script.");
        }
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public void EndPlayerTurn()
    {
        if (isPlayerTurn)
        {
            Debug.Log("[TurnManager] Ending player turn.");
            isPlayerTurn = false;
            Debug.Log("[TurnManager] Switching to enemy turn...");
            enemyTurnManager.StartEnemyTurn();
        }
    }

    public void StartPlayerTurn()
    {
        Debug.Log("[TurnManager] Starting player turn.");
        isPlayerTurn = true;
    }
}
