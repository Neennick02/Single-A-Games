using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    [SerializeField] private float _dissolveDuration = 1f;
    [SerializeField] private float _dissolveAmount = 1f;
    private float _dissolveStrength;
    [SerializeField] private float _dissolveProgress;
    [SerializeField] private List<Renderer> _renderers;
    public void StartDissolve()
    {
        StartCoroutine(DissolveRoutine("_DissolveStrength"));
    }

    public void StartHorizontalDissolve()
    {
        StartCoroutine(DissolveRoutine("_CutoffHeight"));
    }
    public IEnumerator DissolveRoutine(string strength)
    {
        float elapsedTime = 0;

        while (elapsedTime < _dissolveDuration)
        {
            elapsedTime += Time.deltaTime;

            _dissolveStrength = Mathf.Lerp(_dissolveProgress, _dissolveProgress + _dissolveAmount, elapsedTime / _dissolveDuration);

            //loop over materials and apply changes
            for (int i = 0; i < _renderers.Count; i++)
            {
                _renderers[i].material.SetFloat(strength, _dissolveStrength);

            }
            yield return null;
        }

        _dissolveProgress += _dissolveAmount;
    }
}
