using TMPro;
using UnityEngine;

public class CoinInventoryUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI coinsText;
    
    private void Start() {
        ChangeText("0");
    }
    
    public void ChangeText(string text) {
        coinsText.SetText(text);
    }
}
