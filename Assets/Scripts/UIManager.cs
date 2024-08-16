using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public AudioSource backgroundAudio;
    public AudioSource actionAudio;
    public AudioClip audioclipFall;
    public AudioClip audioclipgood;
    public AudioClip audioclipBoom;
    public AudioClip audioclipBoomColor;
    private void Awake()
    {
        Instance = this;
    }
    public void PlaySound(AudioClip clip)
    {
        actionAudio.PlayOneShot(clip);
    }    
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
