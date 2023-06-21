using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;               // UI 추가 
using System;                       // 추가 

public class GameManager : MonoBehaviour
{
    protected SceneChanger SceneChanger => SceneChanger.Instance;
    public enum GameState           //게임 상태값 설정
    {
        Start,
        Playing,
        GameOver
    }

    public event Action<GameState> OnGameStateChanged;

    public GameState currentState = GameState.Start;

    public GameState CurrentState
    {
        get { return currentState; }
        private set
        {
            currentState = value;
            OnGameStateChanged?.Invoke(currentState);       //이벤트가 null이 아닌경우에만 이 이벤트를 호출 
        }
    }

    public HUDTextManager hudmanager;

    public void StartGame()
    {   //게임 시작 로직을 여기에 작성
        CurrentState = GameState.Playing;
    }

    public void GameOver()
    {   //게임 오버 로직을 여기에 작성
        CurrentState = GameState.GameOver;
        SceneChanger.LoadEndScene();
    }

    public GameManager() { }
    public static GameManager Instance { get; private set; }    //싱글톤화
    public PlayerHp playerHp;                   //플레이어의 Hp
    public Image playerHpUIImage;               //플레이어 Hp UI 이미지
    public Button BtnSample;                    //UI 버튼 선언

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        Init();
    }

    private void OnDestroy()    //이 오브젝트가 파괴될 경우
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  //이벤트를 삭제한다. 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameScene")
        {
            Init();
        }
    }

    private void Init()
    {
        playerHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHp>();                     //Tag로 오브젝트를 찾는다.
        playerHpUIImage = GameObject.FindGameObjectWithTag("UIHealthBar").GetComponent<Image>();            //Tag로 UI를 찾는다.
        playerHp.Hp = 100;
        CurrentState = GameState.Start;
        hudmanager = this.gameObject.GetComponent<HUDTextManager>();
        hudmanager.canvasObject = GameObject.Find("Canvas");
        BtnSample = GameObject.Find("GameOverButton").GetComponent<Button>();
        hudmanager.hudText = GameObject.Find("Player_Text").GetComponent<Text>();
        hudmanager.character = GameObject.Find("Tank_01.vox");

    }
    private void Update()
    {
        playerHpUIImage.fillAmount = (float)playerHp.Hp / 100.0f;                                            //체력에 비례하게 작업

        if (Input.GetKey("escape"))             // esc를 누르면 게임 종료
            Application.Quit();
    }
}