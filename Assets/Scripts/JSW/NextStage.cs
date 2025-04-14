using UnityEngine;

public class NextStage : MonoBehaviour
{
    public GameObject _nextStage;

    private void Start()
    {
        GameManager.Instance.p1WinAction += SetActiveStage;
    }

    public void SetActiveStage()
    {
        _nextStage.SetActive(true);
    }
}
