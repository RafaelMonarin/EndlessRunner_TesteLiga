using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Animator animator;
    LevelManager levelManager;
    bool doOnce = true;
    public Transform cpTransform;

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
            levelManager.currentCheckPoint = cpTransform;
        }
    }
}
