using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x > 0.8f) {
            animator.SetFloat("LastHorizontal", 1.0f);
            animator.SetFloat("LastVertical", 0.0f);
        }
        if (movement.x < -0.8f) {
            animator.SetFloat("LastHorizontal", -1.0f);
            animator.SetFloat("LastVertical", 0.0f);
        }
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.y > 0.8f) {
            animator.SetFloat("LastVertical", 1.0f);
            animator.SetFloat("LastHorizontal", 0.0f);
        }
        if (movement.y < -0.8f) {
            animator.SetFloat("LastVertical", -1.0f);
            animator.SetFloat("LastHorizontal", 0.0f);
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            Attack();
        }

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
        // Play attack animation

        // Detect enemies within range

        // Apply damage to all enemies
        
    }
}
