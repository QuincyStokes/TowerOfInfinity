using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public static EnemyTurnManager Instance { get; private set; }

    private List<EnemyMovementTest> enemies = new List<EnemyMovementTest>();
    private int enemiesMoving = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[EnemyTurnManager] Initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        enemies.AddRange(FindObjectsOfType<EnemyMovementTest>());
        Debug.Log($"[EnemyTurnManager] Found {enemies.Count} enemies.");
    }

    public void StartEnemyTurn()
    {
        Debug.Log("[EnemyTurnManager] Starting enemy turn...");
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        enemiesMoving = 0;

        foreach (EnemyMovementTest enemy in enemies)
        {
            enemiesMoving++;
            Debug.Log($"[EnemyTurnManager] Enemy {enemy.name} is moving...");
            enemy.MoveOneStep();
            yield return new WaitForSeconds(0.5f); // Small delay between enemies
        }

        // If no enemies are moving, end turn immediately
        if (enemiesMoving == 0)
        {
            Debug.Log("[EnemyTurnManager] No enemies moved. Ending enemy turn.");
            EndEnemyTurn();
        }
    }

    public void EnemyFinishedAction()
    {
        enemiesMoving--;
        Debug.Log($"[EnemyTurnManager] An enemy finished moving. Remaining: {enemiesMoving}");

        if (enemiesMoving <= 0)
        {
            Debug.Log("[EnemyTurnManager] All enemies finished moving. Ending enemy turn.");
            EndEnemyTurn();
        }
    }

    private void EndEnemyTurn()
    {
        Debug.Log("[EnemyTurnManager] Enemy turn ended. Switching to player turn.");
        TurnManager.Instance.StartPlayerTurn();
    }
}
