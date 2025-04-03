using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public static EnemyTurnManager Instance { get; private set; }
    
    private List<EnemyMovementTest> enemies = new List<EnemyMovementTest>();
    private int enemiesMoving = 0;
    private bool canEnemiesMove = false;

    // New: Set to track reserved cells.
    private HashSet<Vector3Int> reservedCells = new HashSet<Vector3Int>();

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
        // Clear reservations at the end of the enemy turn.
        reservedCells.Clear();
        Debug.Log("[EnemyTurnManager] Enemy turn ended. Switching to player turn.");
        TurnManager.Instance.StartPlayerTurn();
    }

    // Reservation API
    public bool IsCellReserved(Vector3Int cell)
    {
        return reservedCells.Contains(cell);
    }

    public bool ReserveCell(Vector3Int cell)
    {
        if (reservedCells.Contains(cell))
        {
            return false;
        }
        reservedCells.Add(cell);
        return true;
    }

    public void UnreserveCell(Vector3Int cell)
    {
        reservedCells.Remove(cell);
    }
}
