using System.Collections;
using System.Collections.Generic;
using System.Linq;          // Linq 를 사용
using UnityEngine;

public enum FxType
{
    hit,
    Explosion
}

[System.Serializable]
public class FxBundle
{
    [SerializeField] private FxType fxType;             // 이펙트 타입
    [SerializeField] private GameObject fxPrefab;       // Fx 프리팹 선언
    public FxType FxType => fxType;
    public GameObject FxPrefab => fxPrefab;
}

public class FxManager : MonoBehaviour
{
    // -------------싱글톤-----------------
    public FxManager() { }
    public static FxManager Instance { get; private set; }

    // -------------싱글톤-----------------

    [SerializeField] private List<FxBundle> fxList;

    public Dictionary<FxType, GameObject> FXDict { get; private set; } = new Dictionary<FxType, GameObject>();
    public List<FxBundle> FxList => fxList;

    private void Awake()
    {
        if(Instance)                                   // 인스턴스가 존재하는지 확인
        {
            Destroy(gameObject);                       // 중복도니 오브젝트가 있으면 지금것 삭제
            return;
        }
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);              // 이 함수는 Scene 이 변경되도 사라지지 않게 한다.
            for(int i = 0; i < FxType.GetValues(typeof(FxType)).Length; i++)
            {
                FXDict.Add((FxType)i, FxList.FirstOrDefault(x => x.FxType == (FxType)i)?.FxPrefab);
            }
        }
    }

    public void PlayFx(Transform targetTransform, FxType targetFx, Vector3 Offset)         // 싱글톤된 함수 어디서든 Fx 를 불러올 수 있게 함.
    {
        Instantiate(FXDict[targetFx], targetTransform.position + Offset, new Quaternion(0.0f, 0.0f, 0.0f, 1.0f));       // 받아온 인수들을 활용하여 해당 위치에 Fx 생성
    }
}
