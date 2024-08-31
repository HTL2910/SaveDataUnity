using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    public GameObject gridParent;
    public GameObject cellPrefab;
    public LineRenderer lineRenderer;

    private int[,] binaryMatrix;
    private int targetNumber;
    private List<Vector2Int> selectedCells;
    private List<Vector3> linePoints;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        targetNumber = Random.Range(0, 64);
        targetText.text = "Target: " + targetNumber.ToString();

        binaryMatrix = new int[5, 5];
        selectedCells = new List<Vector2Int>();
        linePoints = new List<Vector3>();

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                binaryMatrix[i, j] = Random.Range(0, 2);
                GameObject cell = Instantiate(cellPrefab, gridParent.transform);
                cell.GetComponentInChildren<TextMeshProUGUI>().text = binaryMatrix[i, j].ToString();
                int x = i, y = j;
                cell.GetComponent<Button>().onClick.AddListener(() => OnCellSelected(x, y));
            }
        }

        lineRenderer.positionCount = 0; // Initialize the LineRenderer
    }

    void OnCellSelected(int x, int y)
    {
        if (selectedCells.Count < 6)
        {
            selectedCells.Add(new Vector2Int(x, y));
            Vector3 worldPosition = GetWorldPosition(x, y);
            linePoints.Add(worldPosition);

            // Update the LineRenderer
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
            Debug.Log("Line point added at position: " + worldPosition);
        }

        if (selectedCells.Count == 6)
        {
            CheckAnswer();
        }
    }

    Vector3 GetWorldPosition(int x, int y)
    {
        // Convert grid position to world position based on the button's RectTransform
        RectTransform cellRect = gridParent.transform.GetChild(x * 5 + y).GetComponent<RectTransform>();
        return cellRect.position;
    }

    void CheckAnswer()
    {
        int numberFormed = 0;
        for (int i = 0; i < 6; i++)
        {
            numberFormed = (numberFormed << 1) | binaryMatrix[selectedCells[i].x, selectedCells[i].y];
        }

        if (numberFormed == targetNumber)
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Incorrect! Try again.");
        }

        // Clear the selection and reset the LineRenderer
        selectedCells.Clear();
        linePoints.Clear();
        lineRenderer.positionCount = 0;
    }
}
