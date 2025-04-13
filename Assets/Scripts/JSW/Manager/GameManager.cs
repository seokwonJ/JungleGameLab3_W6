using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public TMP_Text timeText;
    public Text p1ScoreText;
    public Text p2ScoreText;
    public GameObject restartButton;
    public GameObject obstacleObjectList;
    public GameObject player1;
    public GameObject player2;
    public GameObject endingCanvas;
    public GameObject finishText;
    public  float _time = 60;

    private bool _isHalf;
    private bool _isEnd;
    private bool _isTrashSpawn;
    private int _p1TrashCount = 0;
    private int _p2TrashCount = 0;
    Animator _timeTextAnimator;
    Animator _trashNum1Animator;
    Animator _trashNum2Animator;

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
        _timeTextAnimator = timeText.GetComponent<Animator>();
        _trashNum1Animator = p1ScoreText.GetComponent<Animator>();
        _trashNum2Animator = p2ScoreText.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
            obstacleObjectList.GetComponent<ObstacleSpawnManager>().SetOverSoon(true);
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
        Invoke("countingTrash", 2f);
    }

    void countingTrash()
    {
        Time.timeScale = 0f;
        if (_p1TrashCount < _p2TrashCount)
        {
            endingCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            endingCanvas.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            endingCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            endingCanvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        restartButton.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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
}
