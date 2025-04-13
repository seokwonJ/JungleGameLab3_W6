using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> _itemList;
    private float _intervalTime= 10;
    private float _nowTime = 5;
    private bool _doGetItem = true;

    private void Update()
    {
        if (_doGetItem)
        {
            _nowTime += Time.deltaTime;
        }
        if (_nowTime > _intervalTime)
        {
            _nowTime = 0;
            int num = Random.Range(0, 2);
            Instantiate(_itemList[num], Vector3.zero, Quaternion.identity);
            _doGetItem = false;
        }
    }

    public void GetItem(bool result)
    {
        _doGetItem = result;
    }


}
