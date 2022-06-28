using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    // Componente.
    Animator animator;
    // Script.
    LevelManager levelManager;
    // Variável.
    bool doOnce = true;
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
            // Executa a animação de colidiu, seta "canCount" falso e chama "LevelFinished()".
            doOnce = false;
            animator.SetTrigger("Collided");
            levelManager.canCount = false;
            levelManager.StartCoroutine(levelManager.LevelFinished());
        }
    }
}
