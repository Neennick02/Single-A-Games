using UnityEngine;
using UnityEngine.UI;

public class Puke : MonoBehaviour
{

    private Image _barImage;

    private float _amount;

    private PlayerInput _playerInput;

    private void Start()
    {
        _barImage = GetComponent<Image>();
    }

    private void OnEnable()
    {

    }

    void Update()
    {

        if (_amount < 1f)
        {
            _barImage.fillAmount = _amount += Time.deltaTime * 0.1f;
        }

        else
        {
            _amount = 1f;
        }
    }
}
