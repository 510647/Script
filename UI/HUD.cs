using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UImanager {

  
 
    public Text text_Life;
    // 상태보드
    public GameObject StatusBoard;
    // 체력바 보드
    public GameObject HPBoard_head;
    public GameObject HPBoard_body;
    public GameObject HPBoard_hpBar;


     void Start()
    {
        player = FindObjectOfType<playerController>();
    }
     void Update()
    {
        
        
            Vector3 hpBar_Scale = HPBoard_hpBar.transform.localScale;
            hpBar_Scale.y = (float)player.Health / player.MaxHealth;
 
            HPBoard_hpBar.transform.localScale = hpBar_Scale;
        
   
    }


    #region
    public void UpdateStatus_LifeText()
    {
        //  text_Life.text = string.Format("{0:D2}" , game)
    }

    void UpdateStatus_StatusBoard()
    {

    }
    #endregion
}
