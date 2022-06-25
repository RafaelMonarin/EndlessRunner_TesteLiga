using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementPC : MonoBehaviour
{
    public enum MovementState { Idle, Running, Sliding, Jumping }
    public enum PlayerState { Normal, Hurting }

    public MovementState movementState;
    public PlayerState playerState;

    public float speed;
    public float runSpeed = 8;
    public float slideSpeed = 15;
    public float jumpSpeed = 5;
    public float hurtSpeed = 2;

    public bool isGrounded = true;
    public float jumpForce = 1000;
    public Transform feetPos;
    public float checkRadius = .3f;
    public LayerMask layerMask;

    float horizontalMove;
    public bool canMove = true;
    bool doOnce = true;

    Rigidbody2D rigidBody;
    CapsuleCollider2D capsuleCollider;
    Animator animator;

    private void Start()
    {
        movementState = MovementState.Idle;
        playerState = PlayerState.Normal;

        rigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
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
                movementState = MovementState.Running;
            }
            else if (horizontalMove < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                movementState = MovementState.Running;
            }
            else
            {
                movementState = MovementState.Idle;
            }

            rigidBody.velocity = new Vector2(horizontalMove, rigidBody.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

            isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, layerMask);
            if (isGrounded && doOnce)
            {
                doOnce = false;
                movementState = MovementState.Idle;
                speed = runSpeed;
            }

            if (rigidBody.velocity.y < -5 && (movementState == MovementState.Idle || movementState == MovementState.Running))
            {
                animator.SetBool("IsFalling", true);
            }
            else if (rigidBody.velocity.y >= 5)
            {
                animator.SetBool("IsJumping", true);
            }
            else if (isGrounded)
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded && playerState == PlayerState.Normal && (movementState == MovementState.Idle || movementState == MovementState.Running))
                {
                    doOnce = true;
                    speed = jumpSpeed;
                    movementState = MovementState.Jumping;
                    rigidBody.AddForce(Vector2.up * jumpForce);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (isGrounded && playerState == PlayerState.Normal && movementState == MovementState.Running)
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

    IEnumerator SlideIEnum()
    {
        movementState = MovementState.Sliding;
        speed = slideSpeed;
        animator.SetTrigger("Slide");
        capsuleCollider.offset = new Vector2(.3f, -.2f);
        capsuleCollider.size = new Vector2(1.5f, 1.9f);
        yield return new WaitForSeconds(.4f);

        movementState = MovementState.Idle;
        yield return new WaitForSeconds(.1f);

        speed = runSpeed;
        capsuleCollider.offset = new Vector2(.3f, .03f);
        capsuleCollider.size = new Vector2(1f, 2.25f);
        yield break;
    }
}
