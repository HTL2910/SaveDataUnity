using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalar : MonoBehaviour
{
    private Board board;
    public float cameraOffset = -10f;
    public float padding = 2f;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        if (board != null)
        {
            RepositionCamera(board.width - 1, board.height - 1);
        }
        else { }
    }

    void RepositionCamera(float width, float height)
    {
        // Tính tỷ lệ khung hình của màn hình
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float targetAspect = width / height;

        // Tính toán kích thước orthographic size của camera
        float orthoSize;
        if (screenAspect >= targetAspect)
        {
            // Màn hình rộng hơn, giới hạn chiều cao
            orthoSize = height / 2 + padding;
        }
        else
        {
            // Màn hình cao hơn, giới hạn chiều rộng
            float differenceInSize = targetAspect / screenAspect;
            orthoSize = (height / 2 + padding) * differenceInSize;
        }

        Camera.main.orthographicSize = orthoSize;

        // Đặt vị trí camera để trung tâm vào bảng
        Vector3 tempPosition = new Vector3(width / 2, height / 2 + 1, cameraOffset);
        transform.position = tempPosition;
    }
}
