using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class ItemManager : MonoBehaviour {


    public enum ItemNumber : int // 아이템 번호를 정의합니다
    {
        LifeEnegySmall,
        LifeEnergyBig,
        LifeTank,
        LifeUp

    }

    #region 유니티 객체
    SpriteRenderer spRender;
    Rigidbody2D rigi;
    #endregion
    #region 접근가능필드
    public string ItemType;
    public int AddHP; // 체력 몇더해줄꺼양
    public int drop_Probability = 50; // 드랍확률
    public bool isDrop; // 떨어진아이템인지 원래배치한아이템인지 체크하기위해서
    public bool isGround;
    public LayerMask is_touch_Ground;
    public float flashTime = 3f;
    public float dropTime = 5f; // 땅에 떨어진후 머무를 수 있느 ㄴ시간
    #endregion
    #region 일반 필드
    RaycastHit2D ground;
    #endregion
    #region 프로퍼티 정의
    public string itemType
    {
        get { return ItemType; }
    }
    public int addHealth
    {
        get { return AddHP; }
        set { AddHP = value; }
    }
    public bool IsDrop
    {

        get { return isDrop; }

        set {  isDrop = value; }
    }
    //public int Probability
    //{
    //    get {return drop_Probability; }
    //    set { drop_Probability = Mathf.Clamp(value,0, 100); }
    //}
    #endregion
    #region MonoBehaviour 기본 매서드
    // Use this for initialization
    void Start () {
        spRender = GetComponent<SpriteRenderer>();
        rigi = GetComponent<Rigidbody2D>();

        rigi.velocity = new Vector2(0, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		if(!ground)
        {
            ground = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, is_touch_Ground);
            rigi.velocity -= new Vector2(0, 20f) * Time.deltaTime;
        }
        else if (!isGround)
        {
            if (transform.position.y - ground.point.y < 2f)
            {
               // transform.position = ground.point + new Vector2(0, 2f);
                rigi.velocity = Vector2.zero;
                isGround = true;
            }
            else { rigi.velocity -= new Vector2(0, 20f) * Time.deltaTime; }
        }
      
        if(IsDrop)
        {
            dropTime -= Time.deltaTime;
            if(dropTime < 0f)
            {
                Destroy(gameObject);
                return;
            }
            else if (dropTime < flashTime)
            {
                spRender.color = (spRender.color == Color.white) ? Color.clear : Color.white;
            }
        }
	}
  
    #endregion
}
