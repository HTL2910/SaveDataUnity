using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Thêm thư viện UI để sử dụng Toggle

public class HintManager : MonoBehaviour
{
    private Board board;
    public float hintDelay;
    private float hintDelaySeconds;
    public GameObject hintParticle;
    public GameObject currentHint;
    public List<GameObject> possibleMoves;
    public Toggle autoPlayToggle;  // Toggle để kích hoạt auto-play
    public bool autoPlayEnabled = false;

    void Start()
    {
        board = FindObjectOfType<Board>();
        hintDelaySeconds = hintDelay;

        // Thêm listener cho toggle auto-play
        if (autoPlayToggle != null)
        {
            autoPlayToggle.onValueChanged.AddListener(delegate { ToggleAutoPlay(autoPlayToggle.isOn); });
        }
        InvokeRepeating("Auto", 1f, hintDelay-1f); // Adjust the time interval as needed (5 seconds in this case)

    }

    private void Update()
    {
        hintDelaySeconds -= Time.deltaTime;

        // Nếu auto-play được bật, thực hiện nước đi tự động
       
        if (hintDelaySeconds <= 0 && currentHint == null)
        {
            MarkHint();
            hintDelaySeconds = hintDelay;
        }
    }

    protected void Auto()
    {
        // If auto-play is enabled, call the AutoPlay function
        if (autoPlayEnabled)
        {
            StartCoroutine(AutoPlayWithDelay());
        }
    }

    // Coroutine to handle the auto-play with a delay
    private IEnumerator AutoPlayWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Delay between each auto-play action

        AutoPlay(); // Call the AutoPlay function after the delay
    }
    List<GameObject> FindAllMatches()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    // Kiểm tra các hướng để tìm nước đi hợp lệ
                    if (i < board.width - 1 && board.SwitchAndCheck(i, j, Vector2.right))
                    {
                        possibleMoves.Add(board.allDots[i, j]);
                    }
                    else if (i >= 1 && board.SwitchAndCheck(i, j, Vector2.left))
                    {
                        possibleMoves.Add(board.allDots[i, j]);
                    }
                    if (j < board.height - 1 && board.SwitchAndCheck(i, j, Vector2.up))
                    {
                        possibleMoves.Add(board.allDots[i, j]);
                    }
                    else if (j >= 1 && board.SwitchAndCheck(i, j, Vector2.down))
                    {
                        possibleMoves.Add(board.allDots[i, j]);
                    }
                }
            }
        }
        return possibleMoves;
    }

    GameObject PickOneRandomly()
    {
        possibleMoves.Clear();
        possibleMoves = FindAllMatches();
        if (possibleMoves.Count > 0)
        {
            int pieceToUse = Random.Range(0, possibleMoves.Count);
            return possibleMoves[pieceToUse];
        }
        return null;
    }

    private void MarkHint()
    {
        GameObject move = PickOneRandomly();
        if (move != null)
        {
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);
            currentHint.transform.parent = this.transform;
        }
    }

    public void DestroyHint()
    {
        if (currentHint != null)
        {
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;
        }
    }

    // Hàm tự động thực hiện nước đi
    private void AutoPlay()
    {
        GameObject move = PickOneRandomly();
        if (move != null)
        {
            // Tìm vị trí của dot và thử di chuyển theo 4 hướng
            for (int i = 0; i < board.width; i++)
            {
                for (int j = 0; j < board.height; j++)
                {
                    if (board.allDots[i, j] == move)
                    {
                        // Thử di chuyển theo các hướng
                        if (i < board.width - 1 && board.SwitchAndCheck(i, j, Vector2.right))
                        {
                            move.GetComponent<Dot>().MovePiecesActual(Vector2.right);
                            return;  // Kết thúc nếu thành công
                        }
                        else if (i >= 1 && board.SwitchAndCheck(i, j, Vector2.left))
                        {
                            move.GetComponent<Dot>().MovePiecesActual(Vector2.left);
                            return;  // Kết thúc nếu thành công
                        }
                        else if (j < board.height - 1 && board.SwitchAndCheck(i, j, Vector2.up))
                        {
                            move.GetComponent<Dot>().MovePiecesActual(Vector2.up);
                            return;  // Kết thúc nếu thành công
                        }
                        else if (j >= 1 && board.SwitchAndCheck(i, j, Vector2.down))
                        {
                            move.GetComponent<Dot>().MovePiecesActual(Vector2.down);
                            return;  // Kết thúc nếu thành công
                        }
                    }
                }
            }
        }
    }

    // Hàm bật/tắt chế độ auto-play
    public void ToggleAutoPlay(bool isOn)
    {
        autoPlayEnabled = isOn;
    }
}
