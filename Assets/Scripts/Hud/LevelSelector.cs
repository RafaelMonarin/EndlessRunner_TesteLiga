using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Stars
{
    public Image[] starsIMG;
}

public class LevelSelector : MonoBehaviour
{
    public GameObject[] pages;
    public Image[] pageIndicator;
    public Sprite currentPage;
    public Sprite notCurentPage;
    int page = 0;

    public Button[] levels;
    public GameObject[] locked;
    public Stars[] stars;
    public Sprite fullStar;
    public Sprite emptyStar;

    private void Start()
    {
        SetPage();
        CheckLevels();
        CheckStars();
    }

    // Pages.
    public void NextPage()
    {
        if (page >= pages.Length - 1)
        {
            page = 0;
        }
        else
        {
            page++;
        }
        SetPage();
    }

    public void PreviousPage()
    {
        if (page <= 0)
        {
            page = pages.Length - 1;
        }
        else
        {
            page--;
        }
        SetPage();
    }

    void SetPage()
    {
        for (int i = 0; i < pageIndicator.Length; i++)
        {
            pageIndicator[i].sprite = i == page ? currentPage : notCurentPage;
        }

        CloseAllPages();
        pages[page].SetActive(true);
    }

    void CloseAllPages()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
    }

    public void ResetPages()
    {
        page = 0;
        SetPage();
    }



    // Levels.
    void CheckLevels()
    {
        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked");
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = false;
            locked[i].SetActive(true);
        }

        for (int i = 0; i < levelsUnlocked; i++)
        {
            if (i < levels.Length)
            {
                levels[i].interactable = true;
                locked[i].SetActive(false);
            }
        }
    }

    void CheckStars()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            for (int j = 0; j < stars[0].starsIMG.Length; j++)
            {
                stars[i].starsIMG[j].sprite = j < PlayerPrefs.GetInt("Level" + (i + 1) + "Stars") ? fullStar : emptyStar;
            }
        }
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
