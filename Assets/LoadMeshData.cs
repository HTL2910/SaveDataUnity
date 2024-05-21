using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LoadMeshData : MonoBehaviour
{
    public GameObject parentObject; // Tham chiếu đến GameObject cha
    public List<Material> materials;
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
                newObject.transform.rotation = Quaternion.Euler(meshData.rotation);
                newObject.transform.localScale = meshData.scale;
                // Tạo Renderer để hiển thị mesh
                MeshRenderer renderer = newObject.AddComponent<MeshRenderer>();

                
                renderer.material = FindMaterialWithName(meshData.materialName); // Đặt vật liệu cho renderer

                Debug.Log("Mesh data loaded from: " + filePath);
            }
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }
    public Material FindMaterialWithName(string name)
    {
        for (int i = 0;i<materials.Count;i++)
        {
            if (materials[i].name == name)
            {
                return materials[i];
            }    
        }
        return materials[0];
    }    
    [System.Serializable]
    public class MeshData
    {
        public Vector3[] vertices;
        public Vector3[] normals;
        public int[] triangles;
        public Vector3 position; // Thêm dữ liệu vị trí
        public Vector3 rotation;
        public Vector3 scale;
        public string objectName; // Thêm dữ liệu tên đối tượng
        public string materialName; // Thêm dữ liệu material
    }

    [System.Serializable]
    public class MeshDataCollection
    {
        public List<MeshData> meshDataList = new List<MeshData>();
    }
}
