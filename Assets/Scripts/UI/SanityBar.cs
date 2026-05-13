using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{
    private Image _barImage;
    private float _maxSanity;
    private void OnEnable()
    {
        SanityManager.OnDrainAmountChanged += UpdateBar;
    }

    private void OnDisable()
    {
        SanityManager.OnDrainAmountChanged -= UpdateBar;
    }

    private void Start()
    {
        _barImage = GetComponent<Image>();
        _maxSanity = SanityManager.MaxSanity;
    }

    private void UpdateBar(float amount)
    {
        _barImage.fillAmount = amount / _maxSanity;
    }
}
