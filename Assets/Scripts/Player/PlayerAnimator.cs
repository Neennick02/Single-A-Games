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
        _armAnimator.SetBool("Attack", true);
        _handAnimator.SetFloat("Attack", 1f);

        yield return new WaitForSeconds(0.2f);

        _armAnimator.SetBool("Attack", false);

        while (_handAnimator.GetFloat("Attack") > 0)
        {
            _handAnimator.SetFloat("Attack", _handAnimator.GetFloat("Attack") - 6f * Time.deltaTime);

            if (_handAnimator.GetFloat("time") < 0)
            {
                _handAnimator.SetFloat("Attack", 0f);
            }

            yield return new WaitForSeconds(0.01f);
        }

    }
}
