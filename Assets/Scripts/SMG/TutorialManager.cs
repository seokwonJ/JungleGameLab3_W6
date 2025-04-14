using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    Canvas tutorialCanvas;
    Canvas systemCanvas;
    TutorialPanel[] panels;
    int currTutorialIdx;
    PlayerController player1;
    PlayerController player2;
    ObstacleSpawnManager spawnManager;


    private void Awake()
    {
        tutorialCanvas = FindAnyObjectByType<TutorialPanel>().transform.parent.GetComponent<Canvas>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length >= 2)
        {
            bool isPlayer1First = players[0].GetComponent<PlayerInput>().actions.name.Contains("Player1");
            player1 = ((isPlayer1First) ? players[0] : players[1]).GetComponent<PlayerController>();
            player2 = ((isPlayer1First) ? players[1] : players[0]).GetComponent<PlayerController>();
        }

        spawnManager = FindAnyObjectByType<ObstacleSpawnManager>();
        spawnManager?.SetOverSoon(true);

        systemCanvas = GameObject.Find("System Canvas").GetComponent<Canvas>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(tutorialCanvas != null)
        {
            panels = tutorialCanvas.GetComponentsInChildren<TutorialPanel>();
            currTutorialIdx = 0;

            for(int i = 0; i < panels.Length; i++)
            {
                if(i < panels.Length - 1)
                {
                    panels[i].completed += NextTutorial;
                }
                else
                {
                    // Last Tutorial
                    panels[i].completed += FinishTutorial;
                }
                panels[i].ShowPanel(false);
            }
            

            if (panels.Length > 0)
            {
                panels[currTutorialIdx].ShowPanel(true);
                panels[currTutorialIdx].isActive = true;
            }
                
        }

        if(systemCanvas != null)
        {
            Button[] buttons = systemCanvas.GetComponentsInChildren<Button>(true);
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.AddListener(OnReturnTitleScene);
            }
        }
        
    }


    int getTrashCountBeforeAttackP1;
    int getTrashCountBeforeAttackP2;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            getTrashCountBeforeAttackP1 = player1.trashList.Count;
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            getTrashCountBeforeAttackP1 = 0;
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            getTrashCountBeforeAttackP2 = player2.trashList.Count;
        }
        else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            getTrashCountBeforeAttackP2 = 0;
        }
    }

    void NextTutorial()
    {
        if (currTutorialIdx < panels.Length - 1)
        {
            panels[currTutorialIdx].ShowPanel(false);
            panels[currTutorialIdx].isActive = false;
            currTutorialIdx++;
            panels[currTutorialIdx].ShowPanel(true);
            panels[currTutorialIdx].isActive = true;

            if (player1 != null)
            {
                switch (panels[currTutorialIdx].name)
                {
                    case "Clean Tutorial Panel":
                        spawnManager?.SetOverSoon(false);
                        panels[currTutorialIdx].ExtraIF1P = () => { return player1.trashList.Count > 0; };
                        panels[currTutorialIdx].ExtraIF2P = () => { return player2.trashList.Count > 0; };
                        break;
                    case "Short Attack Tutorial Panel":
                        panels[currTutorialIdx].ExtraIF1P = () => { return player1.trashList.Count > 0; };
                        panels[currTutorialIdx].ExtraIF2P = () => { return player2.trashList.Count > 0; };
                        break;
                    case "Long Attack Tutorial Panel":
                        panels[currTutorialIdx].ExtraIF1P = () => { return getTrashCountBeforeAttackP1 > 0; };
                        panels[currTutorialIdx].ExtraIF2P = () => { return getTrashCountBeforeAttackP2 > 0; };
                        break;

                }
            }
        }
    }

    void FinishTutorial()
    {
        panels[currTutorialIdx].ShowPanel(false);
        panels[currTutorialIdx].isActive = false;

        systemCanvas.transform.GetChild(1).gameObject.SetActive(true);
        systemCanvas.transform.GetChild(2).gameObject.SetActive(true);

    }


    public void OnReturnTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
}

