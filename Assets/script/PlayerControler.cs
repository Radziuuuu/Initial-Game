using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public float moveSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //pobierz stan kontrolera (poziomo)
        float x = Input.GetAxis("Horizontal");
        //dodaj do obecnej pozycji jedną jednostkę "w prawo"
        Vector3 movement = Vector3.right * x;

        //pobierz stan kotrolera (pion)
        float y = Input.GetAxis("Vertical");
        movement += Vector3.forward * y;

        //Normalizuj ruch
        movement += movement.normalized;

        //przelicz przez czas ostatniej lkatki
        movement *= Time.deltaTime;

        //Nadanie prędkości
        movement *= moveSpeed;


        //dodaj zmianę położenia na ohbiekt gracza
        transform.position += movement;
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.ToString());
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Gracz Trafiony");
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
            
        }
    }

}
