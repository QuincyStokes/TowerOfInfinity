using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    private bool isPlayerTurn = true;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    // Start the player's turn and then wait 5 seconds before switching turns
    public void EndPlayerTurn()
    {
        if (isPlayerTurn)
        {
            Debug.Log("Player has finished their turn.");
            isPlayerTurn = false; // End player's turn
            StartCoroutine(SwitchTurnAfterDelay(5f)); // Wait for 5 seconds before switching turns
        }
    }

    private IEnumerator SwitchTurnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlayerTurn = true; // Start player's turn again after delay
        Debug.Log("Player's turn starts again.");
    }
}
