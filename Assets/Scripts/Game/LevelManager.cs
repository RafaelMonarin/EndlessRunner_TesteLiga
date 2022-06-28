using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Classe com os valores necessários para cada estrela.
[System.Serializable]
public class StarsRequirement
{
    public float timeNeeded;
    public TMP_Text timeNeededTXT;
    public int livesNeeded;
    public TMP_Text livesNeededTXT;
    public Image starsIMG;
}

public class LevelManager : MonoBehaviour
{
    public int levelNumber;

    // Scripts.
    Player player;
    Health health;

    // Variáveis.
    public GameObject stage1;
    public GameObject stage2;
    public TMP_Text timerTXT;
    public GameObject stage3;
    public GameObject pauseMenu;
    public TMP_Text levelNumberTxt;
    public GameObject[] inGameHud;
    public GameObject gameOver;
    public GameObject playerStatus;
    public Slider levelProgress;
    public TMP_Text levelTimer;
    public TMP_Text liveResult;
    public TMP_Text timeResult;
    public Sprite fullStar;
    public Sprite emptyStar;

    float time = 3;
    float levelTime = 0;
    float minutes;
    float seconds;
    bool doOnce = true;
    public bool canCount = false;

    public Transform startPos;
    public Transform currentCheckPoint;
    public Transform finish;

    int stars = 0;
    public StarsRequirement[] starsRequirements;

    private void Start()
    {
        // Adiciona os métodos aos eventos.
        Player.onPlayerDied += PlayerDied;
        RewardedAdsButton.onRewardedCompleted += NewLive;
        // Encontra os scripts.
        player = FindObjectOfType<Player>();
        health = FindObjectOfType<Health>();

        // Seta os texto de tempo e número do nível na hud, o primeiro checkpoint, e chama "SetStarsRequirements()".
        timerTXT.text = time.ToString();
        levelNumberTxt.text = "NÍVEL " + levelNumber;
        currentCheckPoint = startPos;
        SetStarsRequirements();
    }

    private void Update()
    {
        if (doOnce)
        {
            // Se apertar qualquer tecla, inicia o Coroutine "CountDown()".
            if (Input.anyKey)
            {
                doOnce = false;
                StartCoroutine(CountDown());
            }
        }

        // Se pode contar:
        if (canCount)
        {
            // Soma o tempo do nível e cham "DisplayTime()" massando o tempo.
            levelTime += Time.deltaTime;
            DisplayTime(levelTime);
        }

        // Variável float com o valor da distância horizontal do jogador até o começo, seta o valor máximo do slider de progresso para a distância entre o começo e o fim do nível, e defien o falor do slider com o progresso.
        float progress = Vector2.Distance(new Vector2(player.transform.position.x, 0), new Vector2(startPos.position.x, 0));
        levelProgress.maxValue = Vector2.Distance(startPos.position, finish.position);
        levelProgress.value = progress;
    }

    void SetStarsRequirements()
    {
        // Seta os requisitos para cada estrela na hud.
        for (int i = 0; i < starsRequirements.Length; i++)
        {
            float minutesR = Mathf.FloorToInt(starsRequirements[i].timeNeeded / 60);
            float secondsR = Mathf.FloorToInt(starsRequirements[i].timeNeeded % 60);
            starsRequirements[i].timeNeededTXT.text = string.Format("{0:00}:{1:00}", minutesR, secondsR);
            starsRequirements[i].livesNeededTXT.text = starsRequirements[i].livesNeeded.ToString();
        }
    }

    // Coroutine para contagem regressiva.
    IEnumerator CountDown()
    {
        // Desabilita o estágio 1 (hud de "aperte para iniciar") e habilita o estágio 2 (hud de "contagem regressiva").
        stage1.SetActive(false);
        stage2.SetActive(true);

        // Enquanto o tempo for maior que 0:
        while (time > 0)
        {
            // Muda o texto de contagem regressiva da hud, espera 1 segundo e diminui 1 do tempo.
            timerTXT.text = time.ToString();
            yield return new WaitForSeconds(1);

            time--;
        }

        // Muda o texto da contatem regressiva para "VAI!", seta "canCount" verdadeiro, habilita a movimentação dos scripts e espera 1 segundo.
        timerTXT.text = "VAI!";
        canCount = true;
        player.playerMovementMobile.canMove = true;
        player.playerMovementPC.canMove = true;
        yield return new WaitForSeconds(1);

        // Desativa o estágio 2 (hud de "contagem regressiva").
        stage2.SetActive(false);
        yield break;
    }

    // Evento chamado quando o jogador morre, inicia o Coroutine "Revive()".
    void PlayerDied()
    {
        StartCoroutine(Revive());
    }

    // Coroutine de reviver:
    IEnumerator Revive()
    {
        // Seta "canCount" falso e espera 1 segundo.
        canCount = false;
        yield return new WaitForSeconds(1);

        // Seta canCount verdadeiro e muda o transform do jogador para o transform o checkpoint.
        canCount = true;
        player.transform.position = currentCheckPoint.position;
        yield break;
    }

    // Método que pausa o jogo.
    public void Pause()
    {
        // Pausa o jogo, loop que desativa alguns elementos da hud e ativa o menu de pause.
        Time.timeScale = 0;
        
        for (int i = 0; i < inGameHud.Length; i++)
        {
            inGameHud[i].SetActive(false);
        }

        pauseMenu.SetActive(true);
    }

    // Método que despausa o jogo.
    public void Resume()
    {
        // Despausa o jogo, desativa o menu de pause, e habilida alguns elementos da hud.
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        for (int i = 0; i < inGameHud.Length; i++)
        {
            inGameHud[i].SetActive(true);
        }
    }

