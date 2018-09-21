using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour {
    #region 공용필드
    public BoxCollider2D childCollider; // 자식 콜라이더
    public CamScript cam;
    public bool ThrowawayDoor = false; // 일회용문 사용 여부
    public bool isBossRoomDoor; // 이건 보스방문이야?
    public float speed_PassDoor = 1;
    public Enemy_NomalBoss Boss;
    #endregion
    #region 유니티객체저장
    Animator _Anim;
    BoxCollider2D col;
    #endregion
    #region 각종필드
    bool isOpen = false;
    bool Open
    {
        get { return isOpen; }
        set { _Anim.SetBool("Open", isOpen = value); }
    }
    bool Opening
    { get; set; }
    playerController player = null;
    bool isThrowawayDoorUsed = false; // 일회용 문이 사용되면 참
    StageManager stageManager
    {
        get { return StageManager.Instance; }
    }
    #endregion
    #region MonoBehaviour기본매서드
    // Use this for initialization
    void Start () {
        col = GetComponent<BoxCollider2D>();
        _Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Open || isThrowawayDoorUsed)
            return;
        else if (Opening)
            return;
        //
        if(collision.CompareTag("Player"))
        {
            player = collision.GetComponent<playerController>();
            OpenDoor();
        }
    }
    #endregion
    #region 매서드 정의
    public void OpenDoor()
    {
        if(ThrowawayDoor)
        {
            isThrowawayDoorUsed = true;
        }
        player.InputBlock();
        childCollider.enabled = false;
        StartCoroutine(OpenDoorCorutine());
    }
    public void ClosingDoor()
    {
        if(ThrowawayDoor)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        if(isBossRoomDoor)
        {
            col.isTrigger = false;
            col.gameObject.layer = 9;
        }
        player.RequstChangeSpeed(player.walk_Speed);
        player.RequstMove_Stop();
        Debug.Log("리퀘스트무브스탑");
        Open = false;
        Opening = false;
        childCollider.enabled = true;

        if(!isBossRoomDoor)
        {
            player.InputUnBlock();
        }
        else
        {
            player.playWarningAnim();
            Boss.gameObject.SetActive(true);
            cam.XMinValue = 152.5f;
        }
        player = null;
    }
    // 코루틴 하나만든다고 지역만들기 귀찮으니까 여따구현함
    IEnumerator OpenDoorCorutine()
    {
        float openDoorTime = 0f;
      
        Open = true;
        Opening = true;
        while (_Anim.GetCurrentAnimatorStateInfo(0).IsName("BossRoomDoor_4Opened") == false)
        {
            openDoorTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            Debug.Log("탈출");
        }

        Debug.Log("코루틴진입");
        player.RequstChangeSpeed(speed_PassDoor);
        
        if (player.isSeeRight)
           {
           
            player.RequstMove_Right();
            Debug.Log("플레이어 움ㅈ기이는중");
        }
        else { player.RequstMove_Left(); }
        yield return new WaitForSeconds(2f);

        
        ClosingDoor();
        yield break;
    }
    #endregion
}
