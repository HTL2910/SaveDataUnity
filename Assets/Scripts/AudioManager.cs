using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
    public TMP_InputField audioInputField;
    public List<int> listIndex = new List<int>();
    public float waitTime = 0.4f; // Thời gian chờ giữa các nốt nhạc

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadAllAudioClips();
    }

    void Update()
    {
        // Kiểm tra nếu phím Tab được nhấn
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Kiểm tra nếu TMP_InputField đang được chọn
            if (audioInputField.isFocused)
            {
                TabSuggest();
            }
        }
    }

    public void GetListIndex()
    {
        string tmpString = audioInputField.text;
        listIndex.Clear();
        listIndex = ExtractNumbers(tmpString);
        StartCoroutine(PlayNotesSequentially());
        audioInputField.text = string.Empty;
    }

    void LoadAllAudioClips()
    {
        // Tải tất cả các AudioClip từ thư mục Resources/Audio
        audioClips = Resources.LoadAll<AudioClip>("Notes");

        if (audioClips.Length == 0)
        {
            Debug.LogError("No audio clips found in Resources/Audio");
        }
        else
        {
            Debug.Log(audioClips.Length + " audio clips loaded.");
        }
    }

    IEnumerator PlayNotesSequentially()
    {
        foreach (int index in listIndex)
        {
            if (index >= 0 && index < audioClips.Length)
            {
                audioSource.clip = audioClips[index];
                audioSource.PlayOneShot(audioSource.clip);
                yield return new WaitForSeconds(waitTime); // Chờ thời gian ngắn giữa các nốt nhạc
            }
        }
    }

    public static List<int> ExtractNumbers(string input)
    {
        List<int> numbers = new List<int>();
        Regex regex = new Regex(@"\d+");
        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            if (int.TryParse(match.Value, out int number))
            {
                numbers.Add(number);
            }
        }

        return numbers;
    }

    public void TabSuggest()
    {
        audioInputField.text = "4 3 3 2 3 4 3 1 4 3 3 2 3 4 3 4 4 4 5 6 6 6 6 5 4 5 3 1 4 3 3 2 3 4 3 1 4 3 3 2 3 4 3 4 4 4 5 6 6 6 6 5 4 3 5";
    }
}
