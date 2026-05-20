using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _handAnimator;
    [SerializeField] private Animator _armAnimator;

    public void AttackAnimation()
    {
        StartCoroutine(AttackRoutine());

    }

    IEnumerator AttackRoutine()
    {
        _handAnimator.SetTrigger("Attack");

        _armAnimator.SetBool("Attack", true);

        yield return new WaitForSeconds(0.1f);
        _armAnimator.SetBool("Attack", false);
    }
}
