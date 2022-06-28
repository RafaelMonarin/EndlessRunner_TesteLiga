using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Enum.
    public enum MobileOrPC { Mobile, PC }
    public MobileOrPC mobileOrPC;
    public GameObject controls;

    // Variáveis.
    bool canTakeDamage = true;
    bool doOnce = true;
    bool gameOver = false;

    // Componentes.
    Animator animator;
    Health health;
    LevelManager levelManager;

    // Scripts para movimentação no Mobile e PC.
    public PlayerMovementMobile playerMovementMobile;
    public PlayerMovementPC playerMovementPC;

    // Eventos.
    public delegate void PlayerDeath();
    public static event PlayerDeath onPlayerDied;

    private void Awake()
    {
        onPlayerDied = null;
    }

    private void Start()
    {   
        // Adiociona os métodos aos eventos.
        onPlayerDied += PlayerDied;
        RewardedAdsButton.onRewardedCompleted += NewLive;

        // Pega os componentes.
        animator = GetComponent<Animator>();
        health = FindObjectOfType<Health>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        // Se o enum for:
        switch (mobileOrPC)
        {
            // Mobile:
            case MobileOrPC.Mobile:
                // Ativa o script do Mobile, desativa do PC e ativa os controles na hud.
                playerMovementMobile.enabled = true;
                playerMovementPC.enabled = false;
                controls.SetActive(true);
                break;

            // PC:
            case MobileOrPC.PC:
                // Desativa o script do Mobile, ativa do PC e desativa os controles na hud.
                playerMovementMobile.enabled = false;
                playerMovementPC.enabled = true;
                controls.SetActive(false);
                break;
        }

        // Se o jogador pode se mover:
        if (playerMovementMobile.canMove)
        {
            // Se a vida for menor ou igual a 0, chama "Died()";
            if (health.health <= 0 && doOnce)
            {   
                Died();
            }
        }
    }

    // Para o jogador.
    public void StopPlayer()
    {
        // Para o script de movimento que estiver ativo.
        if (playerMovementMobile.enabled)
        {
            playerMovementMobile.StopPlayer();
        }
        else if (playerMovementPC.enabled)
        {
            playerMovementPC.StopPlayer();
        }
    }

    // Ao colidir com algo isTrigger:
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se pode tomar dano:
        if (canTakeDamage)
        {
            // Se a tag do objeto for "Cactus", inicia o Coroutine "Takedamage()".
            if (other.CompareTag("Cactus"))
            {
               StartCoroutine(TakeDamage());
            }

            // Se a tag do objeto for "Abyss", zera a vida e chama o método "UpdateHealhLives()" que atualiza a hud.
            if (other.CompareTag("Abyss"))
            {
                health.health = 0;
                health.UpdateHealhLives();
            }
        }
    }

    // ao permanecer em uma colisão isTrigger:
    void OnTriggerStay2D(Collider2D other)
    {
        // Se pode tomar dano:
        if (canTakeDamage)
        {
            // Se a tag do objeto for "Cactus", inicia o Coroutine "Takedamage()".
            if (other.CompareTag("Cactus"))
            {
                StartCoroutine(TakeDamage());
            }
        }
    }

    // Método quando o jogador morre.
    public void Died()
    {
        // Se a vida (corações) for maior que 1:
        if (health.lives > 1)
        {
            // Se o evento não for nulo, chama o evento "onPlayerDied()".
            if (onPlayerDied != null)
            {
                onPlayerDied();
            }
        }
        // Caso contrário, inicia o Coroutine "GameOver()". 
        else
        {
            StartCoroutine(GameOver());
        }
    }

    // Coroutine para fim de jogo.
    IEnumerator GameOver()
    {
        // Seta a variável de "gameOver" para verdadeira, "canTakeDamage" para falsa, executa a animação de morte, chama "StopPlayer" e espera 1 segundo.
        gameOver = true;
        canTakeDamage = false;
        animator.SetTrigger("Dead");
        StopPlayer();
        yield return new WaitForSeconds(1);

        // Inicia o Coroutine de Gameover.
        levelManager.StartCoroutine(levelManager.GameOver());
        yield break;
    }

    // Método chamado pelo evento quando o jogador morre.
    void PlayerDied()
    {
        doOnce = false;
        // Se "GameOver" for falso, inicia o Coroutine "Revive()".
        if (!gameOver)
        {
            StartCoroutine(Revive());
        }
    }

    // Coroutine para reviver.
    IEnumerator Revive()
    {
        // Chama "StopPlayer()", executa a animação de morte, seta a variável "canTakeDamage" falsa e espera 1 segundo.
        StopPlayer();
        animator.SetTrigger("Dead");
        canTakeDamage = false;
        yield return new WaitForSeconds(1f);

        // Executa a animação de reviver, termina a animação de caindo, executa a animação "Speed" com valor 0 e espera 1 segundo.
        animator.SetTrigger("Revive");
        animator.SetBool("IsFalling", false);
        animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(1f);

        // Habilita o movimento dos scripts e espera .25 segundos.
        playerMovementMobile.canMove = true;
        playerMovementPC.canMove = true;
        doOnce = true;
        yield return new WaitForSeconds(.25f);

        // Seta a variável "canTakeDamage" verdadeira.
        canTakeDamage = true;
        yield break;
    }

    // Coroutine para tomar dano.
    IEnumerator TakeDamage()
    {
        // Seta "canTakeDamage" falsa, remove 40 de vida, seta a velodidade de machucado, muda o estado para machucando, executa a animação de machucado e espera .55 segundos.
        canTakeDamage = false;
        health.RemoveHealth(-40);
        playerMovementMobile.speed = playerMovementMobile.hurtSpeed;
        playerMovementPC.speed = playerMovementPC.hurtSpeed;
        playerMovementMobile.playerState = PlayerMovementMobile.PlayerState.Hurting;
        playerMovementPC.playerState = PlayerMovementPC.PlayerState.Hurting;
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(.55f);

        // Seta a velodidade de correr, muda o estado para normal e espera .45 segundos.
        playerMovementMobile.speed = playerMovementMobile.runSpeed;
        playerMovementPC.speed = playerMovementPC.runSpeed;
        playerMovementMobile.playerState = PlayerMovementMobile.PlayerState.Normal;
        playerMovementPC.playerState = PlayerMovementPC.PlayerState.Normal;
        yield return new WaitForSeconds(.45f);

        // Seta a variável "canTakeDamage" falsa.
        canTakeDamage = true;
        yield break;
    }

    // Método chamado pelo evento, inicia o Coroutine "NewLiveIEnum()".
    void NewLive()
    {
        StartCoroutine(NewLiveIEnum());
    }

    // Coroutine que revive o jogador depois do Ads de reward.
    IEnumerator NewLiveIEnum()
    {
        // Executa a animação de reviver e espera 1 segundo.
        animator.SetTrigger("Revive");
        yield return new WaitForSeconds(1f);

        // Desabilita a movimentação dos scripts e seta a variável "canTakeDamage" verdadeira.
        playerMovementMobile.canMove = true;
        playerMovementPC.canMove = true;
        canTakeDamage = true;
        doOnce = true;
        yield break;
    }
}
