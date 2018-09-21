using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Canon : EnemyScript {
    // 쏘기만하는 몬스터들 입니다.
    // Use this for initialization

    #region 필드
    public GameObject ShotEffect;
    public Transform shotPos;
    public EnemyBulletScript bullet;
    private playerController player;
    #endregion

    #region MonoBehaviour 매서드
    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<playerController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Health <= 0)
        {
            Die = true;
        }
        if (Die)
        {
            Dead();
        }
  
    }
    #endregion
    #region 트리거
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
    #region 매서드
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
    public void Shot()
    {
        GameObject Shot_effect = Instantiate(ShotEffect, shotPos.position, shotPos.rotation);
        EnemyBulletScript enemy_Bullet = Instantiate(bullet, shotPos.position, shotPos.rotation) as EnemyBulletScript;
        if(isSeeRight)
        {
            Vector2 localScale = Shot_effect.transform.localScale;
            Shot_effect.transform.localScale = new Vector2(-localScale.x, localScale.y);

        }
     //   enemy_Bullet = Instantiate(bullet, shotPos.position, shotPos.rotation);
        bullet.isSeeRight = isSeeRight;
   
        enemy_Bullet.MoveToPlayer(player.gameObject.transform.position);
    }
    #endregion
}
