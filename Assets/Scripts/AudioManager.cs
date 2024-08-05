using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
    public TMP_InputField audioInputField;
    public List<int> listIndex = new List<int>();
    public float waitTime ; // Thời gian chờ giữa các nốt nhạc

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
            
              TabSuggest();
            
        }
    }

    public void GetListIndex()
    {
        int number = 0;
        if (audioInputField.text == null || audioInputField.text == string.Empty)
        {
            number = 50;
            if (int.TryParse(audioInputField.text, out int count))
            {
                number = count;
            }
            PlaySoundCount(number);
        }
        else
        {
            string tmpString = audioInputField.text;
            listIndex.Clear();
            listIndex = ExtractNumbers(tmpString);
            StartCoroutine(PlayNotesSequentially());
            audioInputField.text = string.Empty;
        }
    }
    public void PlaySound(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.PlayOneShot(audioSource.clip);
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
            if (index >= 0 && index <= 24)
            {
                audioSource.clip = audioClips[index-1];
                audioSource.PlayOneShot(audioSource.clip);
                waitTime = UnityEngine.Random.Range(0.357f, 0.5f);
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
    private void PlaySoundCount(int count)
    {
        audioInputField.text = string.Empty;
        if (count > 10)
        {
            for (int i = 0; i < count; i++)
            {
                int tmpCount = UnityEngine.Random.Range(12, 24);
                audioInputField.text += " "+tmpCount.ToString();
            }
        }
        Debug.Log(audioInputField.text);
        GetListIndex();
        
    }    
}
