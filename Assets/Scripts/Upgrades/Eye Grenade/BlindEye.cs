using System.Collections;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;

public class BlindEye : MonoBehaviour
{
    private Image _imagePanel;

    public float FadeOutTime = 1f;
    private bool _blind = false;

    [SerializeField] private Color _startC;
    [SerializeField] private Color _endC;


    private void OnEnable()
    {
        EyeGrenade.OnBlindEye += BlindEyes;
        EyeGrenade.OnHealEye += HealEyes;
        _imagePanel = GetComponent<Image>();
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

    IEnumerator FadeIn()
    {
        float timer = 0;
        while(timer < FadeOutTime)
        {
            timer += Time.deltaTime;

            _imagePanel.color = Color.Lerp(_startC, _endC, timer / 1);

            yield return null;  
        }
    }

    IEnumerator FadeOut()
    {
        float timer = 0;
        while (timer < FadeOutTime)
        {
            timer += Time.deltaTime;

            _imagePanel.color = Color.Lerp(_endC, _startC, timer / 1);

            yield return null;
        }
    }
}
