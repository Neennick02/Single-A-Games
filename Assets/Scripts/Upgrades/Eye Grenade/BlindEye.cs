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
        _imagePanel = GetComponent<Image>();
    }

    private void OnDisable()
    {
        EyeGrenade.OnBlindEye -= BlindEyes;
    }

    private void BlindEyes()
    {
        if (!_blind) _blind = true;


        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        

        float timer = 0;
        while(timer < FadeOutTime)
        {
            timer += Time.deltaTime;

            _imagePanel.color = Color.Lerp(_startC, _endC, timer / 5);

            yield return null;  
        }

        yield return new WaitForSeconds(5f);
    }

    IEnumerator FadeOut()
    {
        yield return null;
    }
}
