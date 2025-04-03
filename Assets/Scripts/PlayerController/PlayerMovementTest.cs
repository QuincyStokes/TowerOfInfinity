using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private const string WALL_TAG = "Wall";

    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float moveProgress = 0f;
    private Tilemap wallTilemap;

    void Start()
    {
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
        // Check if it's the player's turn before allowing movement
        if (!TurnManager.Instance.IsPlayerTurn())
        {
            return; // Don't allow movement if it's not the player's turn
        }

        if (isMoving)
        {
            moveProgress += Time.deltaTime * moveSpeed;
            moveProgress = Mathf.Min(moveProgress, 1f);

            if (moveProgress >= 1f)
            {
                transform.position = targetPosition;
                isMoving = false;
                moveProgress = 0f;

                // Optionally end player's turn here
                // TurnManager.Instance.EndPlayerTurn();
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);
            }
        }
    }

    public void TryMove(Vector2 direction)
    {
        Vector3 nextPosition = transform.position + new Vector3(direction.x, direction.y, 0f);
        if (wallTilemap == null) return;

        Vector3Int nextCell = wallTilemap.WorldToCell(nextPosition);

        // Check if the target cell is blocked by a wall.
        if (wallTilemap.GetTile(nextCell) != null)
        {
            Debug.Log("Move blocked by a wall at " + nextPosition);
            return;
        }

        // Check if the target cell is reserved by an enemy.
        if (EnemyTurnManager.Instance.IsCellReserved(nextCell))
        {
            Debug.Log("Move blocked! Tile " + nextPosition + " is reserved by an enemy.");
            return;
        }

        // If the cell is free, allow movement.
        startPosition = transform.position;
        targetPosition = nextPosition;
        isMoving = true;
        moveProgress = 0f;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
