using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour {
    // 게임을 시작하는 동시에 프리펩들을생성시킵니다.

    public playerController player;
    public Image Faid_in_Image;
    Fade_In_N_Out Fade;
    int TitleScene = 0;
    private void Start()
    {
        player = FindObjectOfType<playerController>();
        Fade = FindObjectOfType<Fade_In_N_Out>();
    }
    private void Update()
    {
        if(player.isDead)
        {
            Invoke("ReturnTitle", 3f);
       //     ReturnTitle();
        }
        if(player.isWin)
        {
            Invoke("Ending", 3f);
        }
    }

    public void ReturnTitle()
    {
        // Faid_in_Image.gameObject.SetActive(true);
        Fade.BoolSetting_PlayTitle();
    }
    public void Ending()
    {
        Fade.BoolSetting_BossDead();
    }

}
