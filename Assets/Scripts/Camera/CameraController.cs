using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    public float currentZoom = 0;
    public float zoomSpeed = 1;

    public float zoomRotation = 1;
    public Vector2 zoomRange = new Vector2(-20, 100);
    public Vector2 zoomAngleRange = new Vector2(20, 70);

    float rotateSpeed = 6f;

    Vector3 lastMousePosition;

    private Vector3 initialPosition;

    private Vector3 initialRotation;


    InputManager key;
    private void Start()
    {
        key = InputManager.instance;
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {


        // FORWARD : BACKWARD : LEFT : RIGHT

        if (Input.GetKey(key.forward) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(key.backward) || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(key.right) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(key.left) || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.Self);
        }

        //ZOOM IN/OUT

        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1000 * zoomSpeed;

        currentZoom = Mathf.Clamp(currentZoom, zoomRange.x, zoomRange.y);

        transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - (initialPosition.y + currentZoom)) * 0.1f, transform.position.z);
        float x = transform.eulerAngles.x - (transform.eulerAngles.x - (initialRotation.x + currentZoom * zoomRotation)) * 0.1f;
        x = Mathf.Clamp(x, zoomAngleRange.x, zoomAngleRange.y);
        transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);

        // Rotation (either button)
        if (Input.GetKey(key.rotation))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            // if the game window is separate from the editor window and the editor
            // window is active then you go to right-click on the game window the
            // rotation jumps if  we don't ignore the mouseDelta for that frame.
            Vector3 mouseDelta;
            if (lastMousePosition.x >= 0
                && lastMousePosition.y >= 0
                && lastMousePosition.x <= Screen.width
                && lastMousePosition.y <= Screen.height)
                mouseDelta = Input.mousePosition - lastMousePosition;
            else
                mouseDelta = Vector3.zero;

            Vector3 rotation = Vector3.up * Time.deltaTime * rotateSpeed * mouseDelta.x;
            rotation += Vector3.left * Time.deltaTime * rotateSpeed * mouseDelta.y;
            transform.Rotate(rotation, Space.Self);

            // Make sure z rotation stays locked
            rotation = transform.rotation.eulerAngles;
            rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }

        if (Input.GetKeyUp(key.rotation))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        lastMousePosition = Input.mousePosition;
    }
}
