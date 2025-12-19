using UnityEngine;
using TMPro; 

public class CoinUI : MonoBehaviour
{
    [SerializeField] private PlayerController player; 
    [SerializeField] private TMP_Text coinText; 

    void Update()
    {
        if (player != null && coinText != null)
        {
            int coins = player.GetCollectedCount();
            coinText.text = $"Coins: {coins}";
        }
    }
}