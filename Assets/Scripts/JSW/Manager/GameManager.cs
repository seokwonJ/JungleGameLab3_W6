using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public TMP_Text timeText;
    public Text p1ScoreText;
    public Text p2ScoreText;
    public GameObject obstacleObjectList;
    public GameObject player1;
    public GameObject player2;
    public GameObject playerCanvas;

    //Text
    public GameObject finishText;
    public GameObject ReadyText;
    public GameObject GoText;

    //Ending
    public GameObject endingCanvas;
    public VideoPlayer winVideoPlayer;
    public VideoPlayer loseVideoPlayer;
    public Texture winEnding_Texture;
    public Texture loseEnding_Texture;
    public GameObject restartButton;
    public GameObject exitButton;

    private float _time = 60;
    private bool _isHalf;
    private bool _isEnd;
    private bool _isTrashSpawn;
    private int _p1TrashCount = 0;
    private int _p2TrashCount = 0;
    private bool _isStart;
    Animator _timeTextAnimator;
    Animator _trashNum1Animator;
    Animator _trashNum2Animator;
    ObstacleSpawnManager _obstacleSpawnManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 인스턴스가 이미 존재하면 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스 설정
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0;
        _timeTextAnimator = timeText.GetComponent<Animator>();
        _trashNum1Animator = p1ScoreText.GetComponent<Animator>();
        _trashNum2Animator = p2ScoreText.GetComponent<Animator>();
        _obstacleSpawnManager = obstacleObjectList.GetComponent<ObstacleSpawnManager>();
        _obstacleSpawnManager.SetOverSoon(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isStart) return;
        if (_time < 0)
        {
            _timeTextAnimator.enabled = false;
            if (!_isEnd)
            {
                //Time.timeScale = 0f;

                player1.GetComponent<PlayerInput>().enabled = false;
                player2.GetComponent<PlayerInput>().enabled = false;
                _isEnd = true;
                finishText.SetActive(true);
                Invoke("DropTrash", 1.5f);
            }
        }
        else
        {
            _time -= Time.deltaTime;
            timeText.text = ((int)_time).ToString();
        }


        if (_time < 6 && !_isTrashSpawn)
        {
            _timeTextAnimator.Play("PlayTimer2", 0, 0f);
            _isTrashSpawn = true;
            _obstacleSpawnManager.SetOverSoon(true);
        }

        if (_time < 30 && !_isHalf)
        {
            _isHalf = true;
            _timeTextAnimator.Play("PlayTimer", 0, 0f);
        }

    }
    void DropTrash()
    {
        finishText.SetActive(false);
        player1.GetComponent<PlayerController>().DropObstacles();
        player2.GetComponent<PlayerController>().DropObstacles();
        Invoke("countingTrash", 3f);
    }

    void countingTrash()
    {
        Time.timeScale = 0f;
        playerCanvas.SetActive(false);
        endingCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        endingCanvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        endingCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        endingCanvas.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);

        if (_p1TrashCount < _p2TrashCount)
        {
            endingCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            endingCanvas.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
            endingCanvas.transform.GetChild(0).GetChild(1).GetComponent<RawImage>().texture = winEnding_Texture;
            endingCanvas.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().texture = loseEnding_Texture;
        }
        else
        {
            endingCanvas.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            endingCanvas.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            endingCanvas.transform.GetChild(0).GetChild(1).GetComponent<RawImage>().texture = loseEnding_Texture;
            endingCanvas.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().texture = winEnding_Texture;
        }
        winVideoPlayer.Play();
        loseVideoPlayer.Play();
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void SceneLoadNum(int i)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(i);
    }

    public void UpdateScore(bool isRight, int num)
    {
        if ((p2ScoreText == null) || (p1ScoreText == null)) return;
        if (isRight)
        {
            _p2TrashCount += num;
            _trashNum2Animator.Play("TrashNumUpdate", 0, 0f);
            p2ScoreText.text = _p2TrashCount.ToString();
        }
        else
        {
            _p1TrashCount += num;
            _trashNum1Animator.Play("TrashNumUpdate", 0, 0f);
            p1ScoreText.text = _p1TrashCount.ToString();
        }
    }

    public void GameStart()
    {
        Time.timeScale = 1;
        StartCoroutine(GameStart_CO());
    }

    IEnumerator GameStart_CO()
    {
        ReadyText.SetActive(true);
        yield return new WaitForSeconds(1f);
        ReadyText.SetActive(false);
        GoText.SetActive(true);
        yield return new WaitForSeconds(1f);
        GoText.SetActive(false);
        _isStart = true;
        _obstacleSpawnManager.SetOverSoon(false);
    }
}
