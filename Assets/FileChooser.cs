using UnityEngine;
using SFB;

public class FileChooser : MonoBehaviour
{
    // Hàm mở hộp thoại chọn tệp và trả về đường dẫn tệp đã chọn
    public string OpenIFCFile()
    {
        // Mở hộp thoại chọn file và chỉ cho phép chọn file đơn lẻ
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Chọn file IFC", "", "ifc", false);

        // Kiểm tra xem người dùng đã chọn file hay chưa
        if (paths != null && paths.Length > 0)
        {
            // Trả về đường dẫn của file IFC đã chọn
            return paths[0];
        }
        else
        {
            // Người dùng đã hủy chọn file
            return null;
        }
    }
}
