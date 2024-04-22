using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSelect : MonoBehaviour
{
    private FileChooser fileChooser;
    public static FileSelect instance;
    public string filePath;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Tuỳ chọn: giữ Singleton này khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Nếu instance đã tồn tại, hủy đối tượng mới được tạo
        }
    }

    private void Start()
    {
        fileChooser = GetComponent<FileChooser>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Mở hộp thoại chọn file IFC khi nhấn phím Space
            filePath = fileChooser.OpenIFCFile();

            if (!string.IsNullOrEmpty(filePath))
            {
                // Xử lý file IFC đã chọn ở đây
                Debug.Log("Đã chọn file IFC: " + filePath);
            }
            else
            {
                Debug.Log("Người dùng đã hủy chọn file IFC.");
            }
        }
    }

}
