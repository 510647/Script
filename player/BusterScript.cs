using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // 게임오브젝트에 자동으로 리지드바디를 추가한다.

public class BusterScript : playerAttackScript {

    #region

    public int Damage; // 탄환이 가지고있는ㄷ 데미지;

    public Camera MainCamera;

    Collider2D _col;

    Rigidbody2D _rigi;


    #endregion

    #region 공용필드정의
    public LayerMask Ground_N_Wall;
    #endregion


    #region 필드 정의
    //public GameObject ReflectedEffect { get { return effects[0]; } } // 총알이 반사당할때 나타나는 이펙트

    //public GameObject HitEffect { get { return effects[1]; } } // 총알이 성공적으로 타격되면 나타나는 이펙트
    #endregion

    #region MonoBehaviour 기본 매서드
    // Use this for initialization
    protected override void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rigi = GetComponent<Rigidbody2D>();
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (MainCamera != null)
        {
            Vector3 camPos = MainCamera.transform.position;
            Vector3 BusterPos = transform.position;
            if(Mathf.Abs(camPos.x - BusterPos.x) > 12) // 총알이 일정거리 날아가고 사라짐.
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            Debug.Log("충돌");
            EnemyScript enemy = col.gameObject.GetComponent<EnemyScript>();
            HitParticle(_rigi.velocity.x < 0, transform);
          
            enemy.GetHurt(damage);
            if(enemy.Life)
            {
                Destroy(gameObject);
            }
        }
        else if(_col.IsTouchingLayers(Ground_N_Wall))
        {
            ReflectParticle(_rigi.velocity.x < 0, transform);
            Destroy(gameObject);
        }
    }
    #endregion

    #region

    #endregion
}
