using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public static EnemyTurnManager Instance { get; private set; }

    private List<EnemyMovementTest> enemies = new List<EnemyMovementTest>();
    private int enemiesMoving = 0;
    private bool canEnemiesMove = false;

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
        canEnemiesMove = true;
        Debug.Log("[EnemyTurnManager] Starting enemy turn...");
        StartCoroutine(EnemyTurnRoutine());
    }

    public void StopEnemyMovement(){
        canEnemiesMove = false;
    }

     private IEnumerator EnemyTurnRoutine()
    {
        foreach (EnemyMovementTest enemy in enemies)
        {
            if (canEnemiesMove)
            {
                enemiesMoving++;
                enemy.MoveOneStep(); 
                yield return new WaitForSeconds(.5f);
            }
        }

        // After all enemies move, inform the TurnManager that the enemy turn has ended
        yield return new WaitUntil(() => enemiesMoving <= 0);

        EndEnemyTurn();
    }

    public void EnemyFinishedAction()
    {
        enemiesMoving--;
        if (enemiesMoving <= 0)
        {
            EndEnemyTurn(); // End enemy turn after last enemy moves
        }
    }

    private void EndEnemyTurn()
    {
        Debug.Log("[EnemyTurnManager] Enemy turn ended. Switching to player turn.");
        TurnManager.Instance.StartPlayerTurn();
    }
}
