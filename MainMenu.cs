using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuUI;

    public Animator sceneTransitionAnim;

    void Start(){
        Tween();
        sceneTransitionAnim.Play("OpenScene");
    }

    void Tween(){
        LeanTween.scale(mainMenuUI,mainMenuUI.transform.localScale * 1.05f,0.2f).setLoopPingPong();
    }

    public void StartGame(){
        StartCoroutine(ChangeScene());
    }

    public void QuitGame(){
        Application.Quit();
    }

    IEnumerator ChangeScene(){
        sceneTransitionAnim.Play("CloseScene");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Main");
    }
    
}
