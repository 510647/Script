using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneScript : MonoBehaviour
{
    public Image Fade_In_Image;
    Fade_In_N_Out Fade;
    // Use this for initialization
    void Start()
    {
        Fade = FindObjectOfType<Fade_In_N_Out>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Attack"))
        {
            //         Fade_In_Image.gameObject.SetActive(true);
            Fade.BoolSetting_PlayTitle();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Fade.Boolsetting_ExitGame();
        }
    }

}
