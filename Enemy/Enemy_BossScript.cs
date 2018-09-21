using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossScript : EnemyScript {
    #region 변수
    public float jump_Speed = 16f;
    #endregion
    #region 사용할 유니티 객체
    //플레이어와 같은 물리처리
    Rigidbody2D Rgbd
    { get { return GetComponent<Rigidbody2D>(); } }

    Animator Anim
    {
        get { return GetComponent<Animator>(); }
    }
    SpriteRenderer Renderer
    { get { return GetComponent<SpriteRenderer>(); } }

    public Vector2 _velocity
    {
        get { return Rgbd.velocity; }
        set { Rgbd.velocity = value; }
    }
    #endregion
    #region 캐릭터 상태
    public bool _isGround = false;
    bool _isMoving = false;
    bool _isJumping = false;
    bool _isFall = false;
    bool _isGuard = false;
    bool _isAttack = false;
    bool _isFlying = false;
    bool Appearance = true;
   public bool Fight = false;
  
    LayerMask touchTheWall;
     public LayerMask touchTheGround;
    public Transform isGround_Center;
    public Transform isGround_Front;
    public Transform isGround_Back;
    HashSet<EdgeCollider2D> land_Edgeset = new HashSet<EdgeCollider2D>();
    HashSet<BoxCollider2D> land_Boxset = new HashSet<BoxCollider2D>();
    // int maxHp = 100;
    // public bool isHpFull() { return }
    // public int bossHp;
    // 애니매이터와 연결시킵니다.
    // 점프대신 날게하자
    public bool isAppear
    { get { return Appearance; }
       protected set { Appearance = value; }

    }

    public bool isFight
    {
        get { return Fight; }
        protected set { Fight = value; }
    }
    public bool isGround
    { get { return _isGround; }
        protected set { Anim.SetBool("isGround", _isGround = value); }
    }
    public bool isMoving
    {
        get { return _isMoving; }
        protected set { Anim.SetBool("isMoving", _isMoving = value); }
    }
    public bool isFlying
    {
        get { return _isFlying; }
        protected set { Anim.SetBool("isFlying", _isFlying = value); }
    }
    public bool isAttack
    {
        get { return _isAttack; }
        protected set { Anim.SetBool("isAttack", _isAttack = value); }
    }
    public bool isGuard
    {
        get { return _isGuard; }
        protected set { Anim.SetBool("isGuard", _isGuard = value); }
    }
    public bool isFall
    {
        get { return _isFall; }
        protected set { Anim.SetBool("isFall", _isFall = value); }
    }
    public bool isJump
    {
        get { return _isJumping; }
        protected set { Anim.SetBool("isJump", _isJumping = value); }
    }

    #endregion
    #region MonoBehaviour 매서드
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        UpdateState();
    }
    #endregion
    #region 트리거존
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
    /// <summary>
    /// 충돌이 유지되고 있습니다.
    /// </summary>
    /// <param name="collision">충돌 객체입니다.</param>
    protected void OnCollisionStay2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
    /// <summary>
    /// 충돌이 끝났습니다.
    /// </summary>
    /// <param name="collision">충돌 객체입니다.</param>
    protected void OnCollisionExit2D(Collision2D collision)
    {
        UpdatePhysicsState(collision);
    }
    
    #endregion
    #region 행동매서드
    //protected virtual void Ground()
    //{
    //    Stop_Jump();
    //    Stop_Fall();
    //}
    //// 이동 (좌 , 우 , 정지)
    //protected virtual void Move_Right()
    //{
    //    if(!isSeeRight)
    //    {
    //        Flip();
    //    }
    //    isMoving = true;
    //    _velocity = new Vector2(en_Speed, 0);
    //}
    //protected virtual void Move_Left()
    //{
    //    if (isSeeRight)
    //    {
    //        Flip();
    //    }
    //    isMoving = true;
    //    _velocity = new Vector2(-en_Speed, 0);
    //}
    //protected virtual void Move_Stop()
    //{

    //    isMoving = false;
    //    _velocity = new Vector2(0, _velocity.y);
    //}
    //// 점프
    //protected virtual void Move_Jump()
    //{
    //    _velocity = new Vector2(_velocity.x, jump_Speed);
    //    isJump = true;
    //}
    //protected virtual void Stop_Jump()
    //{
    //    isJump = false;
    //}
    ////낙하
    //protected virtual void Move_Fall()
    //{
    //    if (_velocity.y > 0)
    //    {
    //        _velocity = new Vector2(_velocity.x, 0);
    //    }

    //    // 운동상태 갱신
    //    isJump = false;
    //    isFall = true;
    //}
    //protected virtual void Stop_Fall()
    //{
    //    isFall = false;
    //}
    #endregion
    #region 보조매서드
    public bool isCheckPlayingAnimation(string animName) // 애니매이션이 현재 실행중인지 체크합니다.
    {
        return Anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    #endregion
    #region 충돌 처리지역(Test)
    //레이어 마스크가 어느 레이어 마스크에 포함되어있는지 확인합니다.
    void UpdateState() //
    {
        Grounding();
    }


    void UpdatePhysicsState(Collision2D col)
    {
        int layer = col.collider.gameObject.layer; // 충돌하는 물체의 레이어를 검사합니다.
        if (CheckLayerMask(layer, touchTheGround))
        {
            EdgeCollider2D groundCollider = col.collider as EdgeCollider2D; 

            //땅충돌
            if (isTouchGround(groundCollider))
            {
                land_Edgeset.Add(groundCollider);
            }
            else
            {
                land_Edgeset.Remove(groundCollider);
            }
        }
        Debug.Log(layer);
    }
    bool CheckLayerMask(int layerNum, LayerMask layerMask)
    {
        return ((1 << layerNum) & layerMask) != 0;
    }

    bool Grounding()//땅에충돌했는지 처리해봅시다.
    {
        RaycastHit2D ray_Back = Physics2D.Raycast(isGround_Back.position, Vector2.down, 1f, touchTheGround);
        RaycastHit2D ray_Front = Physics2D.Raycast(isGround_Front.position, Vector2.down, 1f, touchTheGround);
        Debug.DrawRay(isGround_Back.position, Vector2.down, Color.red);
        Debug.DrawRay(isGround_Front.position, Vector2.down, Color.red); // 벡터가 꽂히는 방향
        //어떻게체크할지 고민해보자.
        //캐릭터에서 직선을내리고 , 경사면의 법선벡터를 구한다.
        //구한 벡터와 무브벡터의 각도가 예각이면 내려오고 둔각이면 올라간다

        if (isOnGround()) // 각 상태마다 처리합니다.
        {
            if (ray_Back.normal.normalized != ray_Front.normal.normalized) // 앞레이 뒷레이 경사각이 다를때
            {
                bool isHitBackSlope = ray_Back.normal.x == 0; // 후방레이가 충돌한표면의 x노말값이 0이면 true
                RaycastHit2D ray = isHitBackSlope ? ray_Back : ray_Front; // 사실이면 back 거짓이면 front
                Vector2 direction = isSeeRight ? Vector2.right : Vector2.left;
                float rayAngle = Vector2.Angle(direction, ray.normal);
                float rayRadian = Mathf.Deg2Rad * rayAngle; // 구한 레이의 각도를 라디안값으로 변형합니다.

                float sx = en_Speed * Mathf.Cos(rayRadian); // x축미끄러짐
                float sy = en_Speed * Mathf.Sin(rayRadian); // y축 미끄러짐
                float x_direction = isSeeRight ? sx : -sx;
         
                if (isJump) // 점프중
                {

                }
                else if (rayAngle < 90) // 예각
                {
                    float vy = -sy;
                    _velocity = new Vector2(x_direction, vy);
                }
                else if (rayAngle > 90) // 둔각
                {
                    float vy = sy;
                    _velocity = new Vector2(x_direction, vy);
                }
                else { } // 90도 낭떠러지

            }
            else
            {
     
            }
            isGround = true;
       //     Debug.Log("땅충돌했어");
        }
        else if (isJump || isFall)
        {
            isGround = false;
        }
        else if (ray_Back || ray_Front)
        {
           
        
             if (isGround) // 땅에 닿아있을때
            {
                Vector3 pos = transform.position;
                float difY;
                if (ray_Back && !ray_Front) // 후방레이 닿고 전방레이 안닿았을때
                {
                    difY = ray_Back.distance / transform.localScale.y;
                    pos.y -= difY;
                }
                else if (!ray_Back && ray_Front) // 전방레이 닿고 후방레이 안닿았을때
                {
                    difY = ray_Front.distance / transform.localScale.y;
                    pos.y -= difY;
                }
                else // 둘다 닿았을때
                {
                    difY = Mathf.Min(ray_Back.distance, ray_Front.distance) / transform.localScale.y;
                    pos.y -= difY;
                }
                transform.position = pos;
                _velocity = new Vector2(_velocity.x, -Mathf.Abs(_velocity.x) * Mathf.Sin(Mathf.Deg2Rad * 60));
                isGround = true;
            }
            else { isGround = false; }

        }
        else
        {
            isGround = false;
        }

        return isGround;
    }

    bool isOnGround() // 벽충돌은 여기서계산안함.
    {
        if (en_Collider.IsTouchingLayers(touchTheGround)) // 콜라이더가 그라운드를 건드렸을때
        {
            float playerfoot = en_Collider.bounds.min.y; // 플레이어의 발은 콜라이더y점의 최소값입니다. 
            foreach (EdgeCollider2D edge in land_Edgeset)
            {
                float ground_Bottom = edge.bounds.min.y; // 땅의 최소좌표
                float ground_Top = edge.bounds.max.y; // 땅의 최대좌표

                if (ground_Bottom == ground_Top)// 평면
                {
                    if (playerfoot >= ground_Top) // 플레이어의 발이 안닿아있다면 참 반환 / 씬에 그려줄것
                    {
                        Debug.DrawLine(edge.points[0], edge.points[1], Color.red);
                        return true;
                    }
                }
                else// 경사면
                {
                    if ((ground_Bottom <= playerfoot && playerfoot <= ground_Top))
                    {
                        Debug.DrawLine(edge.points[0], edge.points[1], Color.blue);
                        return true;
                    }
                }
            }
            return false;
        }
        return false;


    }
    bool isTouchGround(EdgeCollider2D GroundCol)
    {

        if (en_Collider.IsTouching(GroundCol))
        {

            Bounds groundBounds = GroundCol.bounds;
            if (groundBounds.min.y == groundBounds.max.y)
            {
                float playerBottom = en_Collider.bounds.min.y;
                float groundTop = groundBounds.max.y;
                if (playerBottom >= groundTop) // 플레이어의 발이 땅에닿아있다
                {
                    return true;
                }
                else { return false; } // 아니다
            }
            else
            {
                float playerBottom = en_Collider.bounds.min.y;
                float groundBottom = groundBounds.min.y;
                if (groundBottom <= playerBottom)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        return false;
    } // 벽충돌은 여기서 계산 안함
   
}
     #endregion 다른게임에서도 사용가능하다

