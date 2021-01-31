using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public float attackTime = 0.4f;
    public float hurtTime = 0.4f;
    float recoveryTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // Input
        // If the attack animation isn't finished yet, we can't move
        if (Time.time >= recoveryTime) {
            // Take horizontal / vertical input and 
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Save last horizontal direction for animation use
            if (movement.x > 0.1f) {
                animator.SetFloat("LastHorizontal", 1.0f);
            }
            if (movement.x < -0.1f) {
                animator.SetFloat("LastHorizontal", -1.0f);
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                Attack();
            }
            // Press Z to trigger taking damage
            if (Input.GetKeyDown(KeyCode.Z)) {
                Hurt();
            }
        }
        else {
            movement.x = 0.0f;
            movement.y = 0.0f;
        }
        
        // Update animation parameters
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Attack() {
        // Update time till next action
       recoveryTime = Time.time + attackTime;

        // Play attack animation
        animator.SetTrigger("Attack");

        // Detect enemies within range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);


        // Apply damage to all enemies
        foreach(Collider2D enemy in hitEnemies) {
            Debug.Log("We hit " + enemy.name);
        }

    }

    void Hurt() {
         // Update time till next action
        recoveryTime = Time.time + hurtTime;

        // Play hurt animation
        animator.SetTrigger("Hurt");

        // Reduce health and check for game over

        // If not, apply invulnerability
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
