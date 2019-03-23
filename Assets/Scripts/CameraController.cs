using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float screenEdgeX, screenEdgeY;
    public float moveSpeed, zoomSpeed, turnSpeed;

    private int deltaX, deltaY;
    private bool mouseSlide;

    private Vector3 resetPosition;
    private Quaternion resetRotation;

    void Start()
    {
        mouseSlide = false;
        SetResetValues(transform.position, transform.rotation);
    }

    public void SetResetValuesWithCurrentValues()
    {
        resetPosition = transform.position;
        resetRotation = transform.rotation;
    }

    public void SetResetValues(Vector3 position, Quaternion rotation)
    {
        resetPosition = position;
        resetRotation = rotation;
    }

    void Update()
    {
        deltaX = deltaY = 0;

        if(mouseSlide)
        {
            if(Input.mousePosition.x <= screenEdgeX)
                deltaX -= 1;
            else if(Input.mousePosition.x >= Screen.height - screenEdgeX)
                deltaX += 1;

            if(Input.mousePosition.y <= screenEdgeY)
                deltaY -= 1;
            else if(Input.mousePosition.y >= Screen.height - screenEdgeY)
                deltaY += 1;
        }

        if(Input.GetKey(KeyCode.W))
            deltaY += 1;
        if(Input.GetKey(KeyCode.A))
            deltaX -= 1;
        if(Input.GetKey(KeyCode.S))
            deltaY -= 1;
        if(Input.GetKey(KeyCode.D))
            deltaX += 1;

        if(Input.GetMouseButton(2))
        {
            if(deltaX != 0)
                transform.LookAt(transform.position + transform.forward + (transform.right * Time.deltaTime * turnSpeed * deltaX));

            //if(deltaY != 0)
            //    transform.LookAt(transform.position + transform.forward + (transform.up * Time.deltaTime * turnSpeed * deltaY));
        }
        else
        {
            if(deltaX != 0)
                transform.position = transform.position + (transform.right * moveSpeed * 0.1f * deltaX);

            if(deltaY != 0)
                transform.position = transform.position + (transform.up * moveSpeed * 0.1f * deltaY);
        }

        if (Input.mouseScrollDelta.y != 0)
            transform.position = transform.position + (transform.forward * (Input.mouseScrollDelta.y * zoomSpeed * 0.1f));

        if (Input.GetKeyDown(KeyCode.LeftShift))
            mouseSlide = !mouseSlide;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = resetPosition;
            transform.rotation = resetRotation;
        }
    }
}