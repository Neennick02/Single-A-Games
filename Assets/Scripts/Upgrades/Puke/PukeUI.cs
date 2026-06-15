using UnityEngine;
using UnityEngine.UI;

public class PukeUI : MonoBehaviour
{
    [SerializeField] private Puke puke;
    private void OnEnable()
    {
        GameManager.OnEnableVomitUI += EnableUI;
    }

    private void OnDisable()
    {
        GameManager.OnEnableVomitUI -= EnableUI;
    }

    private void EnableUI()
    {
        puke.enabled = true;

        foreach (Transform child in transform)
        {
            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                img.enabled = true;
            }
        }
    }
}
