using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    // Animation and general variables
    public Rigidbody2D rb;
    public Animator animator;

    // Variables related to movement
    public float moveSpeed = 1f;
    Vector2 movement;
    float movementTime;
    float idleTime;
    float recoveryTime;

    // Variables related to aggro range
    public Transform aggroPoint;
    public float aggroRange = 1.0f;
    public LayerMask playerLayer;

    // Variables related to chasing
    Collider2D playerTarget;
    bool isAttacking;
    bool isRetreating;

    // Variables related to combat
    public int maxHealth = 1;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Press X to trigger enemy retreat
        if (Input.GetKeyDown(KeyCode.X)) {
            Retreat();
        }

        // If the enemy is chasing, keep chasing
        if (isAttacking) {
            Chase();
        }
        else {
            // If the enemy isn't retreating, check aggro range
            if (!isRetreating) {
                CheckAggro();
            }
            // If we're done moving/idling, set another destination and timers
            if (Time.time >= recoveryTime) {
                isRetreating = false;
                movement.x = Random.Range(-1.0f, 1.0f);
                movement.y = Random.Range(-1.0f, 1.0f);

                movementTime = Random.Range(0.0f, 2.0f);
                idleTime = Random.Range(0.0f, 2.0f);
                recoveryTime = Time.time + movementTime + idleTime;
            }
        }

        if (movement.x > 0.1f) {
            animator.SetFloat("LastHorizontal", 1.0f);
        }
        if (movement.x < -0.1f) {
            animator.SetFloat("LastHorizontal", -1.0f);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Movement: If the enemy is still in movement time or attacking, they move.
        if (Time.time < recoveryTime - idleTime || isAttacking) {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        // If not, their speed is set to 0 (returns to idle animation)
        if (Time.time >= recoveryTime - idleTime) {
            movement = new Vector2(0.0f, 0.0f);
        }
    }

    // Check if the player enters aggro range
    void CheckAggro() {
        playerTarget = Physics2D.OverlapCircle(aggroPoint.position, aggroRange, playerLayer);
        if (playerTarget != null) {
            Debug.Log("Beginning the Chase");

            isAttacking = true;
        }
    }

    // Chase player position
    void Chase() {
        Transform playerPos = playerTarget.GetComponent<Transform>();
        Vector2 distance = new Vector2(playerPos.position.x - rb.position.x, playerPos.position.y - rb.position.y);
        Vector2 direction = distance.normalized;
        movement = direction;
    }

    // After a succesfull attack, turn back and retreat, ignoring aggro range for a while
    public void Retreat() {
        Debug.Log("Triggering retreat");

        animator.SetTrigger("Retreat");

        isAttacking = false;
        isRetreating = true;
        movement = movement * -1.0f;
        movementTime = 3.0f;
        idleTime = 3.0f;
        recoveryTime = Time.time + movementTime + idleTime;
    }

    // Take damage after being attacked
    public void TakeDamage(int damage) {        
        currentHealth -= damage;

        // Play hurt animation
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0) {
            Die();
        }
    }

    // Die if health reaches 0
    void Die() {
        Debug.Log("Enemy died!");

        // Play death animation
        animator.SetTrigger("Die");

        // Disable enemy
    }

    // Debug tool, visualizes aggro range
    void OnDrawGizmosSelected() {
        if (aggroPoint == null)
            return;

        Gizmos.DrawWireSphere(aggroPoint.position, aggroRange);
    }
}
