using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float runSpeed = 8;
    public float slideSpeed = 15;
    public float jumpSpeed = 5;
    public float hurtSpeed = 2;
    float speed;

    public float jumpForce = 550;
    bool isGrounded;
    public Transform feetPos;
    public float checkRadius = .3f;
    public LayerMask layerMask;
    float jumpTimeCounter;
    public float jumpTime = .35f;
    bool isJumping = false;

    float hor;
    public bool canMove = false;
    bool canTakeDamage = true;
    bool isHurting = false;
    bool isSliding = false;

    Rigidbody2D rigidBody;
    CapsuleCollider2D capsuleCollider;
    Animator animator;
    Health health;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        health = FindObjectOfType<Health>();

        speed = runSpeed;
    }

    private void Update()
    {
        if (canMove)
        {
            // Movement.
            hor = Input.GetAxis("Horizontal");

            if (hor > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (hor < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            rigidBody.velocity = new Vector2(hor * speed, rigidBody.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));



            // Slide input.
            if (Input.GetKeyDown(KeyCode.LeftControl) && !isHurting && isGrounded)
            {
                StartCoroutine(Slide());
            }



            // Jump.
            isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, layerMask);

            if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isHurting && !isSliding)
            {
                speed = jumpSpeed;
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rigidBody.AddForce(Vector2.up * jumpForce);
            }
            if (Input.GetKey(KeyCode.Space) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    rigidBody.AddForce(Vector2.up * jumpForce / 100);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    speed = runSpeed;
                    isJumping = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                speed = runSpeed;
                isJumping = false;
            }



            // In air animations.
            if (!isGrounded && !isJumping)
            {
                animator.SetBool("IsFalling", true);
            }
            else if (!isGrounded && isJumping)
            {
                animator.SetBool("IsJumping", true);
            }
            else if (isGrounded)
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }



            // Dead.
            if (health.lives <= 0)
            {
                Dead();
            }
        }
    }

    // Colisions.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canTakeDamage)
        {
            if (other.CompareTag("Cactus"))
            {
                StartCoroutine(TakeDamage());
            }

            if (other.CompareTag("Abyss"))
            {
                Dead();
            }
        }
    }

    void Dead()
    {
        canMove = false;
        animator.SetTrigger("Dead");
        health.RemoveLives();
    }

    void Revive(Transform CPTransform)
    {
        canMove = true;
        animator.SetTrigger("Dead");
        transform.position = CPTransform.position;
    }

    // Slide.
    IEnumerator Slide()
    {
        isSliding = true;
        speed = slideSpeed;
        animator.SetTrigger("Slide");
        capsuleCollider.offset = new Vector2(.3f, -.2f);
        capsuleCollider.size = new Vector2(1.5f, 1.9f);
        yield return new WaitForSeconds(.35f);

        isSliding = false;
        yield return new WaitForSeconds(.15f);

        speed = runSpeed;
        capsuleCollider.offset = new Vector2(.3f, .03f);
        capsuleCollider.size = new Vector2(1f, 2.25f);
        yield return null;
    }

    IEnumerator TakeDamage()
    {
        health.RemoveHealth(-40);
        speed = hurtSpeed;
        canTakeDamage = false;
        isHurting = true;
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(.55f);

        speed = runSpeed;
        isHurting = false;
        yield return new WaitForSeconds(.45f);

        canTakeDamage = true;
        yield return null;
    }
}
