using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenManager : MonoBehaviour
{
    public GameObject Monster;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);       // 2D 화면에서 유니티의 3D 화면으로 전환하는 함수 ray를 쓸때
            
            RaycastHit hit;

            if(Physics.Raycast(cast, out hit))          // ray 를 쐈을때 물리 결과가 일어나면
            {
                if(hit.collider.tag == "Ground")
                {
                    GameObject temp = (GameObject)Instantiate(Monster);                     // 프리팹 생성 함수 (몬스터를 생성)
                    temp.transform.position = hit.point + new Vector3(0.0f, 1.1f, 0.0f);   // 생성된 몬스터를 Ground에서 살짝 위에 생성 시킴

                    // cast.origin >> Ray를 쏜 시작점, hit.point는 Ray가 맞은 좌표
                    Debug.DrawLine(cast.origin, hit.point, Color.red, 2.0f);
                }
            }
        }
    }
}