    // Coroutine de fim de jogo.
    public IEnumerator GameOver()
    {
        // Chama "RemoveLives()", desabilita alguns elementos da hud, desabilita os controles da hud e habilita a janela de fim de jogo na hud e pausa o jogo.
        health.RemoveLives();
        for (int i = 0; i < inGameHud.Length; i++)
        {
            inGameHud[i].SetActive(false);
        }
        player.controls.SetActive(false);
        gameOver.SetActive(true);
        Time.timeScale = 0;
        yield break;
    }

    // Método que carrega a cena do menu inicial.
    public void LoadStartMenu()
    {
        // Despausa o jogo e carrega a cena do menu inicial.
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }

    // Método que recarrega a cena.
    public void ReloadLevel()
    {
        // Despausa o jogo e carrega a mesma cena.
        Time.timeScale = 1;
        SceneManager.LoadScene("Level " + levelNumber);
    }

    // Método que carrega a próxima cena.
    public void LoadNextLevel()
    {
        // Despausa o jogo e carrega a próxima cena.
        Time.timeScale = 1;
        SceneManager.LoadScene("Level " + (levelNumber + 1));
    }

    // Método que mostra o tempo na tela.
    void DisplayTime(float time)
    {
        // Se o tempo for menor que 0, seta ele igual a 0.
        if (time < 0)
        {
            time = 0;
        }
        // Caso contrário, se o tempo for maior que 0, adiociona 1 ao tempo. 
        else if (time > 0)
        {
            time += 1;
        }

        // Seta os valores de minutos e segundos do tempo.
        minutes = Mathf.FloorToInt(time / 60);
        seconds = Mathf.FloorToInt(time % 60);

        // Mostra na tela o tempo com o formato de minuto e segundos (MM:SS).
        levelTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Coroutine que termina o jogo.
    public IEnumerator LevelFinished()
    {
        // Seta "canCount" falso, chama "FinishAnim()", chama "Results()" e espera 2 segundos.
        canCount = false;
        player.playerMovementMobile.FinishAnim();
        player.playerMovementPC.FinishAnim();
        Results();
        yield return new WaitForSeconds(2);

        // Desativa alguns elementos da hud, desativa a barra de progresso, o tempo do nível e os status do player na hud, chama "SaveData()" e ativa o estágio 3 (Resultados do jogo).
        for (int i = 0; i < inGameHud.Length; i++)
        {
            inGameHud[i].SetActive(false);
        }
        levelProgress.gameObject.SetActive(false);
        levelTimer.gameObject.SetActive(false);
        playerStatus.SetActive(false);
        SaveData();
        stage3.SetActive(true);
        yield break;
    }

    // Método que mostra os resultados no fim do jogo.
    void Results()
    {
        // Mostra na tela o número de vidas restantes (corações) e o tempo levado.
        liveResult.text = health.lives.ToString();
        timeResult.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        stars = 0;

        // Loop que verifica os requisitos para cara estrela.
        for (int i = 0; i < starsRequirements.Length; i++)
        {
            // Se o número de vidas (corações) for maior ou igual ao necessário, e o tempo for menor ou igual a onecessário:
            if (health.lives >= starsRequirements[i].livesNeeded && levelTime <= starsRequirements[i].timeNeeded)
            {
                // Adiociona 1 estrela.
                stars++;
            }
            // Se o número de estrelas salvo no PlayerPrefs for menor ou igual ao número de estrelas do nível:
            if (PlayerPrefs.GetInt("Level" + levelNumber + "Stars") <= stars)
            {
                // Seta o novo número de estrelas no PlayerPrefs.
                PlayerPrefs.SetInt("Level" + levelNumber + "Stars", stars);
            }
            // Muda o sprite de estrelas da hud de acordo com o número de estrelas conseguidas. 
            starsRequirements[i].starsIMG.sprite = i < PlayerPrefs.GetInt("Level" + levelNumber + "Stars") ? fullStar : emptyStar;
        }
    }

    // Método que salva os dados no PlayerPrefs.
    void SaveData()
    {
        // Variável para o número de níveis desbloqueados.
        int levelsUnlocked;
        // Se o valor do PlayerPrefs for "no" (nunca completou o nível):
        if (PlayerPrefs.GetString("Level" + levelNumber + "Completed") == "no")
        {
            // Muda o valor do PlayerPrefs para "yes" (já completou o nível), pega o número de níveis desbloqueados e adiociona 1 (Desbloqueia outro nível), seta esse valor no PlayerPrefs e salva.
            PlayerPrefs.SetString("Level" + levelNumber + "Completed", "yes");
            levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked");
            levelsUnlocked++;
            PlayerPrefs.SetInt("LevelsUnlocked", levelsUnlocked);
            PlayerPrefs.Save();

        }
    }

    // Método chamado pelo evento.
    void NewLive()
    {
        // Despausa o jogo e inicia o Coroutine "NewLiveIEnum()".
        Time.timeScale = 1;
        StartCoroutine(NewLiveIEnum());
    }

    // Coroutine que revive o jogador depois do Ads de reward.
    IEnumerator NewLiveIEnum()
    {
        // Desativa a janela de fim de jogo, ativa alguns elementos da hud, ativa os controles da hud e muda o transform do jogador para o transform do checkpoint.
        gameOver.SetActive(false);
        for (int i = 0; i < inGameHud.Length; i++)
        {
            inGameHud[i].SetActive(true);
        }
        player.controls.SetActive(true);
        player.transform.position = currentCheckPoint.position;
        yield break;
    }
}
