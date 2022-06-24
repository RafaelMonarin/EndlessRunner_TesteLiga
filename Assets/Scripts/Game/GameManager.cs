using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    Player player;

    public GameObject stage1;
    public GameObject stage2;
    public TMP_Text timerTXT;
    public GameObject stage3;

    float time = 3;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        timerTXT.text = 3.ToString();
    }

    private void Update()
    {
        if (!player.canMove)
        {
            if (Input.anyKey)
            {
                StartCoroutine(CountDown());
            }
        }
    }

    IEnumerator CountDown()
    {
        stage1.SetActive(false);
        stage2.SetActive(true);
        yield return new WaitForSeconds(1);

        time -= Time.deltaTime;
        int timer = (int)time;

        if (timer >= 1)
        {
            timerTXT.text = timer.ToString();
        }
        else
        {
            timerTXT.text = "VAI!";
        }
        yield return new WaitForSeconds(2);

        player.canMove = true;
        yield return new WaitForSeconds(1);

        stage2.SetActive(false);
        yield return null;
    }
}
