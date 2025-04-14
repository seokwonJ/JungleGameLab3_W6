using System.Collections;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private int _attackLimit;
    private int _attackLimitUp = 10;
    private int _speedUp = 3;
    private ItemSpawner _itemSpawner;

    private void Start()
    {
        _itemSpawner = GetComponent<ItemSpawner>();
    }

    // 공격 한도 증가
    public void Start_AttackLimitUp(GameObject player)
    {
        _itemSpawner.GetItem(true);
        StartCoroutine(AttackLimitUp(player));
    }

    IEnumerator AttackLimitUp(GameObject player)
    {;
        CleanerArea cleanerArea = player.transform.GetChild(0).GetComponent<CleanerArea>();
        _attackLimit = cleanerArea.GetAttackLimit();
        cleanerArea.SetAttackLimit(_attackLimitUp);
        yield return new WaitForSeconds(5f);
        cleanerArea.SetAttackLimit(_attackLimit);
    }

    // 속도 증가
    public void Start_SpeedUp(GameObject player)
    {
        _itemSpawner.GetItem(true);
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

    // 크기 증가
    public void Start_ScaleUp(GameObject player)
    {
        _itemSpawner.GetItem(true);
        StartCoroutine(ScaleUp(player));
    }

    IEnumerator ScaleUp(GameObject player)
    {
        int scaleUp = 4;
        player.GetComponent<Rigidbody>().mass = 100;                                // 쓰레기들 때문에 날아가지 않도록
        player.transform.GetChild(2).gameObject.SetActive(true);                    // 거인 area
        player.transform.GetChild(0).GetComponent<CleanerArea>().enabled = false;   // 빨아들이지 못하도록
        player.layer = LayerMask.NameToLayer("Giant");


        while (true)
        {
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, Vector3.one * scaleUp, Time.deltaTime);
            if (Vector3.Distance(Vector3.one * scaleUp, player.transform.localScale) < 0.1f)
            {
                player.transform.localScale = Vector3.one * scaleUp;
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        scaleUp = 1;
        player.GetComponent<Rigidbody>().mass = 1;
        player.layer = LayerMask.NameToLayer("Player");

        while (true)
        {
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, Vector3.one, Time.deltaTime * 5);
            if (Vector3.Distance(player.transform.localScale, Vector3.one) < 0.1f)
            {
                player.transform.localScale = Vector3.one;
                break;
            }
            yield return null;
        }
        player.transform.GetChild(2).gameObject.SetActive(false);
        player.transform.GetChild(0).GetComponent<CleanerArea>().enabled = true;
    }
}





