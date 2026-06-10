using NUnit;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float StartLength = 0.1f;
    public float EndLength = 1f;
    public float Duration = 0.5f;

    private float _resetTimer = 0f;
    public float ResetDur = 5f;

    private bool SpikesActive = false;

    [SerializeField] private Transform SpikeTransform;
    private Collider SpikeCollider;

    public AudioClip SpikeSound;
    private void Start()
    {
        SpikeCollider = SpikeTransform.GetComponent<Collider>();
        SpikeTransform.localScale = new Vector3(SpikeTransform.localScale.x, StartLength, SpikeTransform.localScale.z);
    }

    private void Update()
    {
        if (SpikesActive)
        {
            _resetTimer += Time.deltaTime;

            if(_resetTimer >= ResetDur)
            {
                StartCoroutine(ExtendSpikes(EndLength, StartLength));
                SpikeCollider.enabled = false;
                SpikesActive = false;

                _resetTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!SpikesActive)
            {
                SpikeCollider.enabled = true;
                StartCoroutine(ExtendSpikes(StartLength, EndLength));
                SpikesActive = true;
            }
        }
    }

    IEnumerator ExtendSpikes(float start, float end)
    {
        if(start > end)
        {
            yield return new WaitForSeconds(0.5f);
        }

        float timer = 0f;
        float y = StartLength;

        AudioManager.Instance.PlayClip(SpikeSound);


        while (timer < Duration)
        {
            timer += Time.deltaTime;

            y = Mathf.Lerp(start, end, timer / Duration);

            Vector3 scale = new Vector3(SpikeTransform.localScale.x, y, SpikeTransform.localScale.z);
            SpikeTransform.localScale = scale;
            
            yield return null;
        }

        SpikeTransform.localScale = new Vector3(SpikeTransform.localScale.x, end, SpikeTransform.localScale.z);
    }
}
