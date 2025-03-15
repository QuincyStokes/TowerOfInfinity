
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Audio;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    //things the enemy needs
    //health
    //functions that change the "format" it's displayed in, depending on what type of number it is  
    //text for health
    private TMP_Text healthText;
    public string health = "20";
    private Rigidbody2D rb2D;
    public float moveTime = 0.1f;       //Time it will take object to move, in seconds.

    private float inverseMoveTime;      //Used to make movement more efficient.
    
    private bool enemyTurn;

    public GameObject projectilePrefab;

    public LayerMask projectileLayer;
    //public PlayerHealth playerHealth;
    public Animator animator;
    public GameObject thisObject;
    public BoxCollider2D collider2d;
    public CircleCollider2D outerCollider2d;

    [Header("SFX")]
    public AudioMixerGroup SFXamg;
    public AudioClip shootSFX;


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

    void Awake()
    {
        if(GameManager.instance.GetCurrentLevel() == 1)
        {
            health = ((int)Random.Range(1+GameManager.instance.numOfEnemy/1.5f, GameManager.instance.numOfEnemy*1.5f+4)).ToString();
        }
        else if(GameManager.instance.GetCurrentLevel() == 2)
        {
            int n = Random.Range(1, 100);

            if(n <25) // 25% chance to have positive health
            {
                health = ((int)Random.Range(4+GameManager.instance.numOfEnemy/1.5f, GameManager.instance.numOfEnemy*1.6f+7)).ToString();
            }
            else  // 75 % chance to have negative health
            {
                health = ((int)Random.Range(-GameManager.instance.numOfEnemy*1.5f-4, -GameManager.instance.numOfEnemy/1.5f-2)).ToString();
            }
        }
        else if(GameManager.instance.GetCurrentLevel() == 3)
        {
            int upper = (int)Random.Range(4+GameManager.instance.numOfEnemy*2, GameManager.instance.numOfEnemy*3+6);
            int lower = (int)Random.Range(2, GameManager.instance.numOfEnemy/2+3);
            if(upper % lower ==0)
            {
                health = ((int)(upper*0.8)).ToString();
            }
            else
            {
                while(lower%7==0 || lower%11==0 || lower%13==0 || lower%17==0 || lower%19==0 || lower%23==0 || lower%29==0 || lower%31==0 || lower%37==0 || lower%41==0 || lower%43==0 || lower%47==0)
                {
                    lower--;
                }
                health = upper.ToString()+"/"+lower.ToString();
            }
            int n = Random.Range(1, 100);
            if(n<50)
            {
                health = "-"+health;
            }
        }
        else if(GameManager.instance.GetCurrentLevel() == 4)
        {
            int upper = (int)Random.Range(4+GameManager.instance.numOfEnemy*5, GameManager.instance.numOfEnemy*10+6);
            int lower = (int)Random.Range(2+GameManager.instance.numOfEnemy, GameManager.instance.numOfEnemy*3+6);
            if(upper % lower ==0)
            {
                health = (upper*3).ToString();
            }
            else
            {
                while(lower%11==0 || lower%13==0 || lower%17==0 || lower%19==0 || lower%23==0 || lower%29==0 || lower%31==0 || lower%37==0 || lower%41==0 || lower%43==0 || lower%47==0 || lower%53==0 || lower%59==0 || lower%61==0 || lower%67==0 || lower%71==0 || lower%73==0 || lower%79==0)
                {
                    lower++;
                }
                health = upper.ToString()+"/"+lower.ToString();
            }
            int n = Random.Range(1, 100);
            if(n<50)
            {
                health = "-"+health;
            }
        }
    }


    void Start()
    {
        rb2D = GetComponentInChildren<Rigidbody2D>();
        inverseMoveTime = 1 / moveTime;
        healthText = GetComponentInChildren<TMP_Text>();
        //collider2d = GetComponentInChildren<BoxCollider2D>();
        healthText.text = health.ToString();
        ChangeHealth("+0");
        //playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        animator.SetInteger("State", 0);
        this.enabled = false;
    }
   
    public void ChangeHealth(string attack)
    {
        ExpressionTree tree = new ExpressionTree();
        tree.BuildFromInfix(health+attack);
        tree.InorderTraversal();
        health = tree.Evaluate().ToString();
        UpdateHealth();
    }

    public void UpdateHealth(){
        if(health == "0") {
            RewardManager.Instance.EnemyKilled();
            Destroy(gameObject); 
        } else {
            healthText.text = health;
        }
    }

    
    void TakeTurn(Vector2 playerPosition)
    {
        enemyTurn =  true;
        if(enemyTurn)
        {
            Move(playerPosition);
        }
        if(enemyTurn)
        {
            Attack(playerPosition);
        }
    }

    void Move(Vector2 playerPosition) {
       
        if(Vector2.Distance((Vector2)transform.position, playerPosition) < 8)
        {
            RaycastHit2D enemyHit;
            
            float xDif = transform.position.x - playerPosition.x;
            float yDif = transform.position.y - playerPosition.y;
            if(Mathf.Abs(xDif) > 2 || Mathf.Abs(yDif) > 2) // is away from player
            {
                enemyTurn = false;
                if(Mathf.Abs(xDif)> Mathf.Abs(yDif) )
                {
                    if(xDif>=0)
                    {
                        //this is left movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x-1, transform.position.y);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if(enemyHit.transform == null)
                        {
                            thisObject.transform.localScale = new Vector2(-5f, 5f);
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x-1, transform.position.y, 0)));
                        }
                        else
                        {
                            Debug.Log(enemyHit.transform.gameObject.name);
                        }
                       
                    }
                    else{
                        //this is right movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x+1, transform.position.y);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if(enemyHit.transform == null)
                        {
                            thisObject.transform.localScale = new Vector2(5f, 5f);
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x+1, transform.position.y, 0)));
                        }
                    }
                    
                }
                else {
                    if(yDif>=0)
                    {
                        //this is down movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x, transform.position.y-1);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if(enemyHit.transform == null)
                        {
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x, transform.position.y-1, 0)));
                        }
                    }
                    else
                    {
                        //this is up movement
                        Vector2 start = transform.position;
                        Vector2 end = new Vector2(transform.position.x, transform.position.y+1);
                        collider2d.enabled = false;
                        outerCollider2d.enabled = false;
                        enemyHit = Physics2D.Linecast(start, end, projectileLayer);
                        collider2d.enabled = true;
                        outerCollider2d.enabled = true;
                        if(enemyHit.transform == null)
                        {
                            StartCoroutine(SmoothMovement(new Vector3(transform.position.x, transform.position.y+1, 0)));
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


    void Attack(Vector2 playerPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 velocity = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y);

        //can set the text of the projectile
        //get player health to determine what operation to do
        
        StartCoroutine(AttackAnimationTimer());
        AudioManager.Instance.PlayOneShotVariedPitch(shootSFX, 1f, SFXamg, .1f);
        projectile.GetComponent<Projectile>().FireProjectile(velocity, this.gameObject);
    }

    private IEnumerator AttackAnimationTimer()
    {
        animator.SetInteger("State", 2);
        yield return new WaitForSeconds(.5f);
        animator.SetInteger("State", 0);
    }
    //hello

}
