using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    Canvas tutorialCanvas;
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
                //else
                //{
                //    // Last Tutorial
                //    panels[i].completed += NextTutorial;
                //}
                panels[i].ShowPanel(false);
            }
            

            if (panels.Length > 0)
            {
                panels[currTutorialIdx].ShowPanel(true);
                panels[currTutorialIdx].isActive = true;
            }
                
        }
        
    }

    private void Update()
    {
        
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
                    case "Long Attack Tutorial Panel":
                        panels[currTutorialIdx].ExtraIF1P = () => { return player1.trashList.Count > 0; };
                        panels[currTutorialIdx].ExtraIF2P = () => { return player2.trashList.Count > 0; };
                        break;

                }
            }
        }
    }
}
