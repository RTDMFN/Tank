using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    /*
    This is the base class responsible for all tank type enemies. It gives base methods to be used by the sub classes. 
    For now it allows any tank type object to move, rotate, aim, and fire.
    */
    
    public float baseRotateSpeed = 150f;
    public float baseMoveSpeed = 2f;
    public float baseFireCooldown = 0.5f;
    public float baseRecoilForce = 50f;
    protected float timeOfLastShot;

    public Transform tankBase;
    public Transform tankGun;
    
    protected Rigidbody2D tankBaseRB;

    public Transform shellSpawnPoint;
    public GameObject shellPrefab;
    public GameObject spawnEffect;

    protected virtual void Awake(){
        tankBaseRB = GetComponentInChildren<Rigidbody2D>();
        GameObject clone = Instantiate(spawnEffect,tankBaseRB.position,spawnEffect.transform.rotation);
        Destroy(clone,2);
    }

    public void Move(Vector2 directionToMove, float speed){
        if(GameManager.instance.State == GameState.Alive){
            tankBaseRB.MovePosition(tankBaseRB.position + directionToMove * speed * Time.deltaTime);
            UpdateGunPosition();
        }
    }

    public void Move(float throttle, float speed){
        if(GameManager.instance.State == GameState.Alive){
            tankBaseRB.MovePosition(tankBaseRB.position + (Vector2)tankBase.right  * throttle * speed * Time.deltaTime);
            UpdateGunPosition();
        }
    }

    public void Move(float speed){
        if(GameManager.instance.State == GameState.Alive){
            tankBaseRB.MovePosition(tankBaseRB.position + (Vector2)tankBase.right * speed * Time.deltaTime);
            UpdateGunPosition();
        }
    }

    public void Rotate(Vector2 directionToFace, float rotateSpeed){
        if(GameManager.instance.State == GameState.Alive){
            float angle = Mathf.Atan2(directionToFace.y,directionToFace.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle,Vector3.forward);
            tankBase.rotation = Quaternion.Slerp(tankBase.rotation,targetRotation,baseRotateSpeed * Time.deltaTime);
        }
    }

    public void Rotate(float angleToIncrement, float rotateSpeed){
        if(GameManager.instance.State == GameState.Alive) tankBaseRB.MoveRotation(tankBaseRB.rotation + -angleToIncrement * rotateSpeed * Time.deltaTime);
    }

    public void Aim(Vector3 targetPosition){
        if(GameManager.instance.State == GameState.Alive){
            Vector2 aimDirection = (targetPosition - tankBase.position).normalized;
            tankGun.right = aimDirection;
        }
    }

    public void Fire(float cooldown, float recoil){
        if(GameManager.instance.State == GameState.Alive){
            if(Time.time - timeOfLastShot >= cooldown || cooldown == 0){
                Instantiate(shellPrefab,shellSpawnPoint.position,tankGun.rotation);
                FindObjectOfType<AudioManager>().Play("Fire");
                tankBaseRB.AddForce((Vector2)tankGun.right * recoil * -1);
                timeOfLastShot = Time.time;
            }
        }
    }

    public void UpdateGunPosition(){
        if(GameManager.instance.State == GameState.Alive) tankGun.position = tankBase.position;
    }

}
