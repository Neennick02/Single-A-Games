using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float scrollSpeed;
    private void Update()
    {
        /*float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0f)
        {
            transform.position += Vector3.up * (-scroll * moveSpeed * Time.deltaTime);
        }
        else
        {*/
            transform.position += Vector3.up * (scrollSpeed * Time.deltaTime);
        //}
    }
}
