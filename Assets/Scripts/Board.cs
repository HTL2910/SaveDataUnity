using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    private BackgroundTitle[,] allTiles;
    private void Start()
    {
        allTiles = new BackgroundTitle[width, height];
        SetUp();
    }
    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backGroundTiles=Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backGroundTiles.transform.parent=this.transform;
                backGroundTiles.name="("+i+","+j+")";
            }
        }
    }
}
