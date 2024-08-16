using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public AudioSource backgroundAudio;//sound
    public AudioSource actionAudio;//audio
    public AudioClip audioclipFall;
    public AudioClip audioclipgood;
    public AudioClip audioclipBoom;
    public AudioClip audioclipBoomColor;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        backgroundAudio.volume= PlayerPrefs.GetFloat("Volume Sound", 0.5f);
        actionAudio.volume= PlayerPrefs.GetFloat("Volume Audio", 0.5f);
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
