using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    private bool collected = false;

    private void Awake()
    {
        Debug.Log($"[Coin] Awake en '{name}'. Tengo CircleCollider2D? {TryGetComponent<CircleCollider2D>(out var c) && c.isTrigger}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Coin] {name} OnTriggerEnter2D con '{other?.name}' tag={other?.tag}");
        TryCollect(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void TryCollect(Collider2D other)
    {
        if (collected) return;
        if (other == null) return;
        if (!other.CompareTag("Player")) return;

        collected = true;
        Debug.Log($"[Coin] Moneda '{name}' recogida. +{scoreValue} pts");
        Destroy(gameObject);
    }
}