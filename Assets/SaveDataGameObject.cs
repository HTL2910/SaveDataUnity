﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class SaveDataGameObject : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public GameObject model3D;
    public List<GameObject> listGameObjects=new List<GameObject>();
    public void Save()
    {

        for(int i = 0;i<model3D.transform.childCount;i++)
        {
            listGameObjects.Add(model3D.transform.GetChild(i).gameObject);
        }    
        

        MeshDataCollection meshDataCollection = new MeshDataCollection();

        foreach (GameObject obj in listGameObjects)
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

        string filePath = nameInputField.text + ".mesh";
        PlayerPrefs.SetString("NameFile", filePath);
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
