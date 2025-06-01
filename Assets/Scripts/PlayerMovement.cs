using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour {
    // Start is called before the first frame update
    public float moveTime = 0.1f;       //Time it will take object to move, in seconds.
    public LayerMask blockingLayer;     //Layer on which collision will be checked.
    public LayerMask enemyLayer;
    public static bool movementLocked = false;


    private BoxCollider2D boxCollider;  //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;           //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;      //Used to make movement more efficient.
    private bool isPlayerMoving;
    private static PlayerAnimation playerAnimation;
    public static int currentDirection;
    public TilemapSetup tilemapSetup;
    [Header("Sound Effects")]
    public AudioMixerGroup SFXamg;
    public AudioClip footstepSFX;
    // 0 == right
    // 1 == left
    // 2 == up
    // 3 == down
    
    public delegate void PlayerMoved(Vector2 playerPosition);
    public static event PlayerMoved OnPlayerMoved;
    void Start() {
        playerAnimation=GetComponent<PlayerAnimation>();
         //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent <BoxCollider2D> ();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent <Rigidbody2D> ();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;

        //lets set the player's position in the center of the tile room
        transform.position = new Vector3(3.5f, 3.5f);

        GameManager.beatLevel += ResetPlayerPosition;
    }

    // Update is called once per frame
    void Update() {
        if(!movementLocked)
        {
            if(Input.GetKeyDown(KeyCode.D)){
                AttemptMove<Wall>(1, 0);
                currentDirection = 0;
                playerAnimation.ChangeDirection(currentDirection);
            } else if (Input.GetKeyDown(KeyCode.A)){
                AttemptMove<Wall>(-1, 0);
                currentDirection = 1;
                playerAnimation.ChangeDirection(currentDirection);
            }else if (Input.GetKeyDown(KeyCode.W)){
                AttemptMove<Wall>(0, 1);
                currentDirection = 2;
                playerAnimation.ChangeDirection(currentDirection);
            }else if (Input.GetKeyDown(KeyCode.S)){
                AttemptMove<Wall>(0, -1);
                currentDirection = 3;
                playerAnimation.ChangeDirection(currentDirection);
            }
        }
        
    }

    void FixedUpdate()
    {
        
    }
    /*
        void Move()

        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                transform.position = new Vector2(transform.position.x + .16f, transform.position.y);
                print("D pressed!");
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                if(transform.position.x -.16f >= -1.25){
                    transform.position = new Vector2(transform.position.x-.16f, transform.position.y);
                }
            }
            if(Input.GetKeyDown(KeyCode.W))
            {
                if(transform.position.y +.16f <= .6f){
                    transform.position = new Vector2(transform.position.x, transform.position.y+.16f);
                }
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                if(transform.position.y -.16f >= -.8f){
                    transform.position = new Vector2(transform.position.x, transform.position.y-.16f);
                }

            }
        }
        */
    public void ResetPlayerPosition(int level)
    {
        transform.position = new Vector3(3.5f, 3.5f);
        StopAllCoroutines();
        isPlayerMoving = false;
    }

    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component
    {
        //Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D wallHit;
        RaycastHit2D enemyHit;

        //Set canMove to true if Move was successful, false if failed.
        bool canMove = Move (xDir, yDir, out wallHit, out enemyHit);

        // //Check if nothing was hit by linecast
        // if(wallHit.transform == null || enemyHit) 
        //     //If nothing was hit, return and don't execute further code.
        //     return;

        // //Get a component reference to the component of type T attached to the object that was hit
        // T hitComponent = wallHit.transform.GetComponent<T> ();

        // //If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
        // if(!canMove && hitComponent != null){

        //     //Call the OnCantMove function and pass it hitComponent as a parameter.
        //     //OnCantMove(hitComponent);
        // }
    }


    protected bool Move (int xDir, int yDir, out RaycastHit2D wallHit, out RaycastHit2D enemyHit)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2 (xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        wallHit = Physics2D.Linecast (start, end, blockingLayer);
        enemyHit = Physics2D.Linecast (start, end, enemyLayer);

        //Re-enable boxCollider after linecast
        boxCollider.enabled = true;

        //Check if anything was hit
        if(wallHit.transform == null && enemyHit.transform == null && !isPlayerMoving)
        {
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            AudioManager.Instance.PlayOneShotVariedPitch(footstepSFX, 1f, SFXamg, .05f);
            StartCoroutine(SmoothMovement (end));

            //Return true to say that Move was successful
            return true;
        }

        //If something was hit, return false, Move was unsuccesful.
        return false;
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        OnPlayerMoved?.Invoke(transform.position);
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        isPlayerMoving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while(sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector2 newPostion = Vector2.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition (newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
        isPlayerMoving = false;
    }

    
    public static void FacePlayer(int direction)
    {
        currentDirection = direction;
        playerAnimation.ChangeDirection(currentDirection);
    }

    
}
