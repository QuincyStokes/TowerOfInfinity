using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovementTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform player; // Reference to the player
    private const string WALL_TAG = "Wall";

    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float moveProgress = 0f;
    private Tilemap wallTilemap;

    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Start()
    {
        GameObject wallObject = GameObject.FindGameObjectWithTag(WALL_TAG);
        if (wallObject != null)
        {
            wallTilemap = wallObject.GetComponent<Tilemap>();
            Debug.Log($"[{name}] Found wall tilemap.");
        }

        if (wallTilemap == null)
        {
            Debug.LogError($"[{name}] Wall tilemap not found! Make sure your WALLS tilemap has the 'Wall' tag.");
        }
    }

    void Update()
    {
        if (isMoving)
        {
            moveProgress += Time.deltaTime * moveSpeed;
            moveProgress = Mathf.Min(moveProgress, 1f);

            if (moveProgress >= 1f)
            {
                transform.position = targetPosition;
                isMoving = false;
                moveProgress = 0f;
                Debug.Log($"[{name}] Reached target position at {targetPosition}");

                // Let the EnemyTurnManager know this enemy finished moving
                EnemyTurnManager.Instance.EnemyFinishedAction();
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);
            }
        }
    }

    public void MoveOneStep()
    {
        if (!isMoving)
        {
            Debug.Log($"[{name}] Attempting to move...");
            TryMove(GetBestDirection());
        }
        else
        {
            Debug.Log($"[{name}] Already moving, cannot move again.");
        }
    }

    private void TryMove(Vector2 direction)
    {
        Vector3 nextPosition = transform.position + new Vector3(direction.x, direction.y, 0f);
        if (wallTilemap == null || player == null) return;

        Vector3Int nextCell = wallTilemap.WorldToCell(nextPosition);
        Vector3Int playerCell = wallTilemap.WorldToCell(player.position);

        Debug.Log($"[{name}] Trying to move {direction} to {nextPosition}");

        if (wallTilemap.GetTile(nextCell) == null && nextCell != playerCell)
        {
            startPosition = transform.position;
            targetPosition = nextPosition;
            isMoving = true;
            moveProgress = 0f;

            Debug.Log($"[{name}] Moving to {targetPosition}");
        }
        else
        {
            Debug.Log($"[{name}] Move blocked! Wall or player at {nextPosition}");
            EnemyTurnManager.Instance.EnemyFinishedAction();
        }
    }

    private Vector2 GetBestDirection()
    {
        if (player == null)
        {
            Vector2 randomDir = directions[Random.Range(0, directions.Length)];
            Debug.Log($"[{name}] No player found, moving randomly {randomDir}");
            return randomDir;
        }

        Vector2 bestDirection = Vector2.zero;
        float shortestDistance = float.MaxValue;
        Vector3 playerPosition = player.position;

        Debug.Log($"[{name}] Evaluating best move towards player at {playerPosition}");

        foreach (Vector2 dir in directions)
        {
            Vector3 nextPosition = transform.position + new Vector3(dir.x, dir.y, 0f);
            Vector3Int nextCell = wallTilemap.WorldToCell(nextPosition);
            Vector3Int playerCell = wallTilemap.WorldToCell(player.position);

            if (wallTilemap.GetTile(nextCell) == null && nextCell != playerCell)
            {
                float distanceToPlayer = Vector3.Distance(nextPosition, playerPosition);
                Debug.Log($"[{name}] Direction {dir} -> Distance: {distanceToPlayer}");

                if (distanceToPlayer < shortestDistance)
                {
                    shortestDistance = distanceToPlayer;
                    bestDirection = dir;
                }
            }
        }

        if (bestDirection == Vector2.zero)
        {
            bestDirection = directions[Random.Range(0, directions.Length)];
            Debug.Log($"[{name}] No optimal move found, moving randomly {bestDirection}");
        }
        else
        {
            Debug.Log($"[{name}] Best move is {bestDirection}");
        }

        return bestDirection;
    }

    private void EnemyAttack(){
        Debug.Log("Enemy is attacking...");
        EnemyTurnManager.Instance.EnemyFinishedAction();
    }
}
