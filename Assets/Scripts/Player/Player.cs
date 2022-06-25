using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    bool canTakeDamage = true;

    Animator animator;
    Health health;
    LevelManager levelManager;

    public PlayerMovementMobile playerMovementMobile;
    public PlayerMovementPC playerMovementPC;

    public delegate void PlayerDeath();
    public static event PlayerDeath onPlayerDied;

    private void Awake()
    {
        onPlayerDied = null;
    }

    private void Start()
    {
        onPlayerDied += PlayerDied;

        animator = GetComponent<Animator>();
        health = FindObjectOfType<Health>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (playerMovementMobile.canMove)
        {
            if (health.health <= 0)
            {
                Died();
            }
        }
    }

    public void StopPlayer()
    {
        playerMovementMobile.StopPlayer();
        playerMovementPC.StopPlayer();
    }

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
                health.health = 0;
                health.UpdateHealhLives();
                Died();
            }
        }
    }

    public void Died()
    {
        if (health.lives > 0)
        {
            if (onPlayerDied != null)
            {
                onPlayerDied();
            }
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        animator.SetTrigger("Dead");
        StopPlayer();
        yield return new WaitForSeconds(1);

        levelManager.StartCoroutine(levelManager.GameOver());
        yield break;
    }

    void PlayerDied()
    {
        StartCoroutine(Revive());
    }

    IEnumerator Revive()
    {
        StopPlayer();
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(2);

        animator.SetTrigger("Revive");
        yield return new WaitForSeconds(.5f);

        playerMovementMobile.canMove = true;
        playerMovementPC.canMove = true;
        yield break;
    }

    IEnumerator TakeDamage()
    {
        canTakeDamage = false;
        health.RemoveHealth(-40);
        playerMovementMobile.speed = playerMovementMobile.hurtSpeed;
        playerMovementPC.speed = playerMovementPC.hurtSpeed;
        playerMovementMobile.playerState = PlayerMovementMobile.PlayerState.Hurting;
        playerMovementPC.playerState = PlayerMovementPC.PlayerState.Hurting;
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(.55f);

        playerMovementMobile.speed = playerMovementMobile.runSpeed;
        playerMovementPC.speed = playerMovementPC.runSpeed;
        playerMovementMobile.playerState = PlayerMovementMobile.PlayerState.Normal;
        playerMovementPC.playerState = PlayerMovementPC.PlayerState.Normal;
        yield return new WaitForSeconds(.45f);

        canTakeDamage = true;
        yield break;
    }
}
