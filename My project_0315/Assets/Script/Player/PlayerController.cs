using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6;
    public GameObject PlayerPivot;          // 플레이어가 바라보는 방향을 알기위해
    public Camera viewCamera;               // 메인 카메라 받아오는 Camera 오브젝트
    public Vector3 velocity;                // 이동 속도 값
    public ProjectileController projectileController;

    void Start()
    {
        viewCamera = Camera.main;           // 스크립트가 시작될때 카메라를 받아온다
    }

    // Update is called once per frame
    void Update()
    {
        // 화면에서 -> 게임 3D 공간 좌표를 뽑아낸다.
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        
        // 공간좌표가 캐릭터보다 위에 있을경우에는 위를 쳐다보기 때문에 같은 y축 값을 맞춰준다.
        Vector3 targetPosition = new Vector3(mousePos.x, transform.position.y, mousePos.z);

        // 피봇이 해당 타겟을 보게한다.
        PlayerPivot.transform.LookAt(targetPosition, Vector3.up);

        // 방향키를 통해서 이동 벡터값을 생성한다.
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * moveSpeed;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 10.0f, ForceMode.Impulse);
        }

        if(Input.GetMouseButtonDown(0))
        {
            projectileController.FireProjectile();
        }
    }

    private void FixedUpdate()
    {
        // 이동 벡터값을 Rigidbody 물리값에 적용하여 캐릭터를 이동 시킨다.
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + velocity * Time.fixedDeltaTime);
    }
}
