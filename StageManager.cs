using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.CompilerServices;

public class StageManager : MonoBehaviour {
    /// <summary>
    /// 준비 , 보스방에 들어갔을때 경고 , 보스방문 등 각종 스테이지오브젝트의 관리자입니다.
    /// </summary>
    /// 
    #region 공용객체
   public GameObject enemyParent;
    #endregion
    #region 참조보관
    public static StageManager Instance
    {
        get { return GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>(); }
    }
  
    
    Gamemanager gamemanager;
    protected Gamemanager GameManager { get { return gamemanager; } }

    //   public ScreenFader fader;
    #endregion
    #region 프로퍼티 정의

    #endregion
    #region MonoBehaviour 기본매서드
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #endregion
}
