using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Variáveis.
    public float health = 100;
    public float maxHealth = 100;
    public Image healthBar;

    public int lives = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Start()
    {
        // Adiociona os métodos aos eventos.
        Player.onPlayerDied += PlayerDied;
        RewardedAdsButton.onRewardedCompleted += NewLive;

        // Chama "ResetAll()".
        ResetAll();
    }

    // Método que remove saúde, recebe um valor (dano).
    public void RemoveHealth(float value)
    {
        // Adiociona o valor ao número de saúde e atualiza a barra de vida da hud.
        health += value;
        healthBar.fillAmount = health / maxHealth;
    }

    // Método que remove vida (corações).
    public void RemoveLives()
    {
        // Diminui 1 na vida e chama "UpdateHealhLives()".
        lives--;
        UpdateHealhLives();
    }

    // Método que atualiza a hud.
    public void UpdateHealhLives()
    {
        // Loop que troca os sprites das imagens de corações na hud de acordo com o número de vida (corações).
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < lives ? fullHeart : emptyHeart;
        }

        // Atualiza a barra de saúde na hud.
        healthBar.fillAmount = health / maxHealth;
    }

    // Método chamado pelo evento quando o jogador morre, inicia o Coroutine "Revive()".
    void PlayerDied()
    {
        StartCoroutine(Revive());
    }

    // Método que reseta toda a vida e saúde.
    public void ResetAll()
    {
        // Reseta os valores de saúde, vida e chama "UpdateHealhLives()".
        health = maxHealth;
        lives = 3;
        UpdateHealhLives();
    }

    // Coroutine que revive o jogador.
    IEnumerator Revive()
    {
        // Se saúde for menor ou igual a 0, chama "RemoveLives()" e espera 1 segundo.
        if (health <= 0)
        {
            RemoveLives();
        }
        yield return new WaitForSeconds(1);

        // Reseta o valor da saúde e chama "UpdateHealhLives()".
        health = maxHealth;
        UpdateHealhLives();
        yield break;
    }

    // Método chamado pelo evento do Ads de reward.
    void NewLive()
    {
        // Seta "lives" igual a 1 (ganha uma vida), reseta o número de vida e chama "UpdateHealhLives()".
        lives = 1;
        health = maxHealth;
        UpdateHealhLives();
    }
}
