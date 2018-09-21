using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneBoundScript : MonoBehaviour
{
    #region 필드
    public BoxCollider2D[] targetCameraZone;
    public CameraZoneScript[] cameraZone;
    private playerController player;
    private CamScript cam;
    public int NextIndex;
    #endregion
    #region 트리거 & Start매서드
    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<playerController>();
        cam = FindObjectOfType<CamScript>();
      //  cameraZone = FindObjectOfType<CameraZoneScript>();
    }

   //private void OnTriggerStay2D(Collider2D col)
   //{
   //    if (col.CompareTag("Player"))
   //    {
   //       if(cam.CurrentCameraZoneIndex < NextIndex)
   //        {
   //            cam.setCameraZone(targetCameraZone[1] , cameraZone[1] );

   //        }
   //        else if(cam.CurrentCameraZoneIndex == NextIndex)
        
   //        {
   //            cam.setCameraZone(targetCameraZone[0] , cameraZone[0] );

   //        }
   //    }
   //}
    private void OnTriggerExit2D(Collider2D col)
    {
        //if (col.CompareTag("Player"))
        //{

        //    if (cam.CurrentCameraZoneIndex == NextIndex) { }
        //    //else
        //    //{
        //    //    cam.setCameraZone(targetCameraZone[0], cameraZone[0]);

        //    //}
        //}
    }
    #endregion
}
