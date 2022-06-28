using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementMobile : MonoBehaviour
{
    // Enum.
    public enum MovementState { Running, Sliding, Jumping }
    public enum PlayerState { Normal, Hurting }

    public MovementState movementState;
    public PlayerState playerState;

    // Variáveis.
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

    // Componentes.
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    Animator animator;

    public Joystick joystick;

    private void Start()
    {
        // Seta os estados para correndo e normal.
        movementState = MovementState.Running;
        playerState = PlayerState.Normal;

        // Pega os componentes.
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        // Seta a velocidade de corrida.
        speed = runSpeed;
    }

    private void Update()
    {
        // Se pode mover:
        if (canMove)
        {
            // Input do Joystick.
            if (joystick.Horizontal >= .2f)
            {
                horizontalMove = speed;
            }
            else if (joystick.Horizontal <= -.2f)
            {
                horizontalMove = -speed;
            }
            else
            {
                horizontalMove = 0;
            }

            // Rotaciona o jogador baseado no valor do Input do Joystick.
            if (horizontalMove > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (horizontalMove < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            // Seta a velocidade baseado no Input do Joystick, e executa a animação de correr.
            rigidBody.velocity = new Vector2(horizontalMove, rigidBody.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

            // Variável para cada pé que detecta se está colidindo com o chão.
            isGrounded1 = Physics2D.OverlapCircle(feetPos1.position, checkRadius, layerMask);
            isGrounded2 = Physics2D.OverlapCircle(feetPos2.position, checkRadius, layerMask);

            // Se tiver colidindo com o chão:
            if ((isGrounded1 || isGrounded2) && doOnce)
            {
                // Muda o estado para correndo e seta a velocidade de corrida.
                doOnce = false;
                movementState = MovementState.Running;
                speed = runSpeed;
            }
            // Se a velocidade vertical for menor que -5 (caindo) e estiver correndo:
            if (rigidBody.velocity.y < -5 && movementState == MovementState.Running)
            {
                // Executa a animação de cair.
                animator.SetBool("IsFalling", true);
            }
            // Caso contrário se a velocidade vertical for maior que 5 (pulando):
            else if (rigidBody.velocity.y > 5)
            {
                // Executa a animação de pulo.
                animator.SetBool("IsJumping", true);
                doOnce = true;
            }
            // Caso contrário se só estiver encostando no chão:
            else if (isGrounded1 || isGrounded2)
            {
                // Termina a animação de pulo e caindo.
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }
        }
    }

    // Para o jogador setando a variável falsa e zerando a velocidade.
    public void StopPlayer()
    {
        canMove = false;
        rigidBody.velocity = Vector2.zero;
    }

    // Para o jogador setando a variável falsa, zerando a velocidade e executando a animação "Speed" com valor 0.
    public void FinishAnim()
    {
        canMove = false;
        rigidBody.velocity = Vector2.zero;
        animator.SetFloat("Speed", 0);
    }

    // Pula.
    public void Jump()
    {
        // Se pode mover:
        if (canMove)
        {
            // Se estiver no chão, o player estiver normal e correndo:
            if ((isGrounded1 || isGrounded2) && playerState == PlayerState.Normal && movementState == MovementState.Running)
            {
                // Muda o estado para pulando e adiciona uma força para cima (faz pular).
                movementState = MovementState.Jumping;
                rigidBody.AddForce(Vector2.up * jumpForce);
            }
        }
    }

    // Escorrega.
    public void Slide()
    {
        // Se estiver no chão, o player estiver normal e correndo:
        if ((isGrounded1 || isGrounded2) && playerState == PlayerState.Normal && movementState == MovementState.Running)
        {
            // Inicia o Coroutine "SlideEnum()".
            StartCoroutine(SlideIEnum());
        }
    }

    // Coroutine para escorregar "SlideEnum()"..
    IEnumerator SlideIEnum()
    {
        // Muda o estado para escorregando, seta a velocidade de escorregar, executa a animação de escorregar, diminui o colisor e espera .4 segundos.
        movementState = MovementState.Sliding;
        speed = slideSpeed;
        animator.SetTrigger("Slide");
        boxCollider.offset = new Vector2(.3f, -.1f);
        boxCollider.size = new Vector2(1f, 1.25f);
        yield return new WaitForSeconds(.4f);

        // Muda o estado para correndo e espera .1 segundos.
        movementState = MovementState.Running;
        yield return new WaitForSeconds(.1f);

        // Seta a velocidade de corrida e volta a colisão ao normal.
        speed = runSpeed;
        boxCollider.offset = new Vector2(.3f, .1f);
        boxCollider.size = new Vector2(1f, 2f);
        yield break;
    }
}
