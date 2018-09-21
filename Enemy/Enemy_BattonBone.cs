using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BattonBone : EnemyScript {
    #region 유니티 객체 정의
    Rigidbody2D rigi;
    Transform player;
    
    #endregion
    #region 공용필드
    public float move_Speed = 5f;
    public float amplitude_top = 2f;
    public float amplitude_bottom = 1f;
    public float LifeTime = 0f;
    #endregion

    #region MonoBehaviour 기본 매서드
    // Use this for initialization
    protected override void Start () {
        base.Start();
        rigi = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
     //   player = GetComponent<playerController>();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (Health <= 0)
        {
            Die = true;
        }
        if (Die)
        {
            Dead();
        }

        Vector2 diPos = player.gameObject.transform.position - transform.position;
        float angle = Mathf.Atan2(diPos.y, diPos.x);
        float distortion = Random.Range(amplitude_bottom, amplitude_top) * Mathf.Sin(3 * LifeTime);

        float velocity_X = move_Speed * Mathf.Cos(angle);
        float velocity_Y = move_Speed * Mathf.Sin(angle) + distortion;
        rigi.velocity = new Vector2(velocity_X, velocity_Y);

        if(diPos.x < 0 && isSeeRight)
        {
            Flip();
        }
        if(diPos.x > 0 && !isSeeRight)
        {
            Flip();
        }

        LifeTime += Time.deltaTime;

	}
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    #endregion
    #region 트리거구역
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject playerObject = col.gameObject;
            playerController player = playerObject.GetComponent<playerController>();
            Debug.Log("데미지");
            if (player.Is_Invincible)
            {

            }
            else if (!player.Is_Invincible) { player.Hurt(Damage); }


        }
    }
    #endregion

    public override void Dead()
    {
        int itemDrop = Random.Range(0, Items.Length);
        if (Items[itemDrop] != null)
        {
            DropItem(Items[itemDrop]);
            Debug.Log(itemDrop);
        }
        
        base.Dead();
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

}
