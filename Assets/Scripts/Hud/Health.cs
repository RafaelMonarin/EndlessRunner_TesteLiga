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
        SetLives();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RemoveLives();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            RemoveHealth(-40);
        }
    }

    public void RemoveHealth(float value)
    {
        health += value;
        healthBar.fillAmount = health / maxHealth;
        if (health <= 0)
        {
            RemoveLives();
            health = maxHealth;
            healthBar.fillAmount = health / maxHealth;
        }
    }

    public void RemoveLives()
    {
        lives--;
        SetLives();
    }

    public void SetLives()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < lives ? fullHeart : emptyHeart;
        }
    }

    public void ResetAll()
    {
        health = maxHealth;
        healthBar.fillAmount = health / maxHealth;
        lives = 3;
        SetLives();
    }
}
