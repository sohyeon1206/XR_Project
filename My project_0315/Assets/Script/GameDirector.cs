using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public GameObject character;
    public GameObject flag;
    public GameObject distance;

    void Start()
    {
        character = GameObject.Find("chracterPiovt");
        flag = GameObject.Find("flagPivot");
        distance = GameObject.Find("UIDistance");
    }

    // Update is called once per frame
    void Update()
    {
        //float length = flag.transform.position.z - character.transform.position.z;

        float VectorLength = Vector3.Distance(flag.transform.position, character.transform.position);

        distance.GetComponent<Text>().text = "목표 지점까지" + VectorLength.ToString("F2") + "m";
    }
}
