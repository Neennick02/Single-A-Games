using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _handAnimator = new List<GameObject>();
    [SerializeField] private Animator _armAnimator;

    public int _state;
    private int _lastState;

    private bool _change;

    private void Start()
    {
        _lastState = _state;
    }

    public void AttackAnimation()
    {
        StartCoroutine(AttackRoutine());

    }

    private void Update()
    {

        if (_lastState == _state)
        {

            _change = false;

            return;
        }

        else if (!_change)
        {
            _change = true;
            StartCoroutine(ChangeArm());
        }
    }
    private IEnumerator ChangeArm()
    {

        yield return new WaitForSeconds(0.5f);

        Dissolver _dissolveLast = _handAnimator[_lastState].GetComponent<Dissolver>();
        _dissolveLast.StartDissolve();

        Dissolver _dissolve = _handAnimator[_state].GetComponent<Dissolver>();
        _dissolve.StartDissolve();

        yield return new WaitForSeconds(1.1f);

        _dissolve._dissolveProgress = 0;

        _dissolve._dissolveAmount = 1;

        _lastState = _state;

    }

    private IEnumerator AttackRoutine()
    {

        Animator animtor = _handAnimator[_state].GetComponent<Animator>();

        float rando = Random.Range(1, 4);

        Debug.Log(rando);

        _armAnimator.SetFloat("Blend", rando);
        _armAnimator.SetBool("Attack", true);
        animtor.SetBool("Attack", true);



        yield return new WaitForSeconds(0.2f);

        _armAnimator.SetBool("Attack", false);
        animtor.SetBool("Attack", false);



    }
}
