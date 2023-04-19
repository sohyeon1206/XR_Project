using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 5.0f;          // 이동 속도
    public float rotationSpeed = 1.0f;  // 포탑 회전 속도
    public GameObject bulletPrefab;     // 총알 프리팹
    public GameObject EnemyPivot;       // 적 포탑 피봇
    public Transform firePoint;         // 발사 위치
    public float fireRate = 1f;         // 발사 속도

    private Rigidbody rb;
    private Transform player;

    private float NextFireTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();             // rb에 지금 RidgidBody 입력
        player = GameObject.FindGameObjectWithTag("Player").transform;      // Player Tag 를 가지고 있는 오브젝트 transform 을 입력
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if(Vector3.Distance(player.position, transform.position) > 5.0f)
            {
                Vector3 direction = (player.position - transform.position).normalized;      // 이동 방향성 (플레이어와 나)
                rb.MovePosition(transform.position + direction * speed * Time.deltaTime);   // 방향성 계산된 것 RigidBody 에 반영
            }

            // 포탑 회전
            Vector3 targetDirection = (player.position - EnemyPivot.transform.position).normalized;     // 포탑의 방향성을 계산
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            EnemyPivot.transform.rotation = Quaternion.Lerp(EnemyPivot.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); //계산된 회전값을 반영

            if(Time.time > NextFireTime)
            {
                NextFireTime = Time.time + 1f / fireRate;       // 시간대비 쏘는 횟수
                GameObject temp = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                temp.transform.localScale = Vector3.one * 0.3f;
                temp.GetComponent<ProjectileMove>().launchDirection = firePoint.localRotation * Vector3.forward;    // 발사점을 보정해주는 라인
                temp.GetComponent<ProjectileMove>().projectileType = ProjectileMove.PROJECTILETYPE.MONSTER;         // 발사체 타입 설정
                Destroy(temp, 10.0f);               // 10초 후에 생성된 발사체 삭제
            }
        }
    }
}
