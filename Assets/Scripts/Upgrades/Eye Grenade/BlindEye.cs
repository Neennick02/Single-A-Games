using System.Collections;
using UnityEngine;

public class BlindEye : MonoBehaviour
{
    public float FadeOutTime = 1f;
    public float AlphaRange = 3f;
    private bool _blind = false;
    private Renderer _renderer;
    private Material _mat;
    private float timer;

    public static bool Active;

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
        if (!Active) return;

        if (!_blind) _blind = true;


        StartCoroutine(FadeIn());
    }

    private void HealEyes()
    {
        if (!Active) return;

        _blind = false;
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        if (_blind)
        {
            timer += Time.deltaTime;
            _renderer.material.SetFloat("_AlphaAmount", 1 + Mathf.PingPong(timer, AlphaRange));
        }
        else
        {
            timer = 0f;
            _renderer.material.SetFloat("_AlphaAmount", 1);
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
