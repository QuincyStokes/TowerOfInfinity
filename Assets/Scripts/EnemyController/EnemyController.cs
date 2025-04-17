using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(EnemyMovementTest))]

public class EnemyController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject thisObject;
    [SerializeField] private BoxCollider2D collider2d;
    [SerializeField] private CircleCollider2D outerCollider2d;
    [SerializeField] private GameObject exclamationMark;

    [Header("SFX")]
    [SerializeField] public AudioMixerGroup SFXamg;
    [SerializeField] public AudioClip attackSFX;
    private EnemyHealth enemyHealth;
    private EnemyAttack enemyAttack;
    private EnemyMovementTest enemyMovement;



    //ADDED REFERENCE DECLARATIONS
    private EnemyHealth enemyHealth;
    private EnemyAttack enemyAttack;
    private EnemyMovementTest enemyMovementTest;

    private void Awake(){
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovementTest>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeTurn(Vector3 playerPosition){
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (distanceToPlayer <= 2f){
            //EVENTUALLY INCLUDE THE QUESTION MARK BEFORE ATTACKING
            StartCoroutine(AttackTurn(playerPosition));
        }else{
            StartCoroutine(MoveTurn(playerPosition));
        }
    }

    private IEnumerator MoveTurn(Vector3 targetPosition){
        enemyMovement.MoveOneStep();
        yield return new WaitForSeconds(0.5f);
        EnemyTurnManager.Instance.EnemyFinishedAction();
    }

    private IEnumerator AttackTurn(Vector3 targetPosition){
        //COMMENTED OUT BECAUSE NOT IMPLEMENTED YET
        //enemyAttack.Attack();
        yield return new WaitForSeconds(0.5f);
        EnemyTurnManager.Instance.EnemyFinishedAction();
    }

    public void OnHit(){
        
    }

}
