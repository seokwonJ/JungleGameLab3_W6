using UnityEngine;

public class Item : MonoBehaviour
{
    protected ItemManager itemManager;
    private void Start()
    {
        itemManager = FindAnyObjectByType<ItemManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾�� �浹�� ��� �߰� ó��
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerGetItem(collision.gameObject);
            Destroy(gameObject);
        }
    }

    public virtual void PlayerGetItem(GameObject player)
    {
        Debug.Log("Hello from Item Parent!");
    }
}
