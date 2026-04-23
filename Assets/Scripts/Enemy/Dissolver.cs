using System.Collections;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    [SerializeField] private float _dissolveDuration = 1f;
    [SerializeField] private float _dissolveAmount = 1f;
    private float _dissolveStrength;
    [SerializeField] private float _dissolveProgress;

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


        yield return new WaitForSeconds(2f);
        float elapsedTime = 0;
        Material dissolveMaterial = GetComponent<Renderer>().material;

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
