using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DissolveFlesh : MonoBehaviour
{
    [SerializeField] private float _dissolveDuration = 1f;
    [SerializeField] private float _dissolveAmount = 1f;
    private float _dissolveStrength;
    [SerializeField] private float _dissolveProgress;
    [SerializeField] private List<Renderer> _renderers;

    [SerializeField] private float _startNoiseScale = 100;
    [SerializeField] float speed = 1;
    private float _scale;
    private float timer;
    private float scaleAmount = 50;
    [SerializeField] private float duration = 5f;
    public void StartDissolve()
    {
        StartCoroutine(DissolveRoutine("_DissolveStrength"));
    }

    public void StartHorizontalDissolve()
    {
        StartCoroutine(DissolveRoutine("_CutoffHeight"));
    }

    private void Start()
    {
        StartCoroutine(ShiftNoiseScale(speed));
        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material.SetFloat("DissolveStrength", _dissolveStrength);

        }
    }
    private void Update()
    {
        Debug.Log(_scale);
    }

    IEnumerator ShiftNoiseScale(float speed)
    {     
        while (timer < duration)
        {
            timer += Time.deltaTime;

            foreach (var r in _renderers)
            {
                _scale = Mathf.PingPong(timer * speed / duration, scaleAmount);

                foreach (var mat in r.materials)
                {
                    mat.SetFloat("NoiseScale", _scale);
                }
            }

            yield return null;
        }
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
