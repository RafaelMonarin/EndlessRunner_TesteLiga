using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Componente.
    Animator animator;
    // Script.
    LevelManager levelManager;
    // Vari�veis.
    bool doOnce = true;
    public Transform cpTransform;

    private void Start()
    {
        // Pega o componente e o script.
        animator = GetComponentInChildren<Animator>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Quando colidir com um objeto isTrigger:
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se a tag do objeto for "Player":
        if (other.CompareTag("Player") && doOnce)
        {
            // Executa a anima��o de colidiu e passa o transform do checkpoint para "currentCheckPoint".
            doOnce = false;
            animator.SetTrigger("Collided");
            levelManager.currentCheckPoint = cpTransform;
        }
    }
}
