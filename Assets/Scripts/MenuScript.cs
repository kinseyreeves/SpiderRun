using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public Text startText;
    public Button startButton;
	void Start () {
        startButton = startText.GetComponent<Button>();
        startButton.onClick.AddListener(() => { startGame(); });

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
