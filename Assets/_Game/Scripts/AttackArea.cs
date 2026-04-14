using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
           collision.GetComponent<Character>().OnHit(30f);
            Debug.Log(collision.gameObject.name + " hit by attack area");

        }
    }
}
