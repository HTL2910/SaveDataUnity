using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelAnimController : MonoBehaviour
{
    public Animator fadePanelAnimator;
    public Animator resultPanelAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OK();
        }
    }
    public void OK()
    {
        if(resultPanelAnimator != null && fadePanelAnimator!=null) 
        {

            fadePanelAnimator.SetBool("Out", true);
            resultPanelAnimator.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        }
    }
    public void GameOver()
    {
        fadePanelAnimator.SetBool("Out", true);
        //fadePanelAnimator.SetBool("Game Over", false);

    }
    private IEnumerator GameStartCo()
    {
        yield return new WaitForSeconds(1f);
        Board board=FindObjectOfType <Board>();
        board.currentStates = GameStates.Move;
    }
}
