using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LoadMeshData : MonoBehaviour
{
    public GameObject parentObject; // Tham chiếu đến GameObject cha

    public void Load()
    {
        string filePath = PlayerPrefs.GetString("NameFile", string.Empty);
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
                newObject.name = meshData.objectName; // Đặt tên của đối tượng mới từ dữ liệu tên
                newObject.transform.SetParent(parentObject.transform); // Gán cha cho đối tượng mới
                MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                // Gán vị trí của đối tượng mới từ dữ liệu vị trí
                newObject.transform.position = meshData.position;

                // Tạo Renderer để hiển thị mesh
                MeshRenderer renderer = newObject.AddComponent<MeshRenderer>();
                Material material = new Material(Shader.Find("Standard")); // Tạo vật liệu mới
                material.name = meshData.material.materialName;
                material.color = meshData.material.color;
                // Gán texture nếu có
                if (!string.IsNullOrEmpty(meshData.material.texturePath))
                {
                    Texture texture = (Texture)Resources.Load(meshData.material.texturePath);
                    if (texture != null)
                    {
                        material.mainTexture = texture;
                    }
                }

                renderer.material = material; // Đặt vật liệu cho renderer

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
        public string objectName; // Thêm dữ liệu tên đối tượng
        public MaterialData material; // Thêm dữ liệu material
    }

    [System.Serializable]
    public class MaterialData
    {
        public string materialName;
        public Color color;
        public string texturePath; // Thêm dữ liệu texture
    }

    [System.Serializable]
    public class MeshDataCollection
    {
        public List<MeshData> meshDataList = new List<MeshData>();
    }
}
