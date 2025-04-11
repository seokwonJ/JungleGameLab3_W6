using System.Collections;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private int _attackLimit;
    private int _attackLimitUp = 10;
    private int _speedUp = 3;

    // 공격 한도 증가
    public void Start_AttackLimitUp(GameObject player)
    {
        StartCoroutine(AttackLimitUp(player));
    }

    IEnumerator AttackLimitUp(GameObject player)
    {
        print("Player");
        CleanerArea cleanerArea = player.transform.GetChild(0).GetComponent<CleanerArea>();
        _attackLimit = cleanerArea.GetAttackLimit();
        cleanerArea.SetAttackLimit(_attackLimitUp);
        yield return new WaitForSeconds(5f);
        cleanerArea.SetAttackLimit(_attackLimit);
        print("return");
    }

    // 공격 한도 증가

    public void Start_SpeedUp(GameObject player)
    {
        StartCoroutine(SpeedUp(player));
    }

    IEnumerator SpeedUp(GameObject player)
    {
        print("Player");
        PlayerMove playerMove = player.transform.GetComponent<PlayerMove>();
        float playerSpeed = playerMove.speed;

        playerMove.speed = playerSpeed * _speedUp;

        yield return new WaitForSeconds(5f);

        playerMove.speed = playerSpeed;

        print("return");
    }

    public void Start_ScaleUp(GameObject player)
    {
        StartCoroutine(ScaleUp(player));
    }

    IEnumerator ScaleUp(GameObject player)
    {
        int scaleUp = 5;
        player.GetComponent<Rigidbody>().mass = 100;

        while(true)
        {
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, Vector3.one * scaleUp, Time.deltaTime);
            if (Vector3.Distance(Vector3.one * scaleUp, player.transform.localScale) < 0.1f)
            {
                player.transform.localScale = Vector3.one * scaleUp;
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(5f);

        scaleUp = 1;
        player.GetComponent<Rigidbody>().mass = 1;

        while (true)
        {
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, Vector3.one, Time.deltaTime);
            if (Vector3.Distance(player.transform.localScale, Vector3.one) < 0.1f)
            {
                player.transform.localScale = Vector3.one;
                break;
            }
            yield return null;
        }

    }
}





