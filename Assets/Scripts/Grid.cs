using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int height;
    public int width;
    
    public GameObject[,] grid;
    public GameObject prefabsGrid;
    public GameObject wall;
    public SelectTarget select;
    public List<GameObject> obj= new List<GameObject>();
    private void Start()
    {
        grid=new GameObject[height,width];


        CreateGrid();
    }
    private void CreateGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject gridTmp = Instantiate(prefabsGrid, this.gameObject.transform);
                gridTmp.transform.position = new Vector3(i, j, 0);
                grid[i, j] = gridTmp;
                obj.Add(gridTmp);
                gridTmp.name = "[" + i + "," + j + "]";

            }

        }

        for (int j = 0; j < width; j++)
        {
            GameObject gridTmp = Instantiate(wall, this.gameObject.transform);
            gridTmp.transform.position = new Vector3(j,-1, 0);
       
            gridTmp.name = "[" + j+ "," + -1 + "]";

        }
    }
    public void NewGrid()
    {
        // Duyệt qua tất cả các con của đối tượng hiện tại
        for (int i = 0; i < obj.Count; i++)
        {
            // Tìm MeshRenderer của đối tượng con
            MeshRenderer meshRenderer = obj[i].GetComponent<MeshRenderer>();

            // Kiểm tra nếu MeshRenderer tồn tại
            if (meshRenderer != null)
            {
                // Lấy material của MeshRenderer
                Material material = meshRenderer.material;

                // Kiểm tra nếu material là Matdelete
                if (material == select.Matdelete)
                {
                    // Ẩn đối tượng con
                    Destroy(obj[i].gameObject);
                }
            }
        }
    }

}
