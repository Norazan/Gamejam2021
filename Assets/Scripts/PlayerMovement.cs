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

    public Vector2Int currentZone;

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
        Camera.main.transform.position = new Vector3(rb.position.x, rb.position.y, Camera.main.transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        else
        {
            movement.x = 0;
            movement.y = 0;
        }
    }

    void Attack() {
        // Play attack animation

        // Detect enemies within range

        // Apply damage to all enemies
        
    }
}
