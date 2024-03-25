using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LoadMeshData : MonoBehaviour
{
    public string levelFileName = "level1_MeshCollection"; // Tên của tệp dữ liệu bạn muốn tải
    public GameObject parentObject; // Tham chiếu đến GameObject cha

    void Start()
    {
        string filePath = levelFileName + ".mesh";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            MeshDataCollection meshDataCollection = JsonUtility.FromJson<MeshDataCollection>(jsonData);

            // Lặp qua mỗi meshData trong danh sách meshDataList
            foreach (MeshData meshData in meshDataCollection.meshDataList)
            {
                // Tạo mesh mới từ dữ liệu đã đọc
                Mesh mesh = new Mesh();
                mesh.vertices = meshData.vertices;
                mesh.normals = meshData.normals;
                mesh.triangles = meshData.triangles;

                // Tạo GameObject mới và gán mesh vào MeshFilter của nó
                GameObject newObject = new GameObject();
                newObject.name = levelFileName + "_" + "1"; // Đặt tên của đối tượng mới
                newObject.transform.SetParent(parentObject.transform); // Gán cha cho đối tượng mới
                MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                // Gán vị trí của đối tượng mới từ dữ liệu vị trí
                newObject.transform.position = meshData.position;

                // Tạo Renderer để hiển thị mesh
                MeshRenderer renderer = newObject.AddComponent<MeshRenderer>();
                renderer.material = new Material(Shader.Find("Standard")); // Đặt vật liệu cho renderer

                Debug.Log("Mesh data loaded from: " + filePath);
            }
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }

    [System.Serializable]
    public class MeshData
    {
        public Vector3[] vertices;
        public Vector3[] normals;
        public int[] triangles;
        public Vector3 position; // Thêm dữ liệu vị trí
    }

    [System.Serializable]
    public class MeshDataCollection
    {
        public List<MeshData> meshDataList = new List<MeshData>(); // Danh sách các MeshData được lưu
    }
}
