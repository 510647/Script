using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Walker : EnemyScript
{// 걷기만하는 몬스터들이 공통적으로 가지는 스크립트입니다.

    #region 유니티 객체
    Rigidbody2D rigidbody;
    SpriteRenderer spRender;
    Collider2D col;
    #endregion

    #region 접근 가능객체
    public Transform groundChecker;
    public Transform pushChecker;
    public LayerMask whatisGround;
    public LayerMask whatisWall;
    public GameObject mettolClone;
    #endregion

    #region 상태필드
    public float move_Speed;
    #endregion
    
    #region MonoBehaviour 기본매서드
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start () {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
        col = GetComponent <Collider2D>();
        spRender = GetComponent<SpriteRenderer>();
        RaycastHit2D groundRay = Physics2D.Raycast(groundChecker.position, Vector2.down, 10, whatisGround);
        Vector2 newPos = transform.position;
        newPos.y -= Mathf.Abs(col.bounds.min.y - groundRay.point.y);
        transform.position = newPos;
        move_Left();
	}
    public static GameObject mettolConeObject(GameObject gameObject,Transform transform)
    {
      return  Instantiate(gameObject, transform.position, transform.localRotation) as GameObject;
    }
    // Update is called once per frame
    protected override void Update()
    {
       
        base.Update();
        if(Health <= 0)
        {
            Die = true;
        }
        if(Die)
        {
            Dead();
        }
        float x_Pos = transform.position.x;
        float boundLeft = SpawnZone.SpawnZone_Left;
        float boundRight = SpawnZone.SpawnZone_Right;
       
        if (x_Pos < boundLeft)
        {
            move_Right();
         
        }
        else if (x_Pos > boundRight)
        {
            move_Left();
          
        }

       
        //GameObject mettolCone = mettolConeObject(mettolClone, transform);
        //Vector3 scale = mettolCone.transform.localScale;
        //if (isSeeRight) { scale.x *= -1; }
        //mettolCone.transform.localScale = scale;
        //mettolCone.SetActive(false);
        //var imageRenderer = mettolCone.GetComponent<SpriteRenderer>();
        //imageRenderer.sprite = spRender.sprite;
        //mettolCone.SetActive(true); // 활성화
     

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

    #region
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject playerObject = col.gameObject;
            playerController player = playerObject.GetComponent<playerController>();
            Debug.Log("데미지");
            if (player.Is_Invincible)
            {

            }
            else if (!player.Is_Invincible){ player.Hurt(Damage); }
           
            
        }
    }
    #endregion

    #region 이동매서드
    void move_Left()
    {
        if(isSeeRight)
        {
            Debug.Log("플립");
            Flip();
        }
        rigidbody.velocity = new Vector2(-move_Speed, 0);

        //StartCoroutine(Patrol());
    }
    void move_Right()
    {
        if (!isSeeRight)
        {
            Flip();
        }
        rigidbody.velocity = new Vector2(move_Speed, 0);
       // StartCoroutine(Patrol());
    }

    IEnumerator Patrol() // 적이 랜덤으로 이동방향을 바꿉니다.
    {
        while(Health != 0)
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 1)
            {
                move_Left();
            }
            else
            { move_Right(); }
            yield return new WaitForSeconds(3);
        }

    }
    #endregion
    #region
    public override void Dead()
    {
        int itemDrop = Random.Range(0, Items.Length);
        if(Items[itemDrop] != null)
        {
            DropItem(Items[itemDrop]);
            Debug.Log(itemDrop);
        }
        base.Dead();
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
    #endregion
}
