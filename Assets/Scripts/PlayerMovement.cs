using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using static Utility.Helper;
using CELL_PLACEMENT = Zone.CELL_PLACEMENT;
using DIRECTION = Tile.DIRECTION;

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

    public int attackDamage = 1;

    public Vector2Int currentZone;

    public float invulnerabilityTime;
    private float currentInvulnTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.isGameOver)
        {
            return;
        }

        if(currentInvulnTime > 0)
        {
            currentInvulnTime -= Time.deltaTime;

            if(currentInvulnTime < 0)
            {
                currentInvulnTime = 0;
            }
        }

        // Input
        // If the attack animation isn't finished yet, we can't move
        if (Time.time >= recoveryTime) {
            // Take horizontal / vertical input and 
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Save last horizontal direction for animation use
            if (movement.x > 0.1f && animator.GetFloat("LastHorizontal") != 1.0f) {
                animator.SetFloat("LastHorizontal", 1.0f);
                attackPoint.localPosition = new Vector3(Math.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
            }
            if (movement.x < -0.1f && animator.GetFloat("LastHorizontal") != -1.0f) {
                animator.SetFloat("LastHorizontal", -1.0f);
                attackPoint.localPosition = new Vector3(-attackPoint.localPosition.x, attackPoint.localPosition.y, attackPoint.localPosition.z);
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                Attack();
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
        if (GameController.instance.isGameOver)
        {
            return;
        }

        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Camera.main.transform.position = new Vector3(rb.position.x, rb.position.y, Camera.main.transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameController.instance.isGameOver)
        {
            return;
        }

        if (collision.gameObject.GetComponent<Exit>() is var exit && exit != null)
        {
            var zone = ZoneGenerator.instance.GetZone(currentZone);
            var tile = collision.transform.parent.GetComponent<Tile>();
            var currentExitPlacement = zone.InstancedTiles.First(x => x.Value == tile).Key;
            var currentExitDirection = tile.DirectionFromWall(exit.gameObject);

            var startPos = new Tuple<CELL_PLACEMENT, DIRECTION>(currentExitPlacement, currentExitDirection).AttachedCell();
            ZoneGenerator.instance.GenerateZone(NewMapPosition(currentZone, currentExitDirection), startPos);

            exit.gameObject.SetActive(false);
        }
        else if(collision.gameObject.GetComponent<EnemyAI>() is var enemy && enemy != null && currentInvulnTime == 0)
        {
            Hurt(enemy.damage);
            enemy.Retreat();
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }
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
            enemy.GetComponent<EnemyAI>().TakeDamage(attackDamage);
        }

    }

    void Hurt(float damage) {
         // Update time till next action
        recoveryTime = Time.time + hurtTime;

        // Play hurt animation
        animator.SetTrigger("Hurt");

        GameController.instance.UpdateSpoilage(-damage);

        currentInvulnTime = invulnerabilityTime;
    }

    // Debug tool, visualizes attack range
    void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
