using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SanityBlob : MonoBehaviour
{
    private Transform target;
    public float _startSpeed = 1;
    public float _speedIncrease = 0.1f;
    private float xAngle = 1f, yAngle = 1f, zAngle = 1f;
    public AudioClip Clip;
    private void Start()
    {
        target = Camera.main.transform;
        transform.localScale = Vector3.zero;
        StartCoroutine(GrowParticle());
    }

    private void Update()
    {
        float step = _startSpeed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        _startSpeed += _speedIncrease;

        transform.Rotate( xAngle, yAngle, zAngle, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlayClip(Clip, 1, Random.Range(0.6f, 1.5f));
            Destroy(gameObject);
        }
    }

    private IEnumerator GrowParticle()
    {
        float scale = Random.Range(0.1f, 0.4f);
        Vector3 targetScale = new Vector3(scale, scale, scale);
        float timer = 0f;
        float dur = 1f;

        while (timer < dur)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, timer / dur);
            yield return null;
        }

    }
}
