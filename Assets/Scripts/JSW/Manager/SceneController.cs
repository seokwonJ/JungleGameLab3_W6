using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneMove(int i)
    {
        SceneManager.LoadScene(i);
    }
}
