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
       
        DecideSprite();
    }
  
    public void ActivateStars(int count,bool isActive)
    {
        Debug.Log("ActivateStar");
        for(int i=0; i< count; i++)
        {
            stars[i].enabled = isActive;
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
