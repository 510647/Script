using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))] // 적 스폰을 관리하는 스크립트입니다.
public class CamBoxScript : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemySpawnZone"))
        {
            Debug.Log("충돌");
            Enemy_SpawnZone zone = other.gameObject.GetComponent<Enemy_SpawnZone>();
            zone.SpawnEnemy();
        }
    }
    /// <summary>
    /// 충돌체가 여전히 트리거 내부에 있습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerStay2D(Collider2D other)
    {

    }
    /// <summary>
    /// 충돌체가 트리거 내부로 진입했습니다.
    /// </summary>
    /// <param name="other">자신이 아닌 충돌체 개체입니다.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("EnemySpawnZone"))
        {
            Enemy_SpawnZone zone = other.gameObject.GetComponent<Enemy_SpawnZone>();
            zone.RequestEnemyDestroy();
        }
    }

    private void Update()
    {
    
    }
}
