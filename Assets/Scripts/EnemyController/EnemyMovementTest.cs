using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovementTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform player; // Reference to the player
    private const string WALL_TAG = "Wall";
    private const float MOVE_INTERVAL = 1f; // Move every second

    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float moveProgress = 0f;
    private Tilemap wallTilemap;
    private float moveTimer = 0f;

    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Start()
    {
        // Find the WALLS tilemap at start
        GameObject wallObject = GameObject.FindGameObjectWithTag(WALL_TAG);
        if (wallObject != null)
        {
            wallTilemap = wallObject.GetComponent<Tilemap>();
        }

        if (wallTilemap == null)
        {
            Debug.LogError("Wall tilemap not found! Make sure your WALLS tilemap has the 'Wall' tag.");
        }
    }

    void Update()
    {
        if (!isMoving)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= MOVE_INTERVAL)
            {
                moveTimer = 0f;
                TryMove(GetBestDirection());
            }
        }
        else
        {
            // Handle smooth movement between tiles
            moveProgress += Time.deltaTime * moveSpeed;
            moveProgress = Mathf.Min(moveProgress, 1f); // Clamp progress

            if (moveProgress >= 1f)
            {
                transform.position = targetPosition;
                isMoving = false;
                moveProgress = 0f;
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);
            }
        }
    }

    private void TryMove(Vector2 direction)
    {
        Vector3 nextPosition = transform.position + new Vector3(direction.x, direction.y, 0f);
        if (wallTilemap == null || player == null) return;

        Vector3Int nextCell = wallTilemap.WorldToCell(nextPosition);
        Vector3Int playerCell = wallTilemap.WorldToCell(player.position);

        // Check if there's a wall or the player at the next position
        if (wallTilemap.GetTile(nextCell) == null && nextCell != playerCell)
        {
            startPosition = transform.position;
            targetPosition = nextPosition;
            isMoving = true;
            moveProgress = 0f;
        }
    }

    private Vector2 GetBestDirection()
    {
        if (player == null) return directions[Random.Range(0, directions.Length)]; // Fallback to random if no player

        Vector2 bestDirection = Vector2.zero;
        float shortestDistance = float.MaxValue;
        Vector3 playerPosition = player.position;

        foreach (Vector2 dir in directions)
        {
            Vector3 nextPosition = transform.position + new Vector3(dir.x, dir.y, 0f);
            Vector3Int nextCell = wallTilemap.WorldToCell(nextPosition);
            Vector3Int playerCell = wallTilemap.WorldToCell(player.position);

            // Check if it's a valid move (not a wall and not occupied by the player)
            if (wallTilemap.GetTile(nextCell) == null && nextCell != playerCell)
            {
                float distanceToPlayer = Vector3.Distance(nextPosition, playerPosition);
                if (distanceToPlayer < shortestDistance)
                {
                    shortestDistance = distanceToPlayer;
                    bestDirection = dir;
                }
            }
        }

        return bestDirection != Vector2.zero ? bestDirection : directions[Random.Range(0, directions.Length)]; // If no valid move, pick random
    }
}
