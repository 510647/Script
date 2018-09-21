using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneScript : MonoBehaviour {


    #region 필드
    //public BoxCollider2D[] targetCameraZone;
    public int CameraZoneIndex;
    private playerController player;
    private CamScript cam;
    public CameraZoneScript cameraZone;
    public BoxCollider2D camerazoneBox;
    #endregion

    #region MonoBehaviour & 트리거 매서드

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<playerController>();
        cam = FindObjectOfType<CamScript>();
      //  camerazoneBox = GetComponent<BoxCollider2D>();
	}
    private void Update()
    {
     //   cameraZone = FindObjectOfType<CameraZoneScript>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            cam.setCameraZone(camerazoneBox, this);

        }
    }
    #endregion

}
