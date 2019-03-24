using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed, zoomSpeed, turnSpeed;

    private float deltaX, deltaY, deltaR, deltaZoom;
    private bool mouseSlide;

    private Vector3 resetPosition;
    private Quaternion resetRotation;

    void Start()
    {
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
        deltaR = 0;

        deltaX = Input.GetAxis("Horizontal");
        deltaY = Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.Q))
            deltaR -= 1.0f;
        if(Input.GetKey(KeyCode.E))
            deltaR += 1.0f;

        deltaR   += Input.GetAxis("Horizontal_2");
        deltaZoom = Input.mouseScrollDelta.y + Input.GetAxis("Vertical_2");

        if (deltaX != 0 || deltaY != 0 || deltaZoom != 0)
            transform.Translate(new Vector3(moveSpeed * deltaX * Time.deltaTime, moveSpeed * deltaY * Time.deltaTime, deltaZoom * zoomSpeed));

        if(deltaR != 0)
            transform.Rotate(new Vector3(0, 0, turnSpeed * deltaR * Time.deltaTime));

        if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button8) || Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            transform.position = resetPosition;
            transform.rotation = resetRotation;
        }
    }
}