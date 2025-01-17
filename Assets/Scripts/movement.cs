using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public int maxHealth = 3;
    public Animator animator;
    public Rigidbody2D rb;
    public float jumpHeight = 5f;
    private float movementx = 0f;
    public float moveSpeed = 5f;
    private bool facingRight = true;
    public bool isGrounded = true; // Default to true for simplicity

    void Update()
    {
        // Checking Player MaxHealth
        if (maxHealth <= 0)
        {
            Die();
        }

        // Handle horizontal movement input
        movementx = Input.GetAxis("Horizontal");

        // Flip character based on movement direction
        if (movementx < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movementx > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            isGrounded = false;
            animator.SetBool("Jump", true);
        }

        if (Mathf.Abs(movementx) > 0f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movementx < 0.1f)
        {
            animator.SetFloat("Run", 0f);
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Attack");
            
        }
    }

    private void FixedUpdate()
    {
        // Move the character horizontally
        rb.linearVelocity = new Vector2(movementx * moveSpeed, rb.linearVelocity.y);

        // Simple ground check to verify if velocity.y is close to zero
        isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.001f;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }

    public void TakeDamage (int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
    }

    void Die()
    {
        Debug.Log("Player Death :(");
    }

}
