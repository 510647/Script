using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadEffect : EffectScript {
     Enemy_NomalBoss Boss;
	// Use this for initialization
	void Start () {
        Boss = FindObjectOfType<Enemy_NomalBoss>();
        StartCoroutine(ExplosionCoroutine());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateExplosionEff(Vector3 position)
    {
        GameObject effect = Instantiate(Boss.explosion, position, transform.rotation);
    }
    IEnumerator ExplosionCoroutine()
    {
        for (int i = 0; i < 100; i++)
        {
            Debug.Log("다시탄다");
            float RandomPos_X = Random.Range(-1, 1);
            float RandomPos_Y = Random.Range(-1, 1);

            Vector3 explosionPos = transform.position + new Vector3(RandomPos_X, RandomPos_Y);
            CreateExplosionEff(explosionPos);


             float IntervalTime_Explosion = Random.Range(0.3f, 0.4f);
            yield return new WaitForSeconds(IntervalTime_Explosion);
        }


        //  Co_Dead = null;
        //Destroy(gameObject);
       

        yield break;
    }
}
