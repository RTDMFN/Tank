using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{

    /*
    This is the player class and inherits the features of the tank class.
    This class takes the player's inputs and passes them into their respective methods in the parent class.
    */

    float rotateInput;
    float throttleInput;

    void Update(){
        HandleInputs();
        if(Input.GetMouseButtonDown(0)) Fire(baseFireCooldown,baseRecoilForce);

        if(Input.GetKeyDown(KeyCode.Escape)) GameManager.instance.Pause();
    }

    void FixedUpdate(){
            Rotate(rotateInput,baseRotateSpeed);
            Move(throttleInput,baseMoveSpeed);  
            Aim(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void HandleInputs(){
        rotateInput = Input.GetAxisRaw("Horizontal");
        throttleInput = Input.GetAxisRaw("Vertical");
    }
    
}
