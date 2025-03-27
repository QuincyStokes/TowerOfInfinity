using UnityEngine;

public class PlayerAttackTest : MonoBehaviour
{
    // This method will be called to perform an attack in the given direction.
    public void Attack(Vector2 direction)
    {
        // Print out the attack message with the given direction.
        Debug.Log("ATTACK in direction: " + direction);

    }
}
