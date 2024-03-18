using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float rotationSpeed = 1;
    public float translationSpeed = 20;
    public float mouseSensitivity = 7.0f;
    private bool isMouseClicked = false;
    public Vector2 turn;

    public float mouseSensitivityPan = 0.05f;
    private Vector3 lastPosition;

    public Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotateMouse();
        zoomMouse();
        panMouse();
        keysMovement();
    }

    private void keysMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward / 50 * translationSpeed, Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left / 50 * translationSpeed, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back / 50 * translationSpeed, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right / 50 * translationSpeed, Space.Self);
        }
    }


    private void rotateMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseClicked = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseClicked = false;
        }
        if (isMouseClicked)
        {
            turn.x += Input.GetAxis("Mouse X") * mouseSensitivity;
            turn.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
            transform.localRotation = Quaternion.Euler(turn.y, turn.x, 0);
        }
    }

    private void zoomMouse()
    {
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");

        if (ScrollWheelChange != 0)
        {                                            //If the scrollwheel has changed
            float R = ScrollWheelChange * 15;                                   //The radius from current camera
            float PosX = Camera.main.transform.eulerAngles.x + 90;              //Get up and down
            float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);       //Get left to right
            PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
            PosY = PosY / 180 * Mathf.PI;                                       //^
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
            float Y = R * Mathf.Cos(PosX);                                      //^
            float CamX = Camera.main.transform.position.x;                      //Get current camera postition for the offset
            float CamY = Camera.main.transform.position.y;                      //^
            float CamZ = Camera.main.transform.position.z;                      //^
            Camera.main.transform.position = new Vector3(CamX + X, CamY + Y, CamZ + Z);//Move the main camera
        }
    }

    private void panMouse()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(2))
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            var delta = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * mouseSensitivityPan, delta.y * mouseSensitivityPan, 0);
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            Cursor.SetCursor(null, hotSpot, cursorMode);
        }
    }
}