using System;
using UnityEngine;
using Unity.VisualScripting;

public class BoxOpeningController : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Animator animator; 
    private Action onComplete;

    public void Play(Action onFinished)
    {
        onComplete = onFinished;
        root.SetActive(true);

        animator.Play("Porco dio", 0);
    }

    public void OnAnimationFinished()
    {
        root.SetActive(false);
        onComplete?.Invoke();
    }
}