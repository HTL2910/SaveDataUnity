using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelAnimController : MonoBehaviour
{
    public Animator fadePanelAnimator;
    public Animator resultPanelAnimator;

    public void OK()
    {
        if(resultPanelAnimator != null && fadePanelAnimator!=null) 
        {
            fadePanelAnimator.SetBool("Out", true);
            resultPanelAnimator.SetBool("Out", true);
        }
    }
}
