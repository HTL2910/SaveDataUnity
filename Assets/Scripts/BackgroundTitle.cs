using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTitle : MonoBehaviour
{
    #region
    //tip

    //1 q,w,a,s,d,e
    //2 Alt+ Move arrow (lên xuống dòng code)
    //3 Layout: Animation
    //4 Invoke
    //5 InvokeRepeating 
    //6 nameof()
    //7 Ctrl+ R+R to rename of()
    //8 Ctrl+Shift + N : create empty GameObject
    //9 F lấy nét object, Shift F theo dõi
    //10 chọn Camera và Ctrl Shift F để căn chỉnh View 
    //11 C chuyển animation( keyframe->curve)
    //12 Ctrl K C và Ctrl K U : comment code và uncomment
    //13 Ctrl P: play
    //Ctrl Shift P:Pause
    //Ctrl Alt P: tiền từng frame
    //14 [SerializeField view variable và HideInspector: hide
    //15 GameObject: ---------Name-------- view dễ hơn
    //16 [Header] và [Space] không gian
    //17 mở 2 scene cùng lúc
    //18 [ToolTips] hiển thị thông tin
    //19 [Range] khoảng giá trị
    //20 [Min] giá trị tối thiểu
    //21 dấu "/" tạo menu con
    //22 //Region và //endRegion: tạo một menu thả xuống để sắp xếp mã
    //23 discard changes in scene view
    //24 [ContextMenu("Function/Test");

    //tip
    #endregion
    private SpriteRenderer _spriteRenderer;
    public int hitPoints;
    private GoalsManager _goalsManager;
    private void Start()
    {
        _goalsManager=FindObjectOfType<GoalsManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (hitPoints <= 0)
        {
            if (_goalsManager != null)
            {
                _goalsManager.CompareGoal(this.gameObject.tag);
                _goalsManager.UpdateGoal();
            }
            Destroy(gameObject);
        }

    }
    public void TakeDamage(int damage)
    {
        hitPoints-=damage;
        MakeLighter();
    }
    void MakeLighter()
    {
        Color color=_spriteRenderer.color;
        float newAlpha=color.a*0.5f;
        _spriteRenderer.color=new Color(color.r,color.g,color.b,newAlpha);
    }
}
