using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Player Object")]
    public GameObject playerObject;
    private PlayerMovement playerMovement;
    public Sprite playerFacingDown;
    public Sprite playerFacingUp;
    public Sprite playerFacingSide;
    public GameObject hitboxes;

    private SpriteRenderer sr;
    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        sr = playerObject.GetComponent<SpriteRenderer>();
    }
    public void ChangeDirection(int direction)
    {
        if(direction == 0)    // facing right
        {
            sr.sprite=playerFacingSide;
            playerObject.transform.localScale = new Vector2(-.7f, .6f);
            hitboxes.transform.localPosition = new Vector3(1, 0, 0);
        }
        else if(direction == 1)    // facing left
        {
            sr.sprite=playerFacingSide;
            playerObject.transform.localScale = new Vector2(0.7f,.6f);
            hitboxes.transform.localPosition = new Vector3(-1, 0, 0);
        }
        else if(direction == 2)
        {
            sr.sprite = playerFacingUp; //facing up
            hitboxes.transform.localPosition = new Vector3(0, 1, 0);
        }
        else if(direction == 3)
        {
            sr.sprite = playerFacingDown;
            hitboxes.transform.localPosition = new Vector3(0, -1, 0);
        }
    }
}
