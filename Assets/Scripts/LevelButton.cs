using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public bool isActive;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    private Image buttonImage;
    private Button myButton;
    
    public Image[] stars;
    public TextMeshProUGUI levelText;
    public int level;

    private void Start()
    {
        buttonImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        myButton = gameObject.transform.GetChild(0).GetComponent<Button>();
        ActivateStars();
        DecideSprite();
    }
  
    void ActivateStars()
    {
        for(int i=0; i<stars.Length; i++)
        {
            stars[i].enabled = false;
        }
    }
    void DecideSprite()
    {
        if(isActive)
        {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelText.enabled = true;
        }
        else
        {
            buttonImage.sprite= lockedSprite;
            myButton.enabled = false;
            levelText.enabled = false;
        }
    }
 
}
