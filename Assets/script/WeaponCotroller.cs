using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCotroller : MonoBehaviour
{
    public float range = 10f;

    //transform gracza
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("player").transform;
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        Transform target = TagTargetter("Enemy");
        if (target != transform)
        {
            Debug.Log("Celuje do: " + target.gameObject.name);
            transform.LookAt(target.position + Vector3.up);
        }
    }


    Transform TagTargetter(string tag)
    {
        //Tablica wszystkich obiekt�w pasuj�cych do taga podanego jako argument
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        //szukamy najbli�szego
        Transform closestTarget = transform;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject target in targets)
        {
            //wektor przesuni�cia wzgl�dem gracza
            Vector3 difference = target.transform.position - player.position;
            //odleglosc od gracza
            float distance = difference.magnitude;
            if(distance < closestDistance && distance < range) 
            {
                closestTarget = target.transform;
                closestDistance = distance;
            }
            
        }
        return closestTarget;
    }
       
    Transform LegeacyTargeter()
        {
        //Znajdz wszystkie colidery w promieniu = range izapisz je do tablicy collidersInRange
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        //do cel�w testowych

        // Debug.Log("Ilosc collider�w w zasi�gu borni: " + collidersInRange.Length);

        Transform target;
        float targetDistance = Mathf.Infinity;

        foreach (Collider collider in collidersInRange)
        {
            //wcyi�gnij transforma od tego collidera

            //najpierw znajdz kapsu��/model ( w�a�ciciela collidera)
            GameObject model = collider.gameObject;
            if (model.transform.parent != null)
            {
                //znajdz rodzica modelu czyli bashera
                GameObject enemy = model.transform.parent.gameObject;

                //sprawdzenie czy jest to przeciwnik
                if (enemy.CompareTag("Enemy"))
                {
                    //Okre�l wektor przesuni�cia je�li to przeciwnik
                    Vector3 diference = player.position - enemy.transform.position;
                    //policz d�ugo�� wektora
                    float distance = diference.magnitude;
                    if (distance < targetDistance)
                    {
                        target = enemy.transform;
                        targetDistance = distance;
                    }
                }
            }
        }
        //TEST
        //Debug.Log("celuje do: " + target.gameObject.name);
        return transform;
    }
}

