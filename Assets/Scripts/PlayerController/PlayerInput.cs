using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovementTest pm;
    public static Vector2 lastDirection = Vector2.right; // default facing right

    void Start()
    {
        pm = GetComponent<PlayerMovementTest>();
    }

    void Update()
    {
        if (TurnManager.Instance.IsPlayerTurn())
        {
            if (pm != null && !pm.IsMoving())
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    pm.TryMove(Vector2.up);
                    lastDirection = Vector2.up;
                    TurnManager.Instance.EndPlayerTurn();
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    pm.TryMove(Vector2.down);
                    lastDirection = Vector2.down;
                    TurnManager.Instance.EndPlayerTurn();
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    pm.TryMove(Vector2.left);
                    lastDirection = Vector2.left;
                    TurnManager.Instance.EndPlayerTurn();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    pm.TryMove(Vector2.right);
                    lastDirection = Vector2.right;
                    TurnManager.Instance.EndPlayerTurn();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Trigger the attack and print out the debug information.
                GetComponent<PlayerAttackTest>().Attack(lastDirection);
                TurnManager.Instance.EndPlayerTurn();
            }
        }
    }
}
