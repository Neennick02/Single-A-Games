using System.Collections;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    [SerializeField] private float _dissolveDuration = 1f;
    [SerializeField] private float _dissolveAmount = 1f;
    private float _dissolveStrength;
    [SerializeField] private float _dissolveProgress;
    [SerializeField] private Renderer _renderer;
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

        Material dissolveMaterial = _renderer.material;
        Debug.Log(dissolveMaterial.ToString());

        while (elapsedTime < _dissolveDuration)
        {
            elapsedTime += Time.deltaTime;

            _dissolveStrength = Mathf.Lerp(_dissolveProgress, _dissolveProgress + _dissolveAmount, elapsedTime / _dissolveDuration);
            dissolveMaterial.SetFloat(strength, _dissolveStrength);
            
            yield return null;
        }

        _dissolveProgress += _dissolveAmount;
    }
}
