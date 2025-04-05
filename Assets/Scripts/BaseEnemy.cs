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
        mustAttack=false;
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
            BossRoomHandler.Instance.OpenDoor();
            RewardManager.Instance.EnemyKilled();
            Destroy(gameObject);
            
        } else {
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


}
