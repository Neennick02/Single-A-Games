using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BlindEye : MonoBehaviour
{
    public float FadeOutTime = 1f;
    private bool _blind = false;
    private Renderer _renderer;
    private Material _mat;

    private void OnEnable()
    {
        EyeGrenade.OnBlindEye += BlindEyes;
        EyeGrenade.OnHealEye += HealEyes;
        _renderer = GetComponent<Renderer>();
        _mat = _renderer.material;
    }

    private void OnDisable()
    {
        EyeGrenade.OnBlindEye -= BlindEyes;
        EyeGrenade.OnHealEye -= HealEyes;
    }

    private void BlindEyes()
    {
        if (!_blind) _blind = true;


        StartCoroutine(FadeIn());
    }

    private void HealEyes()
    {
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        if (_blind)
        {
            _renderer.material.SetFloat("_AlphaAmount", Mathf.PingPong(Time.time, 1));
        }
    }

    IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < FadeOutTime)
        {
            timer += Time.deltaTime;

            _mat.SetFloat("_DissolveStrength", Mathf.Lerp(2, 0, timer / 1));

            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float timer = 0;
        while (timer < FadeOutTime)
        {
            timer += Time.deltaTime;
            _mat.SetFloat("_DissolveStrength", Mathf.Lerp(0, 2, timer / 1));

            yield return null;
        }
    }
}
