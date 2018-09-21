using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour {

    Animator _Anim;


	void Awake()
	{
		_Anim = GetComponent<Animator> ();
	}
    bool end_Effect;
    public bool EndEffected
    { get { return end_Effect; }set { _Anim.SetBool("EndEfferct", end_Effect = value); } }
     bool ef_Destroy;
    public bool DestroyEfeected
    { get { return ef_Destroy; }private set { ef_Destroy = value; } }
    
    public void DestroyEffect()
    {
        _Anim.enabled = false;
     
        DestroyEfeected = true; // 

    }
	//이펙트 종료
    public void EndEffect()
    {
     
          
            EndEffected = true;
         
        

    }

    void Update()
    {
		if (ef_Destroy == false) 
		{
			return;
		}
		if (_Anim.enabled)
        {
			
		} else 
		{
			Destroy (gameObject);
        }

    }

    /// <summary>
    /// 한번쓰는 이펙트를 제거시킵니ㅏㄷ
    /// </summary>
    void Once_EndEffect()
    {
        _Anim.enabled = false;
        Destroy(gameObject);
    }
	
}



