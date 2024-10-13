using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int height;
    public int width;
    
    public GameObject[,] grid;
    public GameObject prefabsGrid;
    private void Start()
    {
        grid=new GameObject[height,width];


        CreateGrid();
    }
    private void CreateGrid()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                GameObject gridTmp = Instantiate(prefabsGrid, this.gameObject.transform);
                gridTmp.transform.position = new Vector3(i, j, 0);
                grid[i,j]=gridTmp;
                gridTmp.name = "[" + i + ","+j+"]";

            }    
        }
    }
}
