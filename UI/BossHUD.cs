using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHUD : UImanager {

    public Text text_Life;
    // 상태보드
    public GameObject StatusBoard;
    // 체력바 보드
    public GameObject HPBoard_head;
    public GameObject HPBoard_body;
    public GameObject HPBoard_hpBar;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Boss = FindObjectOfType<Enemy_NomalBoss>();
        if(Boss != null)
        {
            Vector3 hpBar_Scale = HPBoard_hpBar.transform.localScale;
            hpBar_Scale.y = (float)Boss.Health / Boss.MaxHealth;

            HPBoard_hpBar.transform.localScale = hpBar_Scale;
        }
  

    }
}
