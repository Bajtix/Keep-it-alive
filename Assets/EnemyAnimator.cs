using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;

    private Enemy enemy;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        animator.SetFloat("Speed",enemy.agent.velocity.magnitude);
        animator.SetBool("Reload", enemy.reloading);
        animator.SetBool("Aiming", enemy.status == Enemy.AttentionStatus.Battle);
    }
}
