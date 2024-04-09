using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherCotroller : MonoBehaviour
{
    //gracz
    GameObject player;
    //flaga. kt�ra m�wi czy zosta� ju� trafiony przeciwnik (basher) i zosta� za niego policzony
    bool HasBeenHit = false;

    private GameObject LevelManager;
    //pr�dko�� pod��ania za graczem
    public float walkSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        LevelManager = GameObject.Find("LevelManager");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
        //idz do przodu
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //je�li zosta� ju� trafiony to nic nie r�b
        if (HasBeenHit) { return; }

        /*Debug.Log("Trafiony!");*/
        //obiekt z kt�rym mamy kolizj�
        GameObject projectile = collision.gameObject;

        //tylko je�li trafi� nas gracz
        if (projectile.CompareTag("PlayerProjectile"))
        {
            //ustaw flage
            HasBeenHit = true;

            //dolicz punkty
            LevelManager.GetComponent<LevelManager>().addPoints(1);

            //zniknij pocisk
            Destroy(projectile);

            //zniknij przeciwnika
            Destroy(transform.gameObject);
        }
    }
}
