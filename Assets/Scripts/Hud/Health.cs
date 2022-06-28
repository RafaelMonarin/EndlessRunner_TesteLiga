using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Vari�veis.
    public float health = 100;
    public float maxHealth = 100;
    public Image healthBar;

    public int lives = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Start()
    {
        // Adiociona os m�todos aos eventos.
        Player.onPlayerDied += PlayerDied;
        RewardedAdsButton.onRewardedCompleted += NewLive;

        // Chama "ResetAll()".
        ResetAll();
    }

    // M�todo que remove sa�de, recebe um valor (dano).
    public void RemoveHealth(float value)
    {
        // Adiociona o valor ao n�mero de sa�de e atualiza a barra de vida da hud.
        health += value;
        healthBar.fillAmount = health / maxHealth;
    }

    // M�todo que remove vida (cora��es).
    public void RemoveLives()
    {
        // Diminui 1 na vida e chama "UpdateHealhLives()".
        lives--;
        UpdateHealhLives();
    }

    // M�todo que atualiza a hud.
    public void UpdateHealhLives()
    {
        // Loop que troca os sprites das imagens de cora��es na hud de acordo com o n�mero de vida (cora��es).
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < lives ? fullHeart : emptyHeart;
        }

        // Atualiza a barra de sa�de na hud.
        healthBar.fillAmount = health / maxHealth;
    }

    // M�todo chamado pelo evento quando o jogador morre, inicia o Coroutine "Revive()".
    void PlayerDied()
    {
        StartCoroutine(Revive());
    }

    // M�todo que reseta toda a vida e sa�de.
    public void ResetAll()
    {
        // Reseta os valores de sa�de, vida e chama "UpdateHealhLives()".
        health = maxHealth;
        lives = 3;
        UpdateHealhLives();
    }

    // Coroutine que revive o jogador.
    IEnumerator Revive()
    {
        // Se sa�de for menor ou igual a 0, chama "RemoveLives()" e espera 1 segundo.
        if (health <= 0)
        {
            RemoveLives();
        }
        yield return new WaitForSeconds(1);

        // Reseta o valor da sa�de e chama "UpdateHealhLives()".
        health = maxHealth;
        UpdateHealhLives();
        yield break;
    }

    // M�todo chamado pelo evento do Ads de reward.
    void NewLive()
    {
        // Seta "lives" igual a 1 (ganha uma vida), reseta o n�mero de vida e chama "UpdateHealhLives()".
        lives = 1;
        health = maxHealth;
        UpdateHealhLives();
    }
}
