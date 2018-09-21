using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSceneScript : MonoBehaviour
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Fade.BoolSetting_PlayGame();
        }
    }

}

