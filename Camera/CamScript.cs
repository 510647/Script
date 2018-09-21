using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    #region 필드 , 변수
    public playerController playerScript;
    public CameraZoneScript CurrentCameraZone;
    public GameObject[] StateAnim;
    public GameObject player;
    public BoxCollider2D bound;
    public UImanager UI;
    Enemy_NomalBoss Boss;
    //박스콜라이더 최대 / 최소영역
    private Vector3 minBound;
    private Vector3 maxBound;
    //
    public float NextZoneMoveSpeed = 0.5f;
    private float halfWidth;
    private float halfHeight;
    GameObject readyAnim;
    GameObject warningAnim;
    private Camera cam;
    public int CurrentCameraZoneIndex;


    Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.15f;

    public bool YmaxEndbled = false;
    public float YMaxValue = 0;

    public bool YminEndbled = false;
    public float YMinValue = 0;

    public bool XmaxEndbled = false;
    public float XMaxValue = 0;

    public bool XminEndbled = false;
    public float XMinValue = 0;
    #endregion v

    #region MonoBehaviour매서드
    private void Awake()
    {
        UI = FindObjectOfType<UImanager>();
       
    }
    // Use this for initialization
    void Start()
    {
        bound = CurrentCameraZone.camerazoneBox;
        cam = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
        settingReadyAnim();
        settingWarningAnim();
     


    }

    // Update is called once per frame
    void Update()
    {
        Boss = FindObjectOfType<Enemy_NomalBoss>();
        //Vector3 targetPos = tr_Player.position;
        //targetPos.z = transform.position.z;
        //transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        //zm     Debug.Log(targetPos);
        // 최소값엔 반너비 더하고 최대값엔 반너비 뺀다
        //
        //   CurrentCameraZone = CurrentCameraZone.cameraZone;
        CurrentCameraZoneIndex = CurrentCameraZone.CameraZoneIndex;
        
        if(player != null)
        {
            float x = Mathf.Clamp(player.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float y = Mathf.Clamp(player.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
            gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
        }
       
   

        //수직
    }
    #endregion
    #region 매서드존
    public void PlayReadyAnim()
    {
        readyAnim.SetActive(true);
        StartCoroutine(ReadyEndCoroutine());
    }
    public void PlayWarningAnim()
    {
        warningAnim.SetActive(true);
        StartCoroutine(WarningEndCoroutine());
    }
    GameObject CloneAnim(GameObject gameObject , Transform transform)
    {
        return Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
    }

    void settingReadyAnim()
    {
        readyAnim = CloneAnim(StateAnim[0], transform);
        readyAnim.transform.SetParent(this.gameObject.transform);
        readyAnim.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z + 10);
        readyAnim.SetActive(false);
    }
    void settingWarningAnim()
    {
        warningAnim = CloneAnim(StateAnim[1], transform);
        warningAnim.transform.SetParent(this.gameObject.transform);
        warningAnim.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z + 10);
        warningAnim.SetActive(false);
    }

    public void setCameraZone(BoxCollider2D newCameraZoneBox , CameraZoneScript newCameraZone)
    {
        bound = newCameraZoneBox;
        CurrentCameraZone = newCameraZone;
         minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }

    #endregion

    #region 코루틴존
    IEnumerator ReadyEndCoroutine()
    {
 
        yield return new WaitForSeconds(3.25f);
        UI.HUD.gameObject.SetActive(true);
        playerScript.InputUnBlock();

        
    }
    IEnumerator WarningEndCoroutine()
    {
        
        yield return new WaitForSeconds(4.7f);
        playerScript.InputUnBlock();
        UI.Boss_HUD.gameObject.SetActive(true);
        Boss.Fight = true;
    }
    #endregion
}

