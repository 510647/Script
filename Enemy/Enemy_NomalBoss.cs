using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_NomalBoss : Enemy_BossScript {
    // 이 보스엔 중력을 적용하지 않습니다.
    #region 공용객체
    public EnemyScript shield;
    public EnemyBulletScript enemyBullet;
    public GameObject shotEffect;
    public Transform[] shotPos;
    public playerController playerScript;
    public GameObject Beam;
    Transform player;


    public float move_Speed_X = 1f;
    public float move_Speed_Y = 0.5f;
    public float move_DashSpeed_X = 5f;
    public float move_DashSpeed_Y = 5F;

    public float action_GuardTime = 5f; // 실드 부서지면바로끌거임
    public float playerChaseTime = 1.5f;
    public float fire_IntervalTime = 1f; // 사격 간격
    public float rapidFire_IntervalTime = 0.2f; // 연사 사격간격
    public bool EndAppaer = false;

    bool RapidFire = false;
    #endregion
    #region MonoBehaviou 매서드
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        isFlying = true;
        isGround = false;
        Stop_Fall();
        Appearance();
        
    }
    protected override void Update()
    {
        base.Update();
     
        if(isAppear)
        {
            return;
        }
        else if(!Fight)
        {
            return;
        }
        else if (EndAppaer)
        {
        //    isGround = true;
        }
       // Chase();

        if(Health <= 0)
        {
            //  gameObject.SetActive(false);



            BossDead();
        }

        //Appearance();

       
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject playerObject = collision.gameObject;
            playerController player = playerObject.GetComponent<playerController>();
            Debug.Log("데미지");
            if (player.Is_Invincible)
            {

            }
            else if (!player.Is_Invincible) { player.Hurt(Damage); }
        }


    }
    #endregion
    #region 행동 매서드존
    public void Appearance()
    {
        Debug.Log("등자앙");
        Move_Down();
        StartCoroutine(AppearCoroutine());
    }
    public override void Dead()
    {
        base.Dead();
    }
    public void Ground()
    {
        Stop_Jump();
        Stop_Fall();
    }
    // 이동 (좌 , 우 , 정지)
    public  void Move_Right()
    {
        if (!isSeeRight)
        {
            Flip();
        }
        _velocity = new Vector2(move_Speed_X, 0);
        isMoving = true;

    }
    public void Move_Left()
    {
        if (isSeeRight)
        {
            Flip();
        }
        _velocity = new Vector2(-move_Speed_X, 0);
        isMoving = true;

    }
    public void Move_Stop()
    {
        _velocity = new Vector2(0, 0);
        isMoving = false;
      
    }
    // 점프
    public void Move_Up()
    {
        _velocity = new Vector2(_velocity.x, move_Speed_Y);
        isMoving = true;
    }
    public void Move_Down()
    {

            _velocity = new Vector2(_velocity.x, -move_Speed_Y);
            isMoving = true;
        


    }
    public void Stop_Jump()
    {
        isJump = false;
    }
    //낙하
    public void Move_Fall()
    {
        if (_velocity.y > 0)
        {
            _velocity = new Vector2(_velocity.x, 0);
        }

        // 운동상태 갱신
        isJump = false;
        isFall = true;
    }
    public void Stop_Fall()
    {
        isFall = false;
    }
    ////////////////////////
    ///추격 , 공격 , 방어등 보스패턴
    ///////////////////////
    void Chase()
    {
        isGuard = false;
        isAttack = false;
        Co_Chase = StartCoroutine(ChaseCoroutine());
    }
    void StopChase()
    {
        Move_Stop();
    }
    void Attack()
    {
        isAttack = true;
        Co_Attack = StartCoroutine(AttackCoroutine());
    }
    void ShotBullet(Transform shotPostion , Vector3 player)
    {
      //  GameObject shoteff = Instantiate(shotEffect, shotPos[0].position, shotPos[0].rotation);
        //if(isSeeRight)
        //{
        //    Vector3 localScale = shoteff.transform.localScale;
        //    shoteff.transform.localScale = new Vector3(-localScale.x, localScale.y);
        //}

        EnemyBulletScript bullet = Instantiate(enemyBullet, shotPostion.position, shotPostion.rotation) as EnemyBulletScript;
        bullet.isSeeRight = isSeeRight;
        bullet.MoveToPlayer(player);
    }
    void Guard()
    {
        isGuard = true;
        shield.gameObject.SetActive(true);
        Co_Guard = StartCoroutine(GuardCoroutine());
    }
    #endregion
    #region 보조 행동 매서드존
   private void AttackToPlayer()
    {// 폭탄을 발사합니다.
        Vector3 playerPos = player.position;
        Vector2 diff_Player_Boss = playerPos - this.transform.position;
        if (diff_Player_Boss.x < 0 && isSeeRight)
        {
            /// MoveLeft();
            Flip();
        }
        else if (diff_Player_Boss.x > 0 && !isSeeRight)
        {
            /// MoveRight();
            Flip();
        }
        ShotBullet(shotPos[0], player.position);
    }
   private void ChaseToPlayer()
    {
        Vector3 playerPos = player.position;
        Vector2 diff_Player_Boss = playerPos - this.transform.position;
        if(diff_Player_Boss.x < 0)
        {
            Move_Left();
        }
        else if(diff_Player_Boss.x > 0)
        {
            Move_Right();
           
        }
        if (diff_Player_Boss.y > 0)
        {
            Move_Up();
            Debug.Log("상");
        }
        else if(diff_Player_Boss.y < 0)
        {
            Move_Down();
            Debug.Log("하");
        }
        
    }
    void NextAction_Chase()
    {
        Attack();
    }
    void NextAction_Attack()
    {
        Guard();
    }
    void NextAction_Guard()
    {
        Chase();
    }

    public void BossDead()
    {

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);


        playerScript.isWin = true;
        playerScript.Is_Invincible = true;
        playerScript.InputBlock();

    }

    #endregion
    #region 코루틴
    // 공격, 가드 , 등장 , 추적?

    Coroutine Co_Chase;
    Coroutine Co_Attack;
    Coroutine Co_Guard;
    Coroutine Co_Dead;
    IEnumerator AppearCoroutine()
    {
        yield return new WaitForSeconds(4.7f);
     //   en_Collider.isTrigger = true;
        Move_Stop();
      
        isAppear = false;
        EndAppaer = true;
     //   isGround = false;
        Chase();
        yield break;
    }
    IEnumerator ChaseCoroutine()
    {
        float ChaseTime = 0f;
        while(ChaseTime < playerChaseTime)
        {
            ChaseToPlayer();
            ChaseTime += Time.deltaTime;
            yield return false;
        }
        Move_Stop();
        NextAction_Chase();
        Co_Chase = null;
        yield break;
    }
    IEnumerator AttackCoroutine()
    {
        Move_Stop();

        AttackToPlayer();
        yield return new WaitForSeconds(fire_IntervalTime);
        Debug.Log("1");
        AttackToPlayer();
        yield return new WaitForSeconds(fire_IntervalTime);
        AttackToPlayer();
        yield return new WaitForSeconds(fire_IntervalTime);
        AttackToPlayer();
        yield return new WaitForSeconds(fire_IntervalTime);
        AttackToPlayer();
        yield return new WaitForSeconds(fire_IntervalTime);
        Debug.Log("2");

        isAttack = false;
        Co_Attack = null;
        NextAction_Attack();
     
        yield break;
    }
    IEnumerator GuardCoroutine()
    {
        float GuardTime = 0f;
        while (GuardTime < 3f)
        {
          
            GuardTime += Time.deltaTime;
            yield return false;
        }
        isGuard = false;
        shield.gameObject.SetActive(false);
        NextAction_Guard();
        
        Co_Chase = null;
        yield break;
    }
   
    #endregion
}
