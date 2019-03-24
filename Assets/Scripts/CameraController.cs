using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float axisDeadZone;
    public float moveSpeed, zoomSpeed, turnSpeed;

    private float deltaX, deltaY;
    private int deltaR;
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
        deltaX = deltaY = deltaR = 0;

        deltaX = Input.GetAxis("Horizontal");
        deltaY = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Q))
            deltaR -= 1;
        if(Input.GetKey(KeyCode.E))
            deltaR += 1;

        if(deltaX > axisDeadZone || deltaX < -axisDeadZone ||
           deltaY > axisDeadZone || deltaY < -axisDeadZone ||
           Input.mouseScrollDelta.y != 0)
            transform.Translate(new Vector3(moveSpeed * deltaX * Time.deltaTime, moveSpeed * deltaY * Time.deltaTime, Input.mouseScrollDelta.y * zoomSpeed));

        if(deltaR != 0)
            transform.Rotate(new Vector3(0, 0, turnSpeed * deltaR * Time.deltaTime));

        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = resetPosition;
            transform.rotation = resetRotation;
        }
    }
}