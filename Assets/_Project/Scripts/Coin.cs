using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Coin] Trigger con '{other?.name ?? "null"}' tag={other?.tag ?? "null"}");
        if (other == null) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[Coin] Moneda recogida. +{scoreValue} pts");
        Destroy(gameObject);
    }
}