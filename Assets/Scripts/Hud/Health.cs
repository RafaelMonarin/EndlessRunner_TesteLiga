using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100;
    public float maxHealth = 100;
    public Image healthBar;

    public int lives = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Start()
    {
        Player.onPlayerDied += PlayerDied;

        ResetAll();
    }

    public void RemoveHealth(float value)
    {
        health += value;
        healthBar.fillAmount = health / maxHealth;
    }

    public void RemoveLives()
    {
        lives--;
        UpdateHealhLives();
    }

    public void UpdateHealhLives()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < lives ? fullHeart : emptyHeart;
        }

        healthBar.fillAmount = health / maxHealth;
    }

    void PlayerDied()
    {
        StartCoroutine(Revive());
    }

    public void ResetAll()
    {
        health = maxHealth;
        lives = 3;
        UpdateHealhLives();
    }

    IEnumerator Revive()
    {
        UpdateHealhLives();
        if (health == 0)
        {
            RemoveLives();
        }
        yield return new WaitForSeconds(1.5f);

        if (health < 0)
        {
            RemoveLives();
        }
        health = maxHealth;
        UpdateHealhLives();
        yield break;
    }
}
