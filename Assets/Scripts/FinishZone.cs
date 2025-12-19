using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class FinishZone : MonoBehaviour
{
    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            int rotations = player.GetRotationCount();
            int collected = player.GetCollectedCount();
            Debug.Log($"Фініш! Обертів: {rotations}  |  Зібрано предметів: {collected}");
            SceneManager.LoadScene("Menu");
        }
    }
}
