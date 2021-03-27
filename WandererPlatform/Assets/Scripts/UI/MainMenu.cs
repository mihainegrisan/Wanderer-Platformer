using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] private Button continueGameButton;
    [SerializeField] private TextMeshProUGUI continueGameText;
    [SerializeField] private TMP_ColorGradient disabledColorGradient;
    [SerializeField] private TMP_ColorGradient enabledColorGradient;
    private static string path;
    private VertexGradient textGradient;

    private void Start() {
        path = Application.persistentDataPath + "/game.save";

        if (!File.Exists(path)) {
            continueGameButton.interactable = false;
            //continueGameText.enableVertexGradient = false;
            //continueGameText.color = new Color(27, 11, 11, 255);
            //continueGameText.color = new Color(33, 32, 10);
            continueGameText.colorGradientPreset = disabledColorGradient;
            print("disabled");
        }
        else {
            continueGameButton.interactable = true;
            //continueGameText.enableVertexGradient = true;
            //continueGameText.color = Color.white;
            continueGameText.colorGradientPreset = enabledColorGradient;
            print("enabled");
        }
    }

    public void ContinueGame() {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        GameManager.Instance.ContinueGame();
    }

    public void NewGame() {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        GameManager.Instance.NewGame();
    }

    public void QuitGame() {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        Application.Quit();
    }
}
