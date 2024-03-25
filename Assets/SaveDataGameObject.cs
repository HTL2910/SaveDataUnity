using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveDataGameObject : MonoBehaviour
{
    public int levelInt = 1;
    public void Save()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");

        MeshDataCollection meshDataCollection = new MeshDataCollection();

        foreach (GameObject obj in objects)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                MeshData meshData = new MeshData();
                meshData.vertices = meshFilter.sharedMesh.vertices;
                meshData.normals = meshFilter.sharedMesh.normals;
                meshData.triangles = meshFilter.sharedMesh.triangles;
                meshData.position = obj.transform.position; // Lưu vị trí của đối tượng
                meshData.objectName = obj.name; // Lưu tên của đối tượng

                meshDataCollection.meshDataList.Add(meshData);

                Debug.Log("Mesh data saved for object: " + obj.name);
            }
            else
            {
                Debug.LogWarning("Mesh filter or mesh is missing on object: " + obj.name);
            }
        }

        string jsonData = JsonUtility.ToJson(meshDataCollection);

        string filePath = "level" + levelInt + "_MeshCollection.mesh";
        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            streamWriter.Write(jsonData);
        }

        Debug.Log("Mesh data collection saved to: " + filePath);
    }

    [System.Serializable]
    public class MeshData
    {
        public Vector3[] vertices;
        public Vector3[] normals;
        public int[] triangles;
        public Vector3 position; // Thêm dữ liệu vị trí
        public string objectName; // Thêm dữ liệu tên đối tượng
    }

    [System.Serializable]
    public class MeshDataCollection
    {
        public List<MeshData> meshDataList = new List<MeshData>();
    }
}
