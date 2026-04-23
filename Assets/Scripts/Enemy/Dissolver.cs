using System.Collections;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    [SerializeField] private float _dissolveDuration = 1f;
    [SerializeField] private float _dissolveAmount = 1f;
    private float _dissolveStrength;
    private float _dissolveProgress;
    private void Start()
    {
        _dissolveProgress = 0f;
    }

    public void StartDissolve()
    {
        StartCoroutine(DissolveRoutine());
    }
    public IEnumerator DissolveRoutine()
    {
        float elapsedTime = 0;
        Material dissolveMaterial = GetComponent<Renderer>().material;

        while (elapsedTime < _dissolveDuration)
        {
            elapsedTime += Time.deltaTime;

            _dissolveStrength = Mathf.Lerp(_dissolveProgress, _dissolveProgress + _dissolveAmount, elapsedTime / _dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveStrength", _dissolveStrength);
            
            yield return null;
        }

        _dissolveProgress += _dissolveAmount;
    }
}
