using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Enemy_SpawnZone : MonoBehaviour {

    #region 접근 가능 필드
    public EnemyScript enemyScript;
    public bool _reSpawn = true; // 리스폰 가능구역
    public bool isSeeRight = false;
    #endregion

    #region 필드 정의
    BoxCollider2D box; // 적 소환되는 박스
    EnemyScript enemy; // 구역에 존재하는 적에대한 정보

    bool one = false; // 적이 한번 소환되면 참
    #endregion

    #region 프로퍼티 정의
    public float SpawnZone_Left
    {
        get { return box.bounds.min.x; }
    }
    public float SpawnZone_Right
    {
        get { return box.bounds.max.x; }
    }
    public bool Respawn
    {
        get { return _reSpawn; }
    }
    /// <summary>
    /// 스테이지매니저
    /// </summary>
    /// 
    StageManager script_StageManager
    {
        get { return StageManager.Instance; }
    }


    #endregion

    #region MonoBehaviour 기본 매서드
    // Use this for initialization
    void Start () {
        box = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   //private void OnTriggerEnter2D(Collider2D collision)
   //{
   //    if(collision.CompareTag("CamBox"))
   //    {
   //         Debug.Log("나감");
   //         SpawnEnemy();
   //    }
   //}
   //private void OnTriggerExit2D(Collider2D collision)
   //{
   // if(collision.CompareTag("CamBox"))
   //    {
   //         Debug.Log("나감");
   //        RequestEnemyDestroy();
   //    }
   //}
    #endregion

    #region 매서드집합
    public void SpawnEnemy()
    {
        if(!one || Respawn) // 에네미가 없고 리스폰존 안에플레이어가있다.

        {
            enemy = Instantiate(enemyScript, transform.position, transform.rotation) as EnemyScript;
            enemy.transform.parent = script_StageManager.enemyParent.transform;
            enemy.isSeeRight = isSeeRight;
            enemy.SpawnZone = this;
            one = true;
            
        }
    }

    public void RequestEnemyDestroy()
    {
        if(enemy != null) // 에네미가 존에 존재한다면
        {
            Destroy(enemy.gameObject);
            one = false;
           
        }

    }

    #endregion
}
