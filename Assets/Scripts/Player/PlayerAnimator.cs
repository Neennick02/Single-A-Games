using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _handAnimator = new List<GameObject>();
    [SerializeField] private Animator _armAnimator;

    public int state;
    private int _lastState;

    private void Start()
    {
        _lastState = state;
    }

    public void AttackAnimation()
    {
        StartCoroutine(AttackRoutine());

    }

    private void Update()
    {
        if (_lastState == state)
        {
            return;
        }

        else
        {
            _handAnimator[_lastState].SetActive(false);
            _handAnimator[state].SetActive(true);
            _lastState = state;
        }
    }

    IEnumerator AttackRoutine()
    {

        Animator animtor = _handAnimator[state].GetComponent<Animator>();

        _armAnimator.SetBool("Attack", true);
        animtor.SetFloat("Attack", 1f);

        yield return new WaitForSeconds(0.2f);

        _armAnimator.SetBool("Attack", false);

        while (animtor.GetFloat("Attack") > 0)
        {
            animtor.SetFloat("Attack", animtor.GetFloat("Attack") - 6f * Time.deltaTime);

            if (animtor.GetFloat("Attack") < 0)
            {
                animtor.SetFloat("Attack", 0f);
            }

            yield return new WaitForSeconds(0.01f);
        }

    }
}
