using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public Camera screenshotCamera;

    public void TakeScreenshot()
    {
        // Khởi tạo một RenderTexture với kích thước của camera
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // Lưu trữ trạng thái target texture hiện tại của camera
        RenderTexture currentTargetTexture = screenshotCamera.targetTexture;

        // Thiết lập camera để sử dụng RenderTexture
        screenshotCamera.targetTexture = renderTexture;

        // Render scene vào RenderTexture
        screenshotCamera.Render();

        // Tạo một Texture2D mới có kích thước tương ứng
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // Đọc dữ liệu từ RenderTexture và ghi vào Texture2D
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // Lưu Texture2D thành một file ảnh PNG
        byte[] bytes = screenshot.EncodeToPNG();
        string filePath = Path.Combine(Application.persistentDataPath, "screenshot.png");
        File.WriteAllBytes(filePath, bytes);

        // Khôi phục lại trạng thái target texture ban đầu của camera
        screenshotCamera.targetTexture = currentTargetTexture;

        // Giải phóng bộ nhớ của RenderTexture và Texture2D
        RenderTexture.active = null;
        Destroy(renderTexture);
        Destroy(screenshot);

        Debug.Log("Screenshot saved: " + filePath);
    }
}
