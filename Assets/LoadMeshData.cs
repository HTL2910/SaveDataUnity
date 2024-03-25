using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LoadMeshData : MonoBehaviour
{
    public string levelFileName = "level1_MeshCollection"; // Tên của tệp dữ liệu bạn muốn tải

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
                newObject.name = levelFileName + "_" + System.Guid.NewGuid().ToString(); // Đặt tên của đối tượng mới
                MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;

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
    }

    [System.Serializable]
    public class MeshDataCollection
    {
        public List<MeshData> meshDataList = new List<MeshData>(); // Danh sách các MeshData được lưu
    }
}
