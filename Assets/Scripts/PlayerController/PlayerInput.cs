using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovementTest pm;
    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovementTest>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm != null && !pm.IsMoving())
        {
            if (Input.GetKeyDown(KeyCode.W)) pm.TryMove(Vector2.up);
            else if (Input.GetKeyDown(KeyCode.S)) pm.TryMove(Vector2.down);
            else if (Input.GetKeyDown(KeyCode.A)) pm.TryMove(Vector2.left);
            else if (Input.GetKeyDown(KeyCode.D)) pm.TryMove(Vector2.right);
        }
    }
}
