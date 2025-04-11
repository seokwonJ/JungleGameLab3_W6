using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text p1ScoreText;
    public TMP_Text p2ScoreText;
    public GameObject restartButton;
    public GameObject obstacleObjectList;
    private float _time = 60;
    private bool isEnd;

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;
        timeText.text = ((int)_time).ToString();

        if(_time  < 0 && !isEnd)
        {
            Time.timeScale = 0f;
            isEnd = true;
            countingTrash();
        }
    }

    void countingTrash()
    {
        int p1Score = 0;
        int p2Score = 0;
        for (int i = 0;i <obstacleObjectList.transform.childCount;i++)
        {
            if (obstacleObjectList.transform.GetChild(i).position.x < 0)
            {
                p1Score += 1;
            }
            else
            {
                p2Score += 1;
            }
        }
        print(p1Score + " " + p2Score);

        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
        restartButton.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
