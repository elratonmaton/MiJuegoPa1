using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[Coin] Moneda '{name}' recogida. +{scoreValue} pts");
        Destroy(gameObject);
    }
}
