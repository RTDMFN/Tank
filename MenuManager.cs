using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI deathText;

    public GameObject deathScreen;
    public GameObject gameScreen;
    public GameObject pausedScreen;

    public static MenuManager instance;

    void Awake(){
        instance = this;
        StartGame();
    }

    void Update(){
        UpdateTimerText();
        UpdateDeathText();
        if(Input.GetKeyDown(KeyCode.Z)) ScaleUIElement(deathScreen);
        if(Input.GetKeyDown(KeyCode.C)) HideUIElement(deathScreen);
    }

    void UpdateTimerText(){
        if(GameManager.instance.State == GameState.Alive) timerText.text = Time.timeSinceLevelLoad.ToString("#.00");
    }

    void UpdateDeathText(){
        if(GameManager.instance.State == GameState.Alive) deathText.text = "You survived " + timerText.text + "s";
    }

    void StartGame(){
        pausedScreen.SetActive(true);
        deathScreen.SetActive(true);
        gameScreen.SetActive(true);
        pausedScreen.transform.localScale = Vector3.zero;
        deathScreen.transform.localScale = Vector3.zero;
        gameScreen.transform.localScale = Vector3.zero;
    }

    public void ShowPauseScreen(){
        gameScreen.transform.localScale = Vector3.zero;
        ScaleUIElement(pausedScreen);
    }

    public void HidePauseScreen(){

        HideUIElement(pausedScreen);
    }

    public void ShowGameScreen(){
        gameScreen.transform.localScale = Vector3.one;
    }

    public void ShowDeathScreen(){
        gameScreen.transform.localScale = Vector3.zero;
        ScaleUIElement(deathScreen);
    }

    void HideUIElement(GameObject uiElement){
        LeanTween.cancelAll();
        LeanTween.scale(uiElement,Vector3.zero,0.1f);
    }

    void ScaleUIElement(GameObject uiElement){
        LeanTween.scale(uiElement,new Vector3(1,1,1),0.2f).setOnComplete(() => Pop(uiElement));
    }

    void Pop(GameObject uiElement){
        LeanTween.scale(uiElement,uiElement.transform.localScale * 1.05f,0.2f).setLoopPingPong();
    }

}
