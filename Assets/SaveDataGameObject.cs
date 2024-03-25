using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveDataGameObject : MonoBehaviour
{
    public int levelInt = 1; // Giả sử levelInt được gán giá trị 1

    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");

        MeshDataCollection meshDataCollection = new MeshDataCollection(); // Tạo một đối tượng mới để lưu trữ dữ liệu của tất cả các mesh

        foreach (GameObject obj in objects)
        {
            // Get the MeshFilter component
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                // Tạo một đối tượng MeshData mới để lưu trữ dữ liệu của mesh
                MeshData meshData = new MeshData();
                meshData.vertices = meshFilter.sharedMesh.vertices;
                meshData.normals = meshFilter.sharedMesh.normals;
                meshData.triangles = meshFilter.sharedMesh.triangles;

                // Thêm MeshData mới vào danh sách MeshDataCollection
                meshDataCollection.meshDataList.Add(meshData);

                Debug.Log("Mesh data saved for object: " + obj.name);
            }
            else
            {
                Debug.LogWarning("Mesh filter or mesh is missing on object: " + obj.name);
            }
        }

        // Convert MeshDataCollection object to JSON
        string jsonData = JsonUtility.ToJson(meshDataCollection);

        // Write JSON data to file
        string filePath = "level" + levelInt + "_MeshCollection.mesh";
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Mesh data collection saved to: " + filePath);
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
