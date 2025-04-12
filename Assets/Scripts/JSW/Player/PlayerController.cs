using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //쓰레기 리스트
    public List<int> trashList = new List<int>();



    //이하 청소기 관련 애니메이션 코드와 같음.
    public GameObject Vacuum; // -> 청소기, 일단 public 으로 뺴둠;
    private Renderer _vacuumRenderer;
    private Material _vacuumMaterial;
    private Coroutine _scaleCoroutine;
    private Vector3 _baseScale; 
    private void Start()
    {
        _vacuumRenderer = Vacuum.GetComponent<Renderer>();
        _vacuumMaterial = _vacuumRenderer.material;
    }
    private void Update()
    {

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

        int count = Mathf.Clamp(trashList.Count, 1, 5);
        float lerpFactor = (count - 1) / 4f;
        Color newColor = Color.Lerp(Color.green, Color.red, lerpFactor);
        _vacuumMaterial.color = newColor;


    }
}