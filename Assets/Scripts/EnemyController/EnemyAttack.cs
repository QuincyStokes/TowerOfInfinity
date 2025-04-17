using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Animator))]
public class EnemyAttack : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("The projectile prefab to instantiate when attacking.")]
    public GameObject projectilePrefab;

    [Tooltip("Speed multiplier for the projectile.")]
    public float projectileSpeed = 1f;

    [Header("Audio Settings")]
    [Tooltip("Sound effect played when attacking.")]
    public AudioClip attackSFX;
    public AudioMixerGroup SFXamg;

    [Header("Animation Settings")]
    [Tooltip("Animator state index for the attack animation.")]
    public int attackAnimationState = 2;

    [Tooltip("Duration to wait before returning to idle state.")]
    public float animationResetDelay = 0.5f;

    private Animator animator;
    private Transform playerTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Attempt to find player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in scene. Make sure the Player has the 'Player' tag assigned.");
        }
    }

    public void Attack()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab not assigned.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning("Cannot attack: Player reference is missing.");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Vector2 velocity = direction * projectileSpeed;

        // Fire projectile
        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.FireProjectile(velocity, this.gameObject);
        }

        // Play attack animation and sound
        StartCoroutine(AttackAnimationTimer());
        if (attackSFX != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayOneShotVariedPitch(attackSFX, 1f, SFXamg, 0.1f);
        }
    }

    private IEnumerator AttackAnimationTimer()
    {
        if (animator != null)
        {
            animator.SetInteger("State", attackAnimationState);
        }

        yield return new WaitForSeconds(animationResetDelay);

        if (animator != null)
        {
            animator.SetInteger("State", 0);
        }
    }
} 
