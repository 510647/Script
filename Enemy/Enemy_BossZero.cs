using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossZero : Enemy_BossScript
{
    // 보스 제로에 대한 스크립트입니다.
    // Use this for initialization
    #region 플레이어블 캐릭터인 만큼 플레이어콘트롤러에서 따오겠음.
    public float move_Speed = 5f;
    //public bool _isGround = false;

    // 공용사용 가능 객체
    public Transform DashBoostPos;
    public Transform DashFogPos;
    public Transform DashBoostStay;
    public Transform isGround_Center;
    public Transform isGround_Front;
    public Transform isGround_Back;
    public Transform ChargeEffectPos;

    public BoxCollider2D DefaultBox;
    public BoxCollider2D DashBox;
    public BoxCollider2D pushCheckBox; //밀고있나확인하는박스

    GameObject Ef_dashBoost = null;
    GameObject Ef_dashFog = null;

    public LayerMask touchTheGround;
    public LayerMask touchTheWall;
    HashSet<EdgeCollider2D> land_Edgeset = new HashSet<EdgeCollider2D>();
    HashSet<BoxCollider2D> land_Boxset = new HashSet<BoxCollider2D>();
    #endregion

    #region 각종 상태값
    // bool _isSeeRight = true; // 플레이어가 오른쪽을 보고 있습니까?
    bool _isMoving = true;
    public bool _isJumping = false;
    public bool _isGround = false;
    public bool _isIdle = false;
    public bool _isFall = false;
    public bool _isDash = false;
    bool _isReady = false; // 준비중?
    bool _isLife = true;
    bool _isDaed = false;
    bool _isDamaged = false;
    bool _isInvincible = false;
    #endregion

    #region
    //   public bool isSeeRight { get { return _isSeeRight; } private set { _isSeeRight = value; } } // 왼쪽 :? 오른쪽?
    public bool isMoving { get { return _isMoving; } set { _Anim.SetBool("isMoving", _isMoving = value); } } // 움직이고있어?
    public bool isJumping { get { return _isJumping; } set { _Anim.SetBool("isJumping", _isJumping = value); } } // 점프하고있어?
    public bool isGround { get { return _isGround; } set { _Anim.SetBool("isGround", _isGround = value); } } // 땅에닿았어 ? 
                                                                                                             //  public bool isDanger { get { return _isDanger; } set { _Anim.SetBool("isDanger", _isDanger = value); if (_isDanger) { _Anim.SetFloat("DangerState", 1); } else { _Anim.SetFloat("DangerState", 0); } } } // 위험해?
    public bool isFall { get { return _isFall; } set { _Anim.SetBool("isFall", _isFall = value); } }
    public bool isDash { get { return _isDash; } set { _Anim.SetBool("isDash", _isDash = value); } }
    //  public bool isWallJump { get { return _isWallJump; } set { _Anim.SetBool("isWallJump", _isWallJump = value); } }
    //  public bool isSlip { get { return _isSlip; } set { _Anim.SetBool("isSlip", _isSlip = value); } }
    //  public bool isCrouch { get { return _isCrouch; } set { _Anim.SetBool("isCrouch", _isCrouch = value); } }
    //  public bool isPusy { get { return _isPusy; } set { _Anim.SetBool("isPusy", _isPusy = value); } }
    //  public bool isLife() { return 0 < p_Health; }
    //  public bool isHealthFull() { return p_Health == p_maxHealth; }
    public bool isDead { get { return _isDaed; } private set { _isDaed = value; } }
    public bool isReady { get; private set; }
    public bool isDamaged { get { return _isDamaged; } set { _Anim.SetBool("isDamaged", _isDamaged = value); } }
    bool isDashBlock { get; set; } // 대쉬가 막혀있는가?
                                   //  bool isMoveBlock { get { return _isMoveBlock; } set { _isMoveBlock = value; } } // 이동이 막혀있는가?
    bool isJumpBlock { get; set; }
    bool isSlipBlock { get; set; }
    //  bool IsShot { get { return _isShot; } set { _Anim.SetBool("IsShot", _isShot = value); } }
    //  bool IsShotBlock { get { return _isShotBlock; } set { _isShotBlock = value; } }
    // bool IsChargeBuster { get { return _isChargeBuster; } set { _isChargeBuster = value; } }
    bool isDashJump { get; set; }
    public bool Is_Invincible { get; set; }
    // float ShotTime { get { return shot_Time; } set { _Anim.SetFloat("ShotTime", shot_Time = value); } }
    // float InvencibleTime { get { return invencibleTime; } set { invencibleTime = value; } }
    #endregion

    #region 객체 정의
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

    public Vector2 _velocity
    {
        get { return _Rgbd.velocity; }
        set { _Rgbd.velocity = value; }
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
        if (!isGround)
        {

        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    #endregion
}

