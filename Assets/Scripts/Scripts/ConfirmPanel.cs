using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour
{
    
    public Image[] stars;
    private void Start()
    {
        ActivateStars();


    }
    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
    
    void ActivateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }
    }
}
