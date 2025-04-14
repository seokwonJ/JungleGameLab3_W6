using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    Vector3 dir;
    float time = 0.5f;
    float nowTime;

    private void Start()
    {
        dir = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * Time.deltaTime* 0.5f;

        nowTime += Time.deltaTime;
        if (nowTime > time)
        {
            nowTime = 0;
            dir *= -1;
        }
    }
}
