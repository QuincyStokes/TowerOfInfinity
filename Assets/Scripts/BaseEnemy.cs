using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Properties")]
    public string health;
    public float moveTime = 0.1f;       //Time it will take object to move, in seconds.

    [Header("Refrences")]
    protected Rigidbody2D rb2D;
    public Animator animator;
    public GameObject thisObject;
    public BoxCollider2D collider2d;
    public CircleCollider2D outerCollider2d;
    public GameObject exclamationMark;

    [Header("SFX")]
    public AudioMixerGroup SFXamg;
    public AudioClip attackSFX;

    //-----------------Internal---------------//
    protected TMP_Text healthText;
    protected bool enemyTurn;
    protected float inverseMoveTime;      //Used to make movement more efficient.
    protected bool mustAttack;

    public LayerMask projectileLayer;



    void OnEnable()
    {
        PlayerMovement.OnPlayerMoved += TakeTurn;
        PlayerAttack.OnPlayerAttacked += TakeTurn;
    }

    void OnDisable()
    {
        PlayerMovement.OnPlayerMoved -= TakeTurn;
        PlayerAttack.OnPlayerAttacked -= TakeTurn;

    }

    protected void Start()
    {
        rb2D = GetComponentInChildren<Rigidbody2D>();
        inverseMoveTime = 1 / moveTime;
        healthText = GetComponentInChildren<TMP_Text>();
        //collider2d = GetComponentInChildren<BoxCollider2D>();
        healthText.text = health.ToString();
        //playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        animator.SetInteger("State", 0);
        this.enabled = false;
        mustAttack = false;
    }

    public void ChangeHealth(string attack)
    {
        ExpressionTree tree = new ExpressionTree();
        tree.BuildFromInfix(health + attack);
        tree.InorderTraversal();
        health = tree.Evaluate().ToString();
        UpdateHealth();
    }

    public virtual void UpdateHealth()
    {
        if (health == "0")
        {
            RewardManager.Instance.EnemyKilled();
            Destroy(gameObject);

        }
        else
        {
            healthText.text = health;
        }
    }
    public void ShowExclamation()
    {
        mustAttack = true;
        exclamationMark.SetActive(true);
    }

    public void HideExclamation()
    {
        mustAttack = false;
        exclamationMark.SetActive(false);
    }

    protected abstract void TakeTurn(Vector2 playerPosition);

    public void Move(Vector2 playerPosition)
    {

        if (Vector2.Distance((Vector2)transform.position, playerPosition) < 8)
        {
            RaycastHit2D enemyHit;

            float xDif = transform.position.x - playerPosition.x;
            float yDif = transform.position.y - playerPosition.y;
            if (Mathf.Abs(xDif) > 2 || Mathf.Abs(yDif) > 2) // is away from player
            {
                enemyTurn = false;
                if (Mathf.Abs(xDif) > Mathf.Abs(yDif))
                {
                    if (xDif >= 0)
                    {
                        //this is left movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x - 1, transform.position.y);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if (enemyHit.transform == null)
                        {
                            thisObject.transform.localScale = new Vector2(-5f, 5f);
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x - 1, transform.position.y, 0)));
                        }
                        else
                        {
                            Debug.Log(enemyHit.transform.gameObject.name);
                        }

                    }
                    else
                    {
                        //this is right movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x + 1, transform.position.y);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if (enemyHit.transform == null)
                        {
                            thisObject.transform.localScale = new Vector2(5f, 5f);
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x + 1, transform.position.y, 0)));
                        }
                    }

                }
                else
                {
                    if (yDif >= 0)
                    {
                        //this is down movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x, transform.position.y - 1);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if (enemyHit.transform == null)
                        {
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x, transform.position.y - 1, 0)));
                        }
                    }
                    else
                    {
                        //this is up movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x, transform.position.y + 1);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if (enemyHit.transform == null)
                        {
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x, transform.position.y + 1, 0)));
                        }
                    }
                }
            }
        }

    }
    
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        //isPlayerMoving = true;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        animator.SetInteger("State", 1);
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
        animator.SetInteger("State", 0);
        rb2D.bodyType = RigidbodyType2D.Static;
        //isPlayerMoving = false;
    }

}
