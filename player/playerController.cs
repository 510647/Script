using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    #region 고정값
    public const float DASH_BEG_TIME = 0.056f; // 애니매이션 길이
    public const float DASH_RUN_TIME = 0.444f;
    public const float DASH_END_TIME = 0.250f;
    public const float DASH_AFTERIMAGE = 0.05f; // 잔상이 유지되는 최대시간
    const float FLASH_TIME = 0.05f; // 공격할떄 플레이어가 잠깐 반짝임.
    public float[] CHARGE_TIME = { 0.2f, 0.5f, 1.2f };
    public float SHOT_ENDTIME = 0.54f;
    public float WALLJUMP_TIME = 0.13f;
    public float JUMP_MAXTIME = 0.13f;
    #endregion

    #region 변수 정의 / 공용 사용 가능 객체
    public float move_Speed = 5f;
    public float walk_Speed = 5f;
    public float dash_Speed = 12f;
    public float jump_Speed = 16f;
    public float jump_DecSpeed = 0.8f;
    public float push_Speed = 0.8f; // 대쉬 멈추기직전스피드
    float dash_AfterImage = 0f;
    public float slip_Speed = 3f;
    float charge_Time = 0f; // 차지시간
    float buster_Speed = 20f;
    float shot_Time = 0;
    public int p_Health = 40;
    public int p_maxHealth = 40;
    public int p_dangerHealth = 10; // 플레이어가 위험상태에 돌입하는 체력
    public float invencibleTime = 0;

    public GameObject[] effects;
    public GameObject[] Busters;
    public GameObject[] StateAnim;

    public BoxCollider2D DefaultBox;
    public BoxCollider2D DashBox;
    public BoxCollider2D pushCheckBox; //밀고있나확인하는박스

     CamScript cam;

    public Transform DashBoostPos;
    public Transform DashFogPos;
    public Transform DashBoostStay;
    public Transform SlipFogPos;
    public Transform SlipFogStay;
    public Transform shot_Pos;
    public Transform shotPos_Walk;
    public Transform shotPos_Dash;
    public Transform shotPos_Slip;
    public Transform shotPos_Jump;
    public Transform shotPos_Crouch;
    public Transform isGround_Center;
    public Transform isGround_Front;
    public Transform isGround_Back;
    public Transform ChargeEffectPos;
    public DeadParticle DeadParticle;

    GameObject Ef_dashBoost = null;
    GameObject Ef_dashFog = null;
    GameObject Ef_SlipFog = null;
    GameObject Ef_Charge_Lv1 = null;
    GameObject Ef_Charge_Lv2 = null;

    public Vector2 _velocity
    {
        get { return _Rgbd.velocity; }
        set { _Rgbd.velocity = value; }
    }


    public LayerMask touchTheGround;
    public LayerMask touchTheWall;
    float DashAfterImage_Time // 잔상이 머문 시간 저장하는값
    { get { return dash_AfterImage; } set { dash_AfterImage = value; } }

    HashSet<EdgeCollider2D> land_Edgeset = new HashSet<EdgeCollider2D>();
    HashSet<BoxCollider2D> land_Boxset = new HashSet<BoxCollider2D>();
    #endregion

    #region 각종 상태값
    bool _isSeeRight = true; // 플레이어가 오른쪽을 보고 있습니까?
    bool _isMoving = true;
    public bool _isJumping = false;
    public bool _isGround = false;
    public bool _isIdle = false;
    bool _isDanger = false;
    public bool _isFall = false;
    public bool _isDash = false;
    bool _isCrouch = false;
    public bool _isDashBlock = false;
    public bool _isMoveBlock = false;
    public bool _isJumpBlock = false;
    bool _isWallJump = false;
    public bool _isPusy = false; // 벽을 밀고있나?
    public bool _isSlip = false;
    bool _isShot = false;
    bool _isShotPressed = false; // 샷키가 눌려있나?
    bool _isShotBlock = false;
    bool _isChargeBuster = false; // 버스터 차지중?
    bool _isReady = false; // 준비중?
    bool _isLife = true;
    bool _isDaed = false;
    bool _isDamaged = false;
    bool _isInvincible = false;
    bool _isWin = false;
    #endregion

    #region 각종 상태값 정의 
    // 상태값이 변경되면 애니매이터에 바로 전달합니다.
    public bool isSeeRight { get { return _isSeeRight; } private set { _isSeeRight = value; } } // 왼쪽 :? 오른쪽?
    public bool isMoving { get { return _isMoving; } set { _Anim.SetBool("isMoving", _isMoving = value); } } // 움직이고있어?
    public bool isJumping { get { return _isJumping; } set { _Anim.SetBool("isJumping", _isJumping = value); } } // 점프하고있어?
    public bool isGround { get { return _isGround; } set { _Anim.SetBool("isGround", _isGround = value); } } // 땅에닿았어 ? 
    public bool isDanger { get { return _isDanger; } set { _Anim.SetBool("isDanger", _isDanger = value); if (_isDanger) { _Anim.SetFloat("DangerState", 1); } else { _Anim.SetFloat("DangerState", 0);} } } // 위험해?
    public bool isFall { get { return _isFall; } set { _Anim.SetBool("isFall", _isFall = value); } }
    public bool isDash { get { return _isDash; } set { _Anim.SetBool("isDash", _isDash = value); } }
    public bool isWallJump { get { return _isWallJump; } set { _Anim.SetBool("isWallJump", _isWallJump = value); } }
    public bool isSlip { get { return _isSlip; } set { _Anim.SetBool("isSlip", _isSlip = value); } }
    public bool isCrouch { get { return _isCrouch; } set { _Anim.SetBool("isCrouch", _isCrouch = value); } }
    public bool isPusy { get { return _isPusy; } set { _Anim.SetBool("isPusy", _isPusy = value); } }
    public bool isLife() { return 0 < p_Health; }
    public bool isHealthFull() { return p_Health == p_maxHealth; }
    public bool isDead { get { return _isDaed; } private set { _isDaed = value; } }
    public bool isReady { get; private set; }
    public bool isDamaged { get { return _isDamaged; } set { _Anim.SetBool("isDamaged", _isDamaged = value); } }
    public bool isWin { get { return _isWin; } set { _Anim.SetBool("Win!", _isWin = value); } }
    bool isDashBlock { get; set; } // 대쉬가 막혀있는가?
    bool isMoveBlock { get { return _isMoveBlock; } set { _isMoveBlock = value; } } // 이동이 막혀있는가?
    bool isJumpBlock { get; set; }
    bool isSlipBlock { get; set; }
    bool IsShot { get { return _isShot; } set { _Anim.SetBool("IsShot", _isShot = value); } }
    bool IsShotBlock { get { return _isShotBlock; } set { _isShotBlock = value; } }
    bool IsChargeBuster { get { return _isChargeBuster; } set { _isChargeBuster = value; } }
    bool isDashJump { get; set; }
    public bool Is_Invincible{get; set;}
    float ShotTime { get { return shot_Time; } set { _Anim.SetFloat("ShotTime", shot_Time = value); } }
    float InvencibleTime { get { return invencibleTime; } set { invencibleTime = value; } }
    public int Health
    {
        get { return p_Health; }
        private set { p_Health = Mathf.Clamp(value, 0, p_maxHealth); }
    }
    public int MaxHealth
    { get { return p_maxHealth; }
        private set { p_maxHealth = value; }
    }
    public int Danger_Health
    {
        get { return p_dangerHealth; }
    }
    bool inputBlocked { get; set; }
    
    #endregion

    #region 플레이어가 사용할 유니티 객체에대한 정의

    Rigidbody2D _Rgbd
    { get { return GetComponent<Rigidbody2D>(); } }

    Animator _Anim
    {
        get { return GetComponent<Animator>(); }
    }

    Collider2D _Collider
    { get { return GetComponent<Collider2D>(); } }

    SpriteRenderer _Renderer
    { get { return GetComponent<SpriteRenderer>(); } }


    #endregion

    #region MonoBehaviour 기본 매서드
     void Awake()
    {
        isReady = true;
    }
    void Start()
    {
        ShotTime = 100;
        cam = FindObjectOfType<CamScript>();


    }
    void Update()
    {
       
       if (isReady)
       {
            InputBlock();
            if (isCheckPlayingAnimation("Idle"))
                EndReady();
            
       }

        if (isDash)
        {
            if (DashAfterImage_Time < DASH_AFTERIMAGE)
            {
                DashAfterImage_Time += Time.deltaTime;

            }
            else
            {
                GameObject DashAfterImage = CreateEffect(effects[4], transform); //  생성후 위치설정한뒤 켜준다
                Vector3 Scale = DashAfterImage.transform.localScale;
                if (isSeeRight == false) { Scale.x *= -1; }
                DashAfterImage.transform.localScale = Scale;
                DashAfterImage.SetActive(false);

                var imageRenderer = DashAfterImage.GetComponent<SpriteRenderer>();
                imageRenderer.sprite = _Renderer.sprite;
                DashAfterImage.SetActive(true); // 활성화
                DashAfterImage_Time = 0f;

            }
        }
     
        ShotTime += Time.fixedDeltaTime;


        if (IsKeyDown("Jump"))
        {
            if (isJumpBlock)
            {

            }
            else if (isSlip)
            {
         
                move_WallJump();

            }
           else if(isDash)
           {
               move_DashJump();
           }
           else
            {
                move_Jump();
            }
        }
        else if (IsKeyDown("Dash"))
        {
            if (isDashBlock)
            { }
            else { DashStart(); }

        }

        if (p_Health > p_dangerHealth)
        {
            isDanger = false;
        }
    }
    void FixedUpdate() // 물리에효과가 적용된 오브젝트는 여기에!
    {
        _Velocity = _Rgbd.velocity;
        //방향키 입력처리
        if (isDash)
        {
            if (isGround == false) // 대쉬중 캐릭터가 공중에떳을때
            {
                if (IsLeftKeyPressd())
                {
                    move_Left();
                }
                else if (IsRightKeyPressd())
                {
                    move_Right();
                }
                else if (isSlipBlock && isWallJump)
                {

                }
                else
                {
                    move_Stop();
                }
            }
        }
        else if (isMoveBlock) // 움직임이 막혔을때
        {

        }
        else // 그 외
        {
            if (IsLeftKeyPressd())
            {
                if (isSeeRight == false && isPusy)
                {
                    move_Stop();
                }
                else
                {
                    if (isSlip) { }
                    else
                    { move_Left(); }
                }

            }
            else if (IsRightKeyPressd())
            {
                if (isSeeRight == true && isPusy)
                {
                    move_Stop();
                }
                else
                {
                    if (isSlip)
                    {

                    }
                    else
                    {
                        move_Right();
                    }

                }

            }
            else if (IsDownKeyPressd() && isGround)
            {
                move_Crouch();
            }
            else if (Requstmove) { }
            else { move_Stop(); }
        }





        //각종 상태에따라 물리상태를 정의합ㄴ디ㅏ./

        if (isJumping)
        {
            if (isPusy)
            {
                if (isSlipBlock)
                {
                    _velocity = new Vector2(_velocity.x, _velocity.y - jump_DecSpeed);
                }
                else { slip_Start(); }

            }
            else if (IsKeyPressd("Jump") == false || _velocity.y <= 0)
            {
                move_Fall();
            }

            else
            {
                _velocity = new Vector2(_velocity.x, _velocity.y - jump_DecSpeed);
            }
        }
        else if (isFall)
        {
            if (isGround)
            {
                Ground();
            }
            else if (isPusy)
            {
                if (isSlipBlock)
                {
                    float velocity_y = _velocity.y - jump_DecSpeed;
                    _velocity = new Vector2(_velocity.x, velocity_y > -16 ? velocity_y : -16);
                }
                else { slip_Start(); }

            }
            else
            {
                float velocity_y = _velocity.y - jump_DecSpeed;
                _velocity = new Vector2(_velocity.x, velocity_y > -16 ? velocity_y : -16);
            }
        }
        else if (isSlip)
        {
            if (isPusy == false)
            {
                stop_Slip();
                move_Fall();
            }
            else if (isGround)
            {
                stop_Slip();
                move_Fall();
            }
            else if (_velocity.y == 0f)
            {

            }
        }
        else if (isPusy)
        {
            if (isGround)
            {

            }
            else
            { slip_Move(); }
        }
        else if (isDash)
        {
            if(isGround == false)
            {
                stop_Dash(false);
                move_Fall();
            }
            else if(!IsKeyPressd("Dash"))
            {
               
                DashEnd(true);
            }
        }
        else if (isCrouch)
        {
            if (!IsDownKeyPressd())
            {
               
             
                    stop_Crouch();
                
            }
            else if (IsLeftKeyPressd() && isSeeRight)
            {
                Flip();
            }
            else if (IsRightKeyPressd() && !isSeeRight)
            { Flip(); }
            else { move_Stop(); }
          
        }
        else
        {
            if (isGround == false)
            {
                move_Fall();
            }
            UnBlockingSlip();
        }
        if (IsKeyPressd("Attack"))
        {
            if (charge_Time > 0)
            {
                Charge();

            }
            else
            {
                _isShotPressed = true;
            }
            charge_Time = (charge_Time >= 3f) ? 3 : charge_Time += Time.deltaTime;
        }
        else if (_isShotPressed == true)
        {
            float chargeTime = charge_Time;

            //
            Shot();
            if (isCheckPlayingAnimation("Idle")
                || isCheckPlayingAnimation("Fall_end")
                || isCheckPlayingAnimation("Shot")
                )
            {
                _Anim.Play("Shot" , 0 , 0); Debug.Log("ㅑㅅ");
            }
        }
    }

    void LateUpdate() // 
    {
        UpdateState();
    }

    void  OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Hp_small"))
        {
            Health += 5;
            Destroy(col.gameObject);
        }
        if(col.CompareTag("Hp_medium"))
        {
            Health += 10;
            Destroy(col.gameObject);
        }
        if(col.CompareTag("Hp_Large"))
        {
            Health += 15;
            Destroy(col.gameObject);
        }
        if(col.CompareTag("LifeUp"))
        {
            Health = 40;
            Destroy(col.gameObject);
        }
    
    }
     void OnTriggerStay2D(Collider2D col)
    {

    }
     void OnTriggerExit2D(Collider2D col)
    {
       
    }
     void OnCollisionEnter2D(Collision2D col)
    {
        UpdatePhysicsState(col); 
    }
     void OnCollisionStay2D(Collision2D col)
    {
        UpdatePhysicsState(col); 

    }
     void OnCollisionExit2D(Collision2D col)
    {

        UpdatePhysicsState(col); 
    }


    #endregion

    #region 플레이어가 움직이기 위한 매서드

    void Ground()
    {
        stop_Jump();
        stop_Fall();
        stop_Dash(false);
        stop_Slip();
        stop_DashJump();

       // isReady = false;

        HitBoxUpdate();

    }

    /// <summary>
    /// 일반적인 움직임에 관한 매서드
    /// </summary>
    ///
    void move_Right()
    {

        _velocity = new Vector2(move_Speed, _velocity.y);
        isMoving = true;
        if (!isSeeRight)
            Flip();
    }
    void move_Left()
    {

        _velocity = new Vector2(-move_Speed, _velocity.y);
        isMoving = true;
        if (isSeeRight)
            Flip();
    }
    void move_Stop()
    {
       // _velocity = new Vector2(_velocity.x, _velocity.y);
        _velocity = new Vector2(0, _velocity.y);
        isMoving = false;
    }

    /// <summary>
    /// 점프
    /// </summary>
    void move_Jump()
    {
        //상테처리
        BlockingDash();
        stop_Crouch();
        BlockingSlip();
        isJumping = true;
        //운동처리
        _velocity = new Vector2(_velocity.x, jump_Speed);
        Invoke("UnBlockingSlip", 0.13f);
    }
    void stop_Jump()
    {
        // 상태 처리
        UnBlockingMove();
        UnBlockingDash();
        isJumping = false;

    }

    /// <summary>
    /// 추락
    /// </summary>
    void move_Fall()
    {
        BlockingDash();
        BlockingJump();
        stop_Crouch();
        if (_velocity.y > 0)
        {
            _velocity = new Vector2(_velocity.x, 0);
        }

        // 운동상태 갱신
        isJumping = false;
        isFall = true;
    }
    void stop_Fall()
    {
        UnBlockingDash();
        UnBlockingJump();

        
        isFall = false;
    }





    // 벽에서 미끄러지는것에대한처리
    void slip_Start()
    {
        //상태갱신
       isSlip = true;
        move_Stop();
        stop_Jump();
        stop_Fall();
        stop_Dash(false);
        BlockingDash();
        UnBlockingJump(); // 벽점프때문에 뺏음

        //운ㄷㅇ처리
        _velocity = new Vector2(0, 0);
        //
        Invoke("slip_Move", 0.13f);
    }
    void slip_Move()
    {
        if (isSlip)
        {
        
            GameObject slipFog = CreateEffect(effects[2], SlipFogPos);
            slipFog.transform.SetParent(isGround_Center.transform);
            if (!isSeeRight)
            {
                var Scale = slipFog.transform.localScale;
                Scale.x = isSeeRight ? Scale.x : -Scale.x;
                slipFog.transform.localScale = Scale;
            }
            Ef_SlipFog = slipFog;
        }

        _velocity = new Vector2(0, -3f);
    }
    void stop_Slip()
    {
        UnBlockingDash();
        isSlip = false;

        if (Ef_SlipFog != null)
        {
            Ef_SlipFog.transform.SetParent(null);
            Ef_SlipFog.GetComponent<EffectScript>().EndEffect();
            Ef_SlipFog = null;
        }



        _velocity = new Vector2(_velocity.x, 0);
    }
    void move_WallJump()
    {
        // 입력 방지
        stop_Jump();
        stop_Fall();
        stop_Slip();
        BlockingDash();
        BlockingJump();
        BlockingSlip();
        BlockingMove();
    //    InputBlock();
        // UnBlockingMove();
        // 운동 처리
        _velocity = new Vector2(isSeeRight ? -1.5f * move_Speed : 1.5f * move_Speed, jump_Speed);


        isJumping = true;
        isWallJump = true;
        isPusy = false;
        // 이펙트 처리
        CreateEffect(effects[3], SlipFogPos);

        // 운동 중단 매서드 호출
        Invoke("stop_WallJump", 0.138888f);

    }
    void stop_WallJump()
    {
        //상태
    //    InputUnBlock();
        UnBlockingSlip();
        UnBlockingMove();

        //운동
        _velocity = new Vector2(0, _velocity.y);

        isWallJump = false;
    
    }

    /// <summary>
    /// 대쉬에 관한 매서드
    /// </summary>
    void move_Dash()
    {
        BlockingMove(); 
        BlockingDash();
        move_Stop();
        move_Speed = dash_Speed;
        isMoving = false;
        isDash = true;
        _velocity = new Vector2(isSeeRight ? dash_Speed: -dash_Speed, _velocity.y);
        //_Rgbd.AddForce(Vector2.right * 100);
        HitBoxUpdate();
    }

    void stop_Dash(bool userCancel)
    {

        UnBlockingMove();
        UnBlockingDash();
        stop_Crouch(); 
        isDash = false;

        float brake_Speed = move_Speed;
        move_Speed = walk_Speed;
        if (userCancel == true)
        {
         
            _velocity = new Vector2(0, _velocity.y);// 대쉬점프중엔 제자리 유지
        //    Debug.Log("대쉬캔슬");
           
            move_Stop();
            stop_Crouch();
        }
        else if(!userCancel)
        {
       
            if (brake_Speed != dash_Speed)
            {
                _velocity = new Vector2(isSeeRight ? brake_Speed : -brake_Speed, _velocity.y);
            }
            else { _velocity = new Vector2(isSeeRight ? walk_Speed : -walk_Speed, _velocity.y); }
        }
        if (Ef_dashBoost != null)
        {
            Ef_dashBoost.GetComponent<EffectScript>().DestroyEffect();
            Ef_dashBoost = null;
        }



        HitBoxUpdate();
    }
    void move_DashJump()
    {
        //상태 갱신
        BlockingDash();
        BlockingSlip();
        stop_Jump();
        stop_Fall();
        stop_Slip();


      
        float dashJump_Direction = 0;
        if (IsLeftKeyPressd() || IsRightKeyPressd())
        {
            dashJump_Direction = isSeeRight ? dash_Speed : -dash_Speed;
        }
        _velocity = new Vector2(dashJump_Direction, jump_Speed);

        isJumping = true;
        isDash = true;
        isDashJump = true;
        Invoke("UnBlockingSlip", 0.13f);
        HitBoxUpdate();

    }
    void stop_DashJump()
    {
    
        isJumping = false;
        isDash = false;
        isDashJump = false;
        HitBoxUpdate();
    }

 

    void DashStart()
    {


        move_Dash(); 
        GameObject dashfog = CreateEffect(effects[0], DashFogPos);
        if (!isSeeRight)
        {
            var newScale = dashfog.transform.localScale;
            newScale.x = isSeeRight ? newScale.x : -newScale.x;
            dashfog.transform.localScale = newScale;
        }// 플레이어가 왼쪽 보고있으면 대쉬 연기의 방향을 바꾼다.
        //대쉬부스터를불러옵니다.
       Co_dashCoroutine = StartCoroutine(dashCoroutine());

    }
   
    void DashEnd(bool userCancel)
    {

        stop_Dash(userCancel);

        if (Co_dashCoroutine != null)
        {
            StopCoroutine(Co_dashCoroutine);
        }
        Co_dashCoroutine = StartCoroutine(dashEndCoroutine());
    }


    //앉기
    void move_Crouch()
    {
        move_Stop();
        isCrouch = true;
        HitBoxUpdate();
    }
    void stop_Crouch()
    {
        isCrouch = false; 
        HitBoxUpdate();
    }




    #endregion

    #region 보조매서드
    public Vector2 _Velocity; // 플레이어 감시용
    public virtual void Hurt(int damage)
    {
        //연산
        Health -= damage;
     

        if(!isLife())
        {
            isDead = true;
            Destroy(gameObject);
            DeadParticle.PleasePlayParticle(this);
            
        }
        else if (p_Health <= p_dangerHealth)
        {
            isDanger = true;
        }
        // 상태
        isDamaged = true;
        Is_Invincible = true;
        InputBlock();
        stop_Dash(false);
        stop_Jump();
        stop_Fall();
        // 넉백
        if(isLife())
        {
            Debug.Log("넉백");
            float force = 100;
            Vector2 direction = (isSeeRight ? Vector2.left : Vector2.right);
            _velocity = Vector2.zero;
            //Vector2 attackvel = new Vector2(-20f, 70f);
          //  _Rgbd.AddForce(attackvel, ForceMode2D.Impulse);
            _Rgbd.AddForce(force * direction);
           
        }
        Invoke("EndHurt", 0.36f);
    }
    void EndHurt()
    {
        isDamaged = false;
        InputUnBlock();
        StartCoroutine(invencibleCoroutine());
    }
    void Flip()
    {
        isSeeRight = !isSeeRight;
        Vector2 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
    public bool isCheckPlayingAnimation(string animName) // 애니매이션이 현재 실행중인지 체크합니다.
    {
        return _Anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    void HitBoxChange(bool dash) // 대쉬 , 앉기에따른 히트박스의 변형을 위한 함수
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        BoxCollider2D hitBox = dash ? DashBox : DefaultBox;
        boxCollider.offset = hitBox.offset;
        boxCollider.size = hitBox.size;
    }

    void HitBoxUpdate()
    {

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        BoxCollider2D hitBox = DefaultBox;

        if (IsCrouchState())
        {
            isCrouch = true;
            hitBox = DashBox;
        }
        boxCollider.offset = hitBox.offset;
        boxCollider.size = hitBox.size;
    }

    public bool IsCrouchState() // 앉았나?
    {
        if (isDash || isCrouch)
        {
            if(!isDashJump)
            return true;
        }
        return false;
    }

    public static GameObject CreateEffect(GameObject gameObject, Transform transform) // 복제한 오브젝트를 게임 오브젝트로 변환
    {
        return Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
    }

    void BlockingDash()
    {



        isDashBlock = true;
    }
    void UnBlockingDash()
    {

        isDashBlock = false;
    }
    void BlockingMove()
    {

        isMoveBlock = true;
    }
    void UnBlockingMove()
    {

        isMoveBlock = false;
    }
    void BlockingJump()
    {
        isJumpBlock = true;
    }
    void UnBlockingJump()
    {
        isJumpBlock = false;
    }
    void BlockingSlip()
    { isSlipBlock = true; }
    void UnBlockingSlip()
    {
        isSlipBlock = false;
    }
    Transform get_ShotPos()
    {
        Transform pos = shot_Pos;
        if (isJumping)
        {
            pos = shotPos_Jump;
        }
        //else if(isSlip)
        //{
        //    pos = shotPos_Slip;
        //}
        else if (isSlip)
        {
            pos = shotPos_Slip;
        }
        else if (isMoving)
        {
            pos = shotPos_Walk;
        }
        else if (isDash)
        {
            pos = shotPos_Dash;
        }
        else if (isCrouch)
        {
            pos = shotPos_Dash;
        }
        return pos;
    }

    void Ready()
    {
        isReady = true;
    }
    void EndReady()
    {
        if(cam != null)
        {
            isReady = false;
            cam.PlayReadyAnim();
        }
      
       
    }
    public void playWarningAnim()
    {
        cam.PlayWarningAnim();
    }

    #endregion

    #region 입력관리지역
    bool IsKeyDown(string buttonName)
    {
        if (inputBlocked)
            return false;
        //
        return Input.GetButtonDown(buttonName);

    }
    bool IsKeyPressd(string buttonName)
    {
        if (inputBlocked)
            return false;
        //
        return Input.GetButton(buttonName);
    }
    bool IsLeftKeyPressd()
    {
        if (inputBlocked)
            return false;
        //
        return Input.GetAxis("Horizontal") == -1;
    }
    bool IsRightKeyPressd()
    {
        if (inputBlocked)
            return false;
        //
        return Input.GetAxis("Horizontal") == 1;
    }
    bool IsUpKeyPressd()
    {
        if (inputBlocked)
            return false;
        //
        return Input.GetAxis("Vertical") == 1;
    }
    bool IsDownKeyPressd()
    {
        if (inputBlocked)
            return false;
        //
        return Input.GetAxis("Vertical") == -1;
    }

    #endregion
    #region 다른 스크립트에서 요청하는 매서드들
    // 다른 스크립트의 요청에 의한 움직임
    bool Requstmove = false;
    bool move_Requst
    {
        get { return Requstmove; }
        set { Requstmove = value; }
    }
    public void RequstMove_Right()
    {
        move_Requst = true;
        move_Right();

    }
    public void RequstMove_Left()
    {
        move_Requst = true;
        move_Left();
 
    }
    public void RequstMove_Stop()
    {
        move_Requst = false;
    }
    public void RequstChangeSpeed(float speed)
    {
        move_Speed = speed;
    }
    public void InputBlock()
    {
        inputBlocked = true;
    }
    public void InputUnBlock()
    {
        inputBlocked = false;
    }
    public void RequstFlip()
    {
        Flip();
    }
    //
    #endregion
    #region 공격 관련 매서드
    void Shot()
    {
        IsShot = true;

        int busterIndex = -1;
        if (charge_Time < CHARGE_TIME[1])
        {
            busterIndex = 0;
        }
        else if (charge_Time < CHARGE_TIME[2])
        {
            busterIndex = 1;
        }
        else
        {
            busterIndex = 2;
        }

        if(Ef_Charge_Lv1 != null)
        {
            if(Ef_Charge_Lv2 != null)
            {
                Ef_Charge_Lv2.GetComponent<EffectScript>().DestroyEffect();
                Ef_Charge_Lv2 = null;
            }
            Ef_Charge_Lv1.GetComponent<EffectScript>().DestroyEffect();
            Ef_Charge_Lv1 = null;
        }

        ShotTime = 0;
        charge_Time = 0;
        _isShotPressed = false;
        StartCoroutine(CreateBusterCouroutine(busterIndex));
        Invoke("EndShot", SHOT_ENDTIME);

    }
    void Charge()
    {
        if (charge_Time < CHARGE_TIME[0])
        {
            
        }
        else if (charge_Time >= CHARGE_TIME[1] && Ef_Charge_Lv1 == null)
        {
            Ef_Charge_Lv1 = CreateEffect(effects[5], ChargeEffectPos);
            Ef_Charge_Lv1.transform.SetParent(ChargeEffectPos);
           
        }
        else if (charge_Time >= CHARGE_TIME[2] && Ef_Charge_Lv2 == null)
        {
            Ef_Charge_Lv2 = CreateEffect(effects[6], ChargeEffectPos);
            Ef_Charge_Lv2.transform.SetParent(ChargeEffectPos);
        }
    }
    void EndShot() // 샷상태를 풀어줘야함.
    {
        if (ShotTime >= SHOT_ENDTIME)
        {
            IsShot = false;
        }
    }
    #endregion

    #region 코루틴
    Coroutine Co_dashCoroutine;

    IEnumerator CreateBusterCouroutine(int index)
    {
        Transform shotPos = get_ShotPos();
        bool toLeft = (isSlip ? isSeeRight : !isSeeRight); // 슬라이딩 중이면 왼쪽 아니면 오른쪽

        GameObject shotEffect = CreateEffect(effects[7 + index], shotPos);
        if (index == 2)
        {
            if (isCheckPlayingAnimation("Idle"))
            {
                shotEffect.transform.position = transform.position;
            }
        }
        // 발사할때 해당부위에 이펙트 붙이기
        Vector3 effectScale = shotEffect.transform.localScale;
        effectScale.x *= (toLeft ? -1 : 1);
        shotEffect.transform.localScale = effectScale;
        shotEffect.transform.parent = shotPos.transform;
    
        // 버스터 스크립트에 대한 처리
        GameObject Buster = CreateEffect(Busters[index], shotPos);
        // 풀차지라면 조금대기후 나타나기
        if (index == 2)
        {
            Buster.GetComponent<SpriteRenderer>().color = Color.clear;
            yield return new WaitForSeconds(0.15f);

        }
        if (Buster != null)
        {
            Buster.GetComponent<SpriteRenderer>().color = Color.white;
            Buster.transform.position = shotPos.position;

            // 날아가는처리
            Vector3 busterScale = Buster.transform.localScale;
            busterScale.x *= (toLeft ? -1 : 1);
            Buster.transform.localScale = busterScale;
            Buster.GetComponent<Rigidbody2D>().velocity = (toLeft ? Vector2.left : Vector2.right) * buster_Speed;
            //탄환이 메인카메라 밖으로 나갈시 삭제합니다.
            BusterScript busterScript = Buster.GetComponent<BusterScript>();
            busterScript.MainCamera = Camera.main;

        }
        yield break;
    }

    IEnumerator dashCoroutine()
    {
        // dash_beg 애니메이션

        yield return new WaitForEndOfFrame();
        if (isCheckPlayingAnimation("Dash_beg"))
        {
            yield return new WaitForSeconds(DASH_BEG_TIME);
        }
        if (isDashJump == false)
        {
            GameObject dashBoost = CreateEffect(effects[1], DashBoostPos);
            dashBoost.transform.SetParent(DashBoostStay.transform);
            if (isSeeRight == false)
            {
                var newScale = dashBoost.transform.localScale;
                newScale.x = isSeeRight ? newScale.x : -newScale.x;
                dashBoost.transform.localScale = newScale;
            }
            Ef_dashBoost = dashBoost;

            yield return new WaitForSeconds(DASH_RUN_TIME);
        }


      //  stop_Dash();
      if(isDashJump == false && isGround)
        {
            DashEnd(false);
        }
        yield break; // 코루틴 중지
    }

   


    IEnumerator dashEndCoroutine() // 플레이어가 대쉬에서 손을 떼면 즉시 멈추게 할것임
    {
        BlockingMove(); // 대쉬가 멈추는 애니매이션동안 못움직인다ㄴ
        yield return new WaitForSeconds(DASH_END_TIME);

        UnBlockingMove();
        Co_dashCoroutine = null;
        yield break;
    }

    IEnumerator invencibleCoroutine()
    {
        InvencibleTime = 0;
        while(invencibleTime < 10)
        {
            if (InvencibleTime % 2 == 0)
            {
                _Renderer.color = new Color32(255, 255, 255, 90);
            }
            else { _Renderer.color = new Color32(255 , 255 , 255 , 255);}

            yield return new WaitForSeconds(0.1f);
            invencibleTime++;
        }
        _Renderer.color = new Color32(255, 255, 255, 255);
        Is_Invincible = false;
        yield return null;
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
        if(CheckLayerMask(layer , touchTheGround))
        {
            EdgeCollider2D groundCollider = col.collider as EdgeCollider2D; // 콜라이더를 edgecolider로 변환합니다.
     
            //땅충돌
            if(isTouchGround(groundCollider))
            {
                land_Edgeset.Add(groundCollider); 
            }
            else
            {
                land_Edgeset.Remove(groundCollider);
            }
        }
        if (CheckLayerMask(layer , touchTheWall))
        {
            bool isTouchingWall = isTouchWall(col);
            bool isKey_Valid = isSeeRight ? IsRightKeyPressd() : IsLeftKeyPressd();
            isPusy = isTouchingWall && isKey_Valid;
        }
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

        if(isOnGround()) // 각 상태마다 처리합니다.
        {
            if (ray_Back.normal.normalized != ray_Front.normal.normalized) // 앞레이 뒷레이 경사각이 다를때
            {
                bool isHitBackSlope = ray_Back.normal.x == 0; // 후방레이가 충돌한표면의 x노말값이 0이면 true
                RaycastHit2D ray = isHitBackSlope ? ray_Back : ray_Front; // 사실이면 back 거짓이면 front
                Vector2 direction = isSeeRight ? Vector2.right : Vector2.left;
                float rayAngle = Vector2.Angle(direction, ray.normal);
                float rayRadian = Mathf.Deg2Rad * rayAngle; // 구한 레이의 각도를 라디안값으로 변형합니다.

                float sx = move_Speed * Mathf.Cos(rayRadian); // x축미끄러짐
                float sy = move_Speed * Mathf.Sin(rayRadian); // y축 미끄러짐
                float x_direction = isSeeRight ? sx : -sx;
                if (isReady)
                {
                    _velocity = Vector2.zero;
                }
                else if (isJumping) // 점프중
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
                if(isReady) // 준비중
                {
                    _velocity = Vector2.zero;
                }
            
            }
            isGround = true;
        }
      else if (isJumping || isFall)
        {
            isGround = false;
        }
     else if(ray_Back || ray_Front)
        {
            if (isSlip)
            {

            }
            else if (isReady)
            {
                _velocity = new Vector2(0, 0);
            }
            else if (isGround) // 땅에 닿아있을때
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
        if (_Collider.IsTouchingLayers(touchTheGround)) // 콜라이더가 그라운드를 건드렸을때
        {
            float playerfoot = _Collider.bounds.min.y; // 플레이어의 발은 콜라이더y점의 최소값입니다. 
            foreach (EdgeCollider2D edge in land_Edgeset)
            {
                float ground_Bottom = edge.bounds.min.y; // 땅의 최소좌표
                float ground_Top = edge.bounds.max.y; // 땅의 최대좌표

                if (ground_Bottom == ground_Top)// 평면
                {
                    if (playerfoot >= ground_Top) // 플레이어의 발이 안닿아있다면 참 반환 / 씬에 그려줄것
                    {
                        Debug.DrawLine(edge.points[0], edge.points[1] , Color.red); 
                        return true;
                    }
                }
                else// 경사면
                {
                    if ((ground_Bottom <= playerfoot && playerfoot <= ground_Top))
                    {
                        Debug.DrawLine(edge.points[0], edge.points[1] , Color.blue);
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
        
        if (_Collider.IsTouching(GroundCol))
        {
        
            Bounds groundBounds = GroundCol.bounds;
            if (groundBounds.min.y == groundBounds.max.y)
            {
                float playerBottom = _Collider.bounds.min.y;
                float groundTop = groundBounds.max.y;
                if (playerBottom >= groundTop) // 플레이어의 발이 땅에닿아있다
                {
                    return true;
                }
                else { return false; } // 아니다
            }
            else
            {
                float playerBottom = _Collider.bounds.min.y;
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
    bool isTouchWall(Collision2D collision) // 벽충돌 계산
    {
        if(_Collider.IsTouchingLayers(touchTheWall))
        {
            if (pushCheckBox.IsTouchingLayers(touchTheWall))
            {
               
                return true; 
            }
            else { return false;  }

        }
        //안닿아있으면 거짓입니다.
        return false; ;
    }
}
     #endregion 다른게임에서도 사용가능하다
