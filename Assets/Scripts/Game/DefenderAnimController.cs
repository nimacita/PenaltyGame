using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderAnimController : MonoBehaviour
{

    [Header("Main Settings")]
    public Vector2 delayRange;
    public Animator animator;
    public AnimationClip[] idleClips;
    public AnimationClip headHit;

    private void OnEnable()
    {
        GameController.onRestartedField += StartAnim;
    }

    private void OnDisable()
    {
        GameController.onRestartedField -= StartAnim;
    }

    private void StartAnim()
    {
        StartCoroutine(GoRandAnim());
    }

    private IEnumerator GoRandAnim()
    {
        float randDelay = Random.Range(delayRange.x, delayRange.y);
        yield return new WaitForSeconds(randDelay);
        //играем анимацию случайную
        RandAnim();
    }

    private void RandAnim()
    {
        int rand = Random.Range(0, idleClips.Length);
        animator.Play(idleClips[rand].name);
    }

    public void HeadHit()
    {
        animator.Play(headHit.name);
    }
}
