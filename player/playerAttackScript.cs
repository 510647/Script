using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttackScript : MonoBehaviour {

    #region 공용필드정의
    public GameObject[] effects;

    public int damage;
    #endregion


    #region 필드 정의
    public GameObject HitEffect { get { return effects[0]; } } // 총알이 반사당할때 나타나는 이펙트

    public GameObject ReflectEffect { get { return effects[1]; } } // 총알이 성공적으로 타격되면 나타나는 이펙트
    #endregion

    #region MonoBehaviour 기본 매서드
    // Use this for initialization

  protected  virtual void Awake()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    protected virtual void FixedUpdate()
    {

    }
    #endregion

    #region
    protected GameObject HitParticle(bool isSeeRight , Transform transform)
    {
        GameObject hitParticle = Instantiate(HitEffect, transform.position, transform.rotation) as GameObject;
        if(isSeeRight)
        {
            Vector3 Scale = hitParticle.transform.localScale;
            Scale.x *= -1;
            hitParticle.transform.localScale = Scale;
        }
        return hitParticle;
    }
    protected GameObject ReflectParticle(bool isSeeRight, Transform transform)
    {
        GameObject reflectParticle = Instantiate(ReflectEffect, transform.position, transform.rotation) as GameObject;
        if (isSeeRight)
        {
            Vector3 Scale = reflectParticle.transform.localScale;
            Scale.x *= -1;
            reflectParticle.transform.localScale = Scale;
        }
        return reflectParticle;
    }
    #endregion
}
