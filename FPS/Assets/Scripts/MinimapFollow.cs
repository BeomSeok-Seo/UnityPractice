using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform target;
    Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = target.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        //transform.rotation = Quaternion.identity;
        transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
    }

    public void ZoomIn()
    {
        if (camera.orthographicSize < 5)
        {
            return;
        }

        camera.orthographicSize -= 1f;
    }

    public void ZoomOut()
    {
        if (camera.orthographicSize > 15)
        {
            return;
        }

        camera.orthographicSize += 1f;
    }
}
