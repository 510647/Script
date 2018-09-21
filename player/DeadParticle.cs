using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadParticle : MonoBehaviour {


    #region 상수정의
    public float particle_1_Speed;
    public float particle_2_Speed;
    public float particle_IntervalTime = 0.2f;
    #endregion
    #region 공용필드
    public GameObject[] particles;
    #endregion
    #region  필드 정의
    GameObject particle1 { get { return particles[0]; } }
    GameObject particle2 { get { return particles[1]; } }
    #endregion

    #region 매서드그룹
    GameObject CreateParticle(GameObject num, Vector2 direction , float speed)
    {
        GameObject particle = Instantiate(num, transform.position, transform.rotation) as GameObject;
        particle.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
        Destroy(gameObject);
       
        return particle;
    }

    IEnumerator CreateDeadEffect()
    {
        float speed = particle_1_Speed;
        CreateParticle(particle1, new Vector2(+1f, 0f), speed);
        CreateParticle(particle1, new Vector2(0f, +1f), speed);
        CreateParticle(particle1, new Vector2(-1f, 1f), speed);
        CreateParticle(particle1, new Vector2(0f, 0f), speed);
        CreateParticle(particle1, new Vector2(-1f, -1f), speed);
        CreateParticle(particle1, new Vector2(+1f, +1f), speed);
        CreateParticle(particle1, new Vector2(+1f, 0f), speed);
        CreateParticle(particle1, new Vector2(0f, -1f), speed);
        CreateParticle(particle1, new Vector2(+1f, +1f), speed);
        CreateParticle(particle1, new Vector2(+1f, -1f), speed);
        CreateParticle(particle1, new Vector2(-1f, 0f), speed);
        speed = particle_2_Speed;
        for (int i = 0; i < 4; i++)
        {
            Vector2 randomVector = Random.insideUnitCircle;
            Vector2 randomVector2 = Random.insideUnitCircle;
            Vector2 randomVector3 = Random.insideUnitCircle;
            Vector2 randomVector4 = Random.insideUnitCircle;
            Vector2 randomVector5 = Random.insideUnitCircle;
            Vector2 randomVector6 = Random.insideUnitCircle;
            Vector2 randomVector7 = Random.insideUnitCircle;
            Vector2 randomVector8 = Random.insideUnitCircle;
            Vector2 randomVector9 = Random.insideUnitCircle;
            Vector2 randomVector10 = Random.insideUnitCircle;
            Vector2 randomVector11 = Random.insideUnitCircle;
            Vector2 randomVector12 = Random.insideUnitCircle;
            Vector2 randomVector13 = Random.insideUnitCircle;

            //randomVector.Normalize();
            //randomVector2.Normalize();
            //randomVector3.Normalize();
            //randomVector4.Normalize();

            CreateParticle(particle2, randomVector, speed);
            CreateParticle(particle2, randomVector2, speed);
            CreateParticle(particle2, randomVector3, speed);
            CreateParticle(particle2, randomVector4, speed);
            CreateParticle(particle2, randomVector5, speed);
            CreateParticle(particle2, randomVector6, speed);
            CreateParticle(particle2, randomVector7, speed);
            CreateParticle(particle2, randomVector8, speed);
            CreateParticle(particle2, randomVector9, speed);
            CreateParticle(particle2, randomVector10, speed);
            CreateParticle(particle2, randomVector11, speed);
            CreateParticle(particle2, randomVector12, speed);
            CreateParticle(particle2, randomVector13, speed);

           // yield return new WaitForSeconds(1f);
        }
        Debug.Log("1번생성과정 탈출");
    

        yield break;
    }

    public void PleasePlayParticle(playerController player)
    {
        transform.position = player.transform.position;
        StartCoroutine(CreateDeadEffect());

    }
    #endregion



}
