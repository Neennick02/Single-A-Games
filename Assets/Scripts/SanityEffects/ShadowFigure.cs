using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowFigure : MonoBehaviour
{
    public float FadeOutDistance = 10f;
    public List<Texture> Textures;
    public float duration = 1f;

    private Material _mat;
    private Transform _playerTransform;
    private void Start()
    {
        _mat = GetComponent<Renderer>().sharedMaterial;
        _mat.SetFloat("_FadeAmount", 0);

        Texture randomT = Textures[Random.Range(0 , Textures.Count-1)];
        _mat.SetTexture("_BaseTexture", randomT);

        _playerTransform = PlayerMotor.Instance.transform;
    }

    private void Update()
    {

        float dist = Vector3.Distance(transform.position, _playerTransform.position);
        
        if(dist < FadeOutDistance)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float time = 0f;
        float amount = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            amount = Mathf.Lerp(amount, -1, time / duration);

            _mat.SetFloat("_FadeAmount", amount);

            yield return null;
        }
        _mat.SetFloat("_FadeAmount", -1);
        Destroy(gameObject);
    }
}
