using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade_In_N_Out : MonoBehaviour {
   public  playerController player;
    Animator _Anim
    {
        get { return GetComponent<Animator>(); }
    }
    bool _PlayTitle;

    bool _PlayGame;

    bool _BossDead;

    bool _ExitGame;

    bool _PlayTutorial;


    bool PlayTitle { get { return _PlayTitle; } set { _Anim.SetBool("PlayTitle", _PlayTitle = value); } }
    bool PlayGame { get { return _PlayGame; } set { _Anim.SetBool("PlayGame", _PlayGame = value); } }
    bool BossDead { get { return _BossDead; } set { _Anim.SetBool("BossDead", _BossDead = value); } }
    bool ExitGame { get { return _ExitGame; } set { _Anim.SetBool("ExitGame", _ExitGame = value); } }
    bool PlayTutorial { get { return _PlayTutorial; } set { _Anim.SetBool("PlayTutorial", _PlayTutorial = value); } }
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<playerController>();
       
        
	}

    // Update is called once per frame
    void Update() {



        

	}
    public void loadNextScene()
    {
        SceneManager.LoadScene(1);
    }
    public void loadTitleScene()
        {
        SceneManager.LoadScene(0);
        }
    public void loadEndingScene()
    {
        SceneManager.LoadScene(2);
    }
    public void loadTutorialScene()
    {
        SceneManager.LoadScene(3);
    }
    public void loadExitGame()
    {
        Application.Quit();
    }
    public void BoolSetting_PlayTitle()
    {
        PlayTitle = true;
    }
    public void BoolSetting_PlayGame()
    {
        PlayGame = true;
    }
    public void BoolSetting_BossDead()
    {
        BossDead = true;
    }
    public void Boolsetting_ExitGame()
    {
        ExitGame = true;
    }
    public void Boolsetting_PlayTutorial()
    {
        PlayTutorial = true;
    }
}
