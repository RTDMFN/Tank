using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Shell : MonoBehaviour
{

    public float shellSpeed = 5f;

    Rigidbody2D shellHeadRB;
    RaycastHit2D hit;

    public GameObject explosionEffect;

    int bounceMax = 3;
    int bounceCounter = 0;

    void Awake(){
        shellHeadRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        shellHeadRB.MovePosition(shellHeadRB.position + (Vector2)transform.right * shellSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.transform.parent.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Play("Death");
            GameObject clone = Instantiate(explosionEffect,other.transform.position,other.transform.rotation);
            Destroy(clone,5);
            CameraShaker.Instance.ShakeOnce(2f,4f,.1f,1f);
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Player"){
            FindObjectOfType<AudioManager>().Play("Death");
            other.gameObject.transform.parent.gameObject.SetActive(false);
            GameManager.instance.Lose();
            GameObject clone = Instantiate(explosionEffect,other.transform.position,other.transform.rotation);
            Destroy(clone,5);
            CameraShaker.Instance.ShakeOnce(2f,4f,.1f,1f);
            Destroy(this.gameObject);
        }

        if(bounceCounter >= bounceMax) Destroy(this.gameObject);
        else{
            hit = Physics2D.Raycast(shellHeadRB.position,(Vector2)transform.right);
            if(hit.collider != null){
                bounceCounter++;
                FindObjectOfType<AudioManager>().Play("Bounce");
                transform.right = Vector2.Reflect(transform.right,hit.normal);
            }
        }
    }

}
