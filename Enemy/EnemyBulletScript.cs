using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : EnemyScript {


    Rigidbody2D _rigi;
    Rigidbody2D rigi { get { return GetComponent<Rigidbody2D>(); } }
    public Vector2 _velocity;
    Camera MainCamera;
    float move_Speed = 3f;

    protected override void Start()
    {
        base.Start();
        MainCamera = FindObjectOfType<Camera>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _velocity = rigi.velocity;
        if (MainCamera != null)
        {
            Vector3 camPos = MainCamera.transform.position;
            Vector3 BusterPos = transform.position;
            if (Mathf.Abs(camPos.x - BusterPos.x) > 12) // 총알이 일정거리 날아가고 사라짐.
            {
                Destroy(gameObject);
            }
        }
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void MoveToPlayer(Vector3 player)
    {
        Vector3 BulletPos = transform.position;
        Vector3 diff_Ppos_Bos = player - BulletPos; 

        if(isSeeRight && diff_Ppos_Bos.x < 0)
        {
            Destroy(gameObject);
            Debug.Log(isSeeRight);
        }
        else if(!isSeeRight && diff_Ppos_Bos.x > 0)
        {
            Destroy(gameObject);
            Debug.Log(isSeeRight);
        }
        rigi.velocity = diff_Ppos_Bos * move_Speed;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject playerObject = col.gameObject;
            playerController player = playerObject.GetComponent<playerController>();
            Debug.Log("데미지");
            if (player.Is_Invincible)
            {
                Destroy(gameObject);
            }
            else if (!player.Is_Invincible) { player.Hurt(Damage); Destroy(this.gameObject); }


        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9 || collision.gameObject.layer == 8)
        {
            Destroy(this.gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
