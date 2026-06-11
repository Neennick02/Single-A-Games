using UnityEngine;

public class PukeUpgrade : MonoBehaviour
{
    void Start()
    {

        Puke puke = FindFirstObjectByType<Puke>();
        puke.enabled = true;

    }

}
