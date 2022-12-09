using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameState State;

    public Animator sceneTransitionAnim;

    void Awake(){
        instance = this;
        sceneTransitionAnim.Play("OpenScene");
    }

    void SwitchState(GameState state){
        switch(state){
            case GameState.Alive:
                HandleAliveState();
                break;
            case GameState.Paused:
                HandlePausedState();
                break;
            case GameState.Dead:
                HandleDeadState();
                break;
            case GameState.Default:
                break;
        }
    }

    public void Lose(){
        SwitchState(GameState.Dead);
    }

    public void Start(){
        SwitchState(GameState.Alive);

    }

    public void Pause(){
        if(State == GameState.Paused){
            SwitchState(GameState.Alive);
            MenuManager.instance.HidePauseScreen();
        }else if(State == GameState.Alive){
            SwitchState(GameState.Paused);
        }else{
            return;
        }
    }

    void HandleAliveState(){
        State = GameState.Alive;
        MenuManager.instance.ShowGameScreen();
    }

    void HandlePausedState(){
        State = GameState.Paused;
        MenuManager.instance.ShowPauseScreen();
    }

    void HandleDeadState(){
        State = GameState.Dead;
        MenuManager.instance.ShowDeathScreen();
    }

    public void LoadGameScene(){
        SceneManager.LoadScene("Main");
    }

    public void LoadMainMenuScene(){
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene(){
        sceneTransitionAnim.SetTrigger("CloseScene");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("MainMenu");
    }

}

public enum GameState{
    Default,
    Alive,
    Paused,
    Dead
}
