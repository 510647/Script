using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    // 각종 적들의 부모 스크립트입니다.
    #region 각종 필드그룹

    public float en_Speed;
    public int en_Hp;
    public int MaxHealth;
    public int py_damage;
    public bool en_awayInvincible = false; // 무적상태가 지속되는가?
    float en_InvincibleTime = 0f;// 플레이어에게 입히는 데미지
    bool en_Damaged; // 에네미가 데미지를 입었는지 체크합ㄴ디ㅏ.
    bool en_Die;     // 죽었나?
    bool en_Invincible;
    bool en_Life = true; // 살아계신가요?

   public GameObject explosion;
    public ItemManager[] Items;
     Enemy_SpawnZone spawnZone;
    public Enemy_SpawnZone SpawnZone
    {
        get { return spawnZone; }
        set { spawnZone = value; }
    }
    /// <summary>
    /// 뭘 구현해야 하는가?
    /// 데미지 입기
    /// 플레이어 공격하기
    /// 움직이기 (패트롤) //안움직이는 것도 있으니까 따로구현하자
    /// 
    /// </summary>

    #endregion

    #region 상태/프로퍼티 정의
    public int Health // 죽었을때 0아래로 안내려가게합시다
    {
        get { return en_Hp; }
    
      //    get { return p_Health; }
        set { en_Hp = Mathf.Clamp(value, 0, MaxHealth); }
    }
    public int Damage
    { get { return py_damage; } }
    public bool Damaged
    {
        get { return en_Damaged; }
        protected set { en_Damaged = value; }
    }
    public bool Die
    { get { return en_Die; }
        protected set { en_Die = value; }
    }
    public bool Invincible
    { get { return en_Invincible; }
        protected set { en_Invincible = value; }
    }
    public bool Life
    { get { return en_Life; }
        protected set { en_Life = value; }
    }
    protected StageManager stageManager
    {
        get { return StageManager.Instance; }
    }


   public bool _isSeeRight = false;
    #endregion

    #region 유니티 객체
    Collider2D _Collider;
    protected Collider2D en_Collider{ get { return _Collider; } }
    #endregion

    #region MonoBehaviour 기본 매서드
    // Use this for initialization
    protected virtual void Awake()
    {
        _Collider = GetComponent<Collider2D>();
    }
    protected virtual void Start() {
        if(en_awayInvincible)
        {
            Invincible = true;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!Life)
        {
            Dead();
        }
    }
    protected virtual void FixedUpdate()
    {

    }
    protected virtual void LateUpdate()
    {

    }
    #endregion

    #region 매서드
    public virtual void Dead()
    {
        gameObject.SetActive(false);
        EnemyDestroyer();


    }
    public void DropItem()
    {
    }

    public virtual void GetHurt(int damage)
    {
        Health -= damage;
        Damaged = true;
        //무적처리 코루틴 실행하기
    }
    protected ItemManager DropItem(ItemManager item)
    {
        if(Random.Range(0 , 100) < item.drop_Probability)
        {
            ItemManager createItem = (ItemManager)Instantiate(item, transform.position, transform.rotation);
            createItem.IsDrop = true;
            Debug.Log(item.drop_Probability);
            return createItem;
        }
   
        return null;
    }
    #endregion

    #region 보조매서드
    public bool isSeeRight
    {
        get { return _isSeeRight; }
        set { _isSeeRight = value; }
    }

   public void Flip()
    {
        isSeeRight = !isSeeRight;
        Vector2 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
    public void EnemyDestroyer() // 
    {
        Destroy(gameObject);
      
    }
    #endregion
}
