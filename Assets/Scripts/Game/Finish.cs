using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    Animator animator;
    LevelManager levelManager;
    bool doOnce = true;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && doOnce)
        {
            doOnce = false;
            animator.SetTrigger("Collided");
            levelManager.canCount = false;
            levelManager.StartCoroutine(levelManager.LevelFinished());
        }
    }
}
