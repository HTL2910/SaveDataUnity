using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public string stringToLoad;
    public GameObject setting;

    public float volumeAudio;
    public float volumeSoundBackground;

    public Slider volumeAudioSlider;
    public Slider volumeSoundSlider;

    public AudioSource audioSoure;
    public AudioSource soundSoure;
    private void Start()
    {
        volumeAudioSlider.value = volumeAudio;
        volumeSoundSlider.value = volumeSoundBackground;
        volumeAudioSlider.onValueChanged.AddListener(OnVolumeAudioChanged);
        volumeSoundSlider.onValueChanged.AddListener(OnVolumeSoundChanged);
    }
    public void OnVolumeAudioChanged(float value)
    {
        volumeAudio = value;
        audioSoure.volume = value;
        audioSoure.Play();
        soundSoure.Stop();
    }
    public void OnVolumeSoundChanged(float value)
    {
        volumeSoundBackground=value;
        soundSoure.volume=value;
        soundSoure.Play();
        audioSoure.Stop();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(stringToLoad);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OK()
    {
        setting.SetActive(false);
        PlayerPrefs.SetFloat("Volume Audio", volumeAudio);
        PlayerPrefs.SetFloat("Volume Sound", volumeSoundBackground);
    }
    public void Setting()
    {
        setting.SetActive(true);
    }
}
