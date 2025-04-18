﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //쓰레기 리스트
    public List<int> trashList = new List<int>();
    public GameObject trash;
    public GameObject ice;
    public GameObject banana;
    public GameObject bottle;
    public GameObject box;
    public GameObject can;
    public GameObject canTrashBox;
    public GameObject plasticBox;

    private Transform _trashListObject;
    private PlayerMove _playerMove;

    //이하 청소기 관련 애니메이션 코드와 같음.
    public GameObject Vacuum; // -> 청소기, 일단 public 으로 뺴둠;
    private Renderer _vacuumRenderer;
    private Material _vacuumMaterial;
    private Coroutine _scaleCoroutine;
    private Vector3 _baseScale;
    private void Start()
    {
        _trashListObject = FindAnyObjectByType<ObstacleSpawnManager>().transform;
        _playerMove = GetComponent<PlayerMove>();
        _vacuumRenderer = Vacuum.GetComponent<Renderer>();
        _vacuumMaterial = _vacuumRenderer.material;
    }

    public void Update_Trash() // 외부에서 호출할 청소기 상황 업데이트 함수;
    {
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
            Vacuum.transform.localScale = _baseScale;
        }

        _baseScale = Vacuum.transform.localScale;

        _scaleCoroutine = StartCoroutine(AnimateVacuumScale());
        UpdateVacuumColor();
        _playerMove.addSpeed = -trashList.Count * 0.1f;
    }


    private IEnumerator AnimateVacuumScale() // 청소기 뽀용 효과.
    {
        Vector3 currentScale = Vacuum.transform.localScale;
        Vector3 targetScale = currentScale * 1.4f;

        float duration = 0.2f;
        float t = 0f;

        while (t < duration)
        {
            Vacuum.transform.localScale = Vector3.Lerp(currentScale, targetScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        Vacuum.transform.localScale = targetScale;

        t = 0f;
        while (t < duration)
        {
            Vacuum.transform.localScale = Vector3.Lerp(targetScale, currentScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        Vacuum.transform.localScale = currentScale;
    }

    private void UpdateVacuumColor() // 청소기 색상 업데이트; 5개에 가까워질 수록 빨간색으로!
    {
        if (_vacuumMaterial == null) return;

        int count = Mathf.Clamp(trashList.Count, 1, 50);
        float lerpFactor = (count - 1) / 50f;
        Color newColor = Color.Lerp(Color.green, Color.red, lerpFactor);
        _vacuumMaterial.color = newColor;
    }


    public void DropObstacles()
    {
        GameObject shootObject = null;
        for (int i = 0; i < trashList.Count; i++)
        {
            int x = Random.Range(-1, 2);
            int z = Random.Range(-1, 2);

            switch (trashList[i])
            {
                case 1:
                    shootObject = Instantiate(trash, transform.position + Vector3.up * i * 0.05f + new Vector3(x, 0, z), Quaternion.identity, _trashListObject);
                    break;
                case 2:
                    shootObject = Instantiate(ice, transform.position + Vector3.up * i * 0.05f + new Vector3(x, 0, z), Quaternion.identity, _trashListObject);
                    break;
                case 3:
                    shootObject = Instantiate(banana, transform.position + Vector3.up * i * 0.05f + new Vector3(x, 0, z), Quaternion.identity, _trashListObject);
                    break;
                case 6:
                    shootObject = Instantiate(bottle, transform.position + Vector3.up * i * 0.05f + new Vector3(x, 0, z), Quaternion.identity, _trashListObject);
                    break;
                case 7:
                    shootObject = Instantiate(box, transform.position + Vector3.up * i * 0.05f + new Vector3(x, 0, z), Quaternion.identity, _trashListObject);
                    break;
                case 8:
                    shootObject = Instantiate(can, transform.position + Vector3.up * i * 0.05f + new Vector3(x, 0, z), Quaternion.identity, _trashListObject);
                    break;
                case 9:
                    shootObject = Instantiate(canTrashBox, transform.position + Vector3.up * i * 0.05f + new Vector3(x, 0, z), Quaternion.identity, _trashListObject);
                    break;
                case 10:
                    shootObject = Instantiate(plasticBox, transform.position + Vector3.up * i * 0.05f + new Vector3(x, 0, z), Quaternion.identity, _trashListObject);
                    break;
                default:
                    break;
            }
        }
        trashList.Clear();
    }
}