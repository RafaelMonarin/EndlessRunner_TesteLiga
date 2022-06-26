using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum MobileOrPC { Mobile, PC }
    public MobileOrPC mobileOrPC;
    public GameObject controls;

    bool canTakeDamage = true;
    bool doOnce = true;

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
        switch (mobileOrPC)
        {
            case MobileOrPC.Mobile:
                playerMovementMobile.enabled = true;
                playerMovementPC.enabled = false;
                controls.SetActive(true);
                break;

            case MobileOrPC.PC:
                playerMovementMobile.enabled = false;
                playerMovementPC.enabled = true;
                controls.SetActive(false);
                break;
        }

        if (playerMovementMobile.canMove)
        {
            if (health.health <= 0 && doOnce)
            {
                Died();
            }
        }
    }

    public void StopPlayer()
    {
        if (playerMovementMobile.enabled)
        {
            playerMovementMobile.StopPlayer();
        }
        else if (playerMovementPC.enabled)
        {
            playerMovementPC.StopPlayer();
        }
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
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (canTakeDamage)
        {
            if (other.CompareTag("Cactus"))
            {
                StartCoroutine(TakeDamage());
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
        doOnce = false;
        StartCoroutine(Revive());
    }

    IEnumerator Revive()
    {
        StopPlayer();
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Revive");
        yield return new WaitForSeconds(.5f);
        playerMovementMobile.canMove = true;
        playerMovementPC.canMove = true;
        doOnce = true;
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
