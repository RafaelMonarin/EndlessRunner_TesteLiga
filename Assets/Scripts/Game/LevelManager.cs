using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    Player player;
    Health health;

    public GameObject stage1;
    public GameObject stage2;
    public TMP_Text timerTXT;
    public GameObject stage3;
    public GameObject pauseMenu;
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
    public int levelNumber;
    public StarsRequirement[] starsRequirements;

    private void Start()
    {
        Player.onPlayerDied += PlayerDied;

        player = FindObjectOfType<Player>();
        health = FindObjectOfType<Health>();
        timerTXT.text = time.ToString();
        currentCheckPoint = startPos;
        SetStarsRequirements();
    }

    private void Update()
    {
        if (doOnce)
        {
            if (Input.anyKey)
            {
                doOnce = false;
                StartCoroutine(CountDown());
            }
        }

        if (canCount)
        {
            levelTime += Time.deltaTime;
            DisplayTime(levelTime);
        }

        float progress = Vector2.Distance(player.transform.position, startPos.position);
        levelProgress.maxValue = Vector2.Distance(startPos.position, finish.position);
        levelProgress.value = progress;
    }

    void SetStarsRequirements()
    {
        for (int i = 0; i < starsRequirements.Length; i++)
        {
            float minutesR = Mathf.FloorToInt(starsRequirements[i].timeNeeded / 60);
            float secondsR = Mathf.FloorToInt(starsRequirements[i].timeNeeded % 60);
            starsRequirements[i].timeNeededTXT.text = string.Format("{0:00}:{1:00}", minutesR, secondsR);
            starsRequirements[i].livesNeededTXT.text = starsRequirements[i].livesNeeded.ToString();
        }
    }

    IEnumerator CountDown()
    {
        stage1.SetActive(false);
        stage2.SetActive(true);

        while (time > 0)
        {
            timerTXT.text = time.ToString();
            yield return new WaitForSeconds(1);

            time--;
        }

        timerTXT.text = "VAI!";
        canCount = true;
        player.playerMovementMobile.canMove = true;
        player.playerMovementPC.canMove = true;

        yield return new WaitForSeconds(1);

        stage2.SetActive(false);
        yield break;
    }

    void PlayerDied()
    {
        StartCoroutine(Revive());
    }

    IEnumerator Revive()
    {
        yield return new WaitForSeconds(1);

        player.transform.position = currentCheckPoint.position;
        yield break;
    }

    public void PauseMenu()
    {
        Time.timeScale = 0;
        
        for (int i = 0; i < inGameHud.Length; i++)
        {
            inGameHud[i].SetActive(false);
        }

        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        for (int i = 0; i < inGameHud.Length; i++)
        {
            inGameHud[i].SetActive(true);
        }
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < inGameHud.Length; i++)
        {
            inGameHud[i].SetActive(false);
        }
        gameOver.SetActive(true);
        Time.timeScale = 0;
        yield break;
    }

    public void LoadStartMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level " + levelNumber);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level " + (levelNumber + 1));
    }

    void DisplayTime(float time)
    {
        if (time < 0)
        {
            time = 0;
        }
        else if (time > 0)
        {
            time += 1;
        }

        minutes = Mathf.FloorToInt(time / 60);
        seconds = Mathf.FloorToInt(time % 60);

        levelTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public IEnumerator LevelFinished()
    {
        canCount = false;
        player.StopPlayer();
        Results();
        yield return new WaitForSeconds(2);

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

    void Results()
    {
        liveResult.text = health.lives.ToString();
        timeResult.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        stars = 0;
        for (int i = 0; i < starsRequirements.Length; i++)
        {
            if (health.lives >= starsRequirements[i].livesNeeded && levelTime <= starsRequirements[i].timeNeeded)
            {
                stars++;
            }
            if (PlayerPrefs.GetInt("Level" + levelNumber + "Stars") <= stars)
            {
                PlayerPrefs.SetInt("Level" + levelNumber + "Stars", stars);
            }
            starsRequirements[i].starsIMG.sprite = i < PlayerPrefs.GetInt("Level" + levelNumber + "Stars") ? fullStar : emptyStar;
        }
    }

    void SaveData()
    {
        int levelsUnlocked;
        if (PlayerPrefs.GetString("Level" + levelNumber + "Completed") == "no")
        {
            PlayerPrefs.SetString("Level" + levelNumber + "Completed", "yes");
            levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked");
            levelsUnlocked++;
            PlayerPrefs.SetInt("LevelsUnlocked", levelsUnlocked);
            PlayerPrefs.Save();

        }
    }
}
