using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PukeVolumeEffect : MonoBehaviour
{

    private Volume _volume;

    private LensDistortion _effect;

    private float _xMult = 1f;
    private float _yMult = -1f;

    private void Start()
    {
        _volume = GetComponent<Volume>();

        _volume.profile.TryGet(out _effect);
    }

    void Update()
    {

        float min = 0.3f;

        float max = 0.6f;

        _effect.xMultiplier.value += _xMult * Time.deltaTime;

        if (_effect.xMultiplier.value >= 0.6f)
        {

            _xMult = Mathf.Lerp(_xMult, -1f, Time.deltaTime * 5f);

        }

        else if (_effect.xMultiplier.value <= 0.3f)
        {

            _xMult = Mathf.Lerp(_xMult, 1f, Time.deltaTime * 5f);

        }


        _effect.yMultiplier.value += _yMult * Time.deltaTime;

        if (_effect.yMultiplier.value >= 0.6f)
        {

            _yMult = Mathf.Lerp(_yMult, -1f, Time.deltaTime * 5f);

        }

        else if (_effect.yMultiplier.value <= 0.3f)
        {

            _yMult = Mathf.Lerp(_yMult, 1f, Time.deltaTime * 5f);

        }

    }
}
