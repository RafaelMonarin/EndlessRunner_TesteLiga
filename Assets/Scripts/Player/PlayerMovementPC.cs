using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementPC : MonoBehaviour
{
    public enum MovementState { Running, Sliding, Jumping }
    public enum PlayerState { Normal, Hurting }

    public MovementState movementState;
    public PlayerState playerState;

    public float speed;
    public float runSpeed = 8;
    public float slideSpeed = 15;
    public float hurtSpeed = 2;

    public bool isGrounded1 = true;
    public bool isGrounded2 = true;
    public float jumpForce = 1000;
    public Transform feetPos1, feetPos2;
    public float checkRadius = .3f;
    public LayerMask layerMask;

    float horizontalMove;
    public bool canMove = true;
    bool doOnce = true;

    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    Animator animator;

    private void Start()
    {
        movementState = MovementState.Running;
        playerState = PlayerState.Normal;

        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        speed = runSpeed;
    }

    private void Update()
    {
        if (canMove)
        {
            horizontalMove = Input.GetAxis("Horizontal") * speed;
            if (horizontalMove > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (horizontalMove < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            rigidBody.velocity = new Vector2(horizontalMove, rigidBody.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

            isGrounded1 = Physics2D.OverlapCircle(feetPos1.position, checkRadius, layerMask);
            isGrounded2 = Physics2D.OverlapCircle(feetPos2.position, checkRadius, layerMask);

            if ((isGrounded1 || isGrounded2) && doOnce)
            {
                doOnce = false;
                movementState = MovementState.Running;
                speed = runSpeed;
            }

            if (rigidBody.velocity.y < -5 && movementState == MovementState.Running)
            {
                animator.SetBool("IsFalling", true);
            }
            else if (rigidBody.velocity.y >= 5)
            {
                animator.SetBool("IsJumping", true);
                doOnce = true;
            }
            else if (isGrounded1 || isGrounded2)
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if ((isGrounded1 || isGrounded2) && playerState == PlayerState.Normal && movementState == MovementState.Running)
                {
                    movementState = MovementState.Jumping;
                    rigidBody.AddForce(Vector2.up * jumpForce);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if ((isGrounded1 || isGrounded2) && playerState == PlayerState.Normal && movementState == MovementState.Running)
                {
                    StartCoroutine(SlideIEnum());
                }
            }
        }
    }

    public void StopPlayer()
    {
        canMove = false;
        rigidBody.velocity = Vector2.zero;
    }

    public void FinishAnim()
    {
        canMove = false;
        rigidBody.velocity = Vector2.zero;
        animator.SetFloat("Speed", 0);
    }

    IEnumerator SlideIEnum()
    {
        movementState = MovementState.Sliding;
        speed = slideSpeed;
        animator.SetTrigger("Slide");
        boxCollider.offset = new Vector2(.3f, -.1f);
        boxCollider.size = new Vector2(1f, 1.25f);
        yield return new WaitForSeconds(.4f);

        movementState = MovementState.Running;
        yield return new WaitForSeconds(.1f);

        speed = runSpeed;
        boxCollider.offset = new Vector2(.3f, .1f);
        boxCollider.size = new Vector2(1f, 2f);
        yield break;
    }
}