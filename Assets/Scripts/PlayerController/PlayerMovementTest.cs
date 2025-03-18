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
        if (isMoving)
        {
            moveProgress += Time.deltaTime * moveSpeed;
            moveProgress = Mathf.Min(moveProgress, 1f);

            if (moveProgress >= 1f)
            {
                transform.position = targetPosition;
                isMoving = false;
                moveProgress = 0f;

                // End player's turn after moving
                TurnManager.Instance.EndPlayerTurn();
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);
            }
        }
    }

    public void TryMove(Vector2 direction)
    {
        if (isMoving) return;

        Vector3 nextPosition = transform.position + new Vector3(direction.x, direction.y, 0f);
        if (wallTilemap == null) return;

        Vector3Int nextCell = wallTilemap.WorldToCell(nextPosition);

        if (wallTilemap.GetTile(nextCell) == null)
        {
            startPosition = transform.position;
            targetPosition = nextPosition;
            isMoving = true;
            moveProgress = 0f;
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
