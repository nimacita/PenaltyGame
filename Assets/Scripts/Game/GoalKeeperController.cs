using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeperController : MonoBehaviour
{

    [Header("Main Settings")]
    [SerializeField] private Transform rightBorder;
    [SerializeField] private Transform leftBorder;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip headHit;
    [SerializeField] private AnimationClip onHeadBlock;
    [SerializeField] private AnimationClip rightBlock;
    [SerializeField] private AnimationClip leftBlock;

    //ставим на новую случайную позицию
    public void SetNewKeeperPos()
    {
        Vector3 newPos = transform.position;

        float newX = Random.Range(rightBorder.position.x, leftBorder.position.x);
        newPos.x = newX;

        transform.position = newPos;
    }

    public void HeadHit()
    {
        animator.Play(headHit.name);
    }

    public void OnHeadBlock()
    {
        animator.Play(onHeadBlock.name);
    }

    public void RightBlock()
    {
        animator.Play(rightBlock.name);
    }

    public void LeftBlock()
    {
        animator.Play(leftBlock.name);
    }
}
