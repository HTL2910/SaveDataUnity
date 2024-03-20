using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using TMPro;
public class MeshLog : MonoBehaviour
{
    public GameObject model;
    private Color modelColor;
    public TextMeshProUGUI textMeshProUGUI;
    private string V3ArrayToStr(string varString, Vector3[] vector3Arr)
    {
        string str = "Vector3[]" + varString + " =new Vector3[] {";
        foreach(Vector3 vertex in vector3Arr)
        {
            str = str + "new Vector3(" + vertex.x + "f," + vertex.y + "f," + vertex.z + "f),";

        }
        if(str.Length>1)
        {
            str=str.Remove(str.Length-1);
        }
        str += "};";
        return str;
    }    
    private string IntArrayToStr(string varString, int[] intArr)
    {
        string str = "int[]" + varString + "=new int[]{";
        foreach (int index in intArr)
        {
            str = str + index + ",";
        }
        if (str.Length > 1)
        {
            str = str.Remove(str.Length - 1);
        }
        str += "};";
        return str;
    }
    public void PrintModel()
    {
        for(int i=0;i<model.GetComponentsInChildren<MeshFilter>().Length;i++)
        {
            Mesh mesh = model.GetComponentsInChildren<MeshFilter>()[i].mesh;
            modelColor = model.GetComponentsInChildren<MeshRenderer>()[i].material.color;
            PrintMesh(mesh);
        }
        
    }    
    private void PrintMesh(Mesh mesh)
    {

        // Lấy dữ liệu từ lưới
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;
        int[] indices = mesh.GetIndices(0);

        // Kiểm tra xem mô hình có bộ kết xuất ReshiRenderer hay không
        if (model.GetComponent<MeshRenderer>() == null)
        {
            // Thêm bộ kết xuất ReshiRenderer vào mô hình
            model.AddComponent<MeshRenderer>();
        }

        // Tạo chuỗi để lưu trữ mã
        StringBuilder sb = new StringBuilder();

        // Thêm mảng đỉnh
        sb.Append(V3ArrayToStr("vertices", vertices));
        sb.Append(Environment.NewLine);

        // Thêm mảng pháp tuyến
        sb.Append(V3ArrayToStr("normals", normals));
        sb.Append(Environment.NewLine);

        // Thêm mảng tam giác
        sb.Append(IntArrayToStr("triangles", triangles));
        sb.Append(Environment.NewLine);

        // Thêm mảng chỉ số
        sb.Append(IntArrayToStr("indices", indices));
        sb.Append(Environment.NewLine);

        // Tạo màu
        sb.Append("Color color = new Color(" + modelColor.r + "f, " + modelColor.g + "f, " + modelColor.b + "f, " + modelColor.a + "f);");
        sb.Append(Environment.NewLine);

        // Tạo lưới mới
        sb.Append("Mesh mesh = new Mesh();");
        sb.Append(Environment.NewLine);

        // Cấu hình định dạng chỉ mục cho lưới
        sb.Append("mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;");
        sb.Append(Environment.NewLine);

        // Gán dữ liệu vào lưới
        sb.Append("mesh.vertices = vertices;");
        sb.Append(Environment.NewLine);
        sb.Append("mesh.normals = normals;");
        sb.Append(Environment.NewLine);
        sb.Append("mesh.triangles = triangles;");
        sb.Append(Environment.NewLine);

        // Cài đặt chỉ số cho lưới
        sb.Append("mesh.SetIndices(indices, MeshTopology." + mesh.GetTopology(0) + ", 0);");
        sb.Append(Environment.NewLine);

        // Tạo đối tượng trò chơi mới
        sb.Append("GameObject meshGameObj = new GameObject();");
        sb.Append(Environment.NewLine);

        // Thêm bộ lọc ReshiFilter và bộ kết xuất ReshiRenderer vào đối tượng trò chơi
        sb.Append("meshGameObj.AddComponent<MeshFilter>();");
        sb.Append(Environment.NewLine);
        sb.Append("meshGameObj.AddComponent<MeshRenderer>();");
        sb.Append(Environment.NewLine);

        // Gán lưới vào bộ lọc ReshiFilter
        sb.Append("meshGameObj.GetComponent<MeshFilter>().mesh = mesh;");
        sb.Append(Environment.NewLine);

        // Gán màu cho bộ kết xuất ReshiRenderer
        sb.Append("meshGameObj.GetComponent<MeshRenderer>().material.color = color;");

        textMeshProUGUI.text = sb.ToString();


    }
}
