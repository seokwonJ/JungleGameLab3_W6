using UnityEngine;

public class GiantArea : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            ContactPoint contact = collision.GetContact(0);
            Vector3 reflectDir = Vector3.Reflect(transform.position - collision.transform.position, contact.normal);
            Vector3 bounce = reflectDir * 7f; // Æ¨±è °­µµ

            // Æ¨±â´Â È¿°ú Àû¿ë
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(bounce, ForceMode.Impulse);
            }
        }
    }
}
