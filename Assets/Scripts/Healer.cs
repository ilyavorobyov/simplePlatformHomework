using UnityEngine;

public class Healer : MonoBehaviour
{
    private int _addingHealth = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.AddHealth(_addingHealth);
            Destroy(gameObject);
        }
    }
}