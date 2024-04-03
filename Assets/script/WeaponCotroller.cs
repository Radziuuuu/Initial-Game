using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //zasi璕 broni
    public float range = 10f;

    //transform gracza
    Transform player;

    //prefab pocisku
    public GameObject projectilePrefab;

    //spawn pocisku
    Transform projectileSpawn;

    //czestotliwosc strzalu (/sek)
    public float rateOfFire = 1;
    //czas od ostatniego wystrzalu
    float timeSinceLastFire = 0;

    //moc wystrza逝 (pr璠ko pocz靖kowa)
    public float projectileForce = 20;

    // Start is called before the first frame update
    void Start()
    {
        // pozycja gracza
        player = GameObject.FindWithTag("Player").transform;

        //znajdz w hierarchii obieku miejsce z ktorego staruje pocisk
        projectileSpawn = transform.Find("ProjectileSpawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Transform target = TagTargeter("Enemy");
        if (target != transform)
        {
            //Debug.Log("Celuje do: " + target.gameObject.name);
            transform.LookAt(target.position + Vector3.up);

            //wystrzel pocisk
            //jei min窸o wi璚ej od ostatniego strza逝 ni� wskazuje na to pr璠ko strzelania
            if (timeSinceLastFire > rateOfFire)
            {
                //stworz pocisk
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);


                //znajdz rrigidbody dla pocisku
                Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
                //"popchnij" pocisk do przodu
                //sila dziala w kierunku przodu dzia豉 (pojectilespawn.z) * si豉 wystrza逝
                projectileRB.AddForce(projectileSpawn.transform.forward * projectileForce, ForceMode.VelocityChange);

                //je瞠li strzelisz to wyzeruj czas 
                timeSinceLastFire = 0;

                //zniszcz pocisk po 5 sekundach
                Destroy(projectile, 5);
            }
            else
            {
                timeSinceLastFire += Time.deltaTime;
            }
        }

    }
    Transform TagTargeter(string tag)
    {
        //tablica wszystkich obiekt闚 pasuj鉍ych do taga podanego jako agument
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        //szukamy najbli窺zego
        Transform closestTarget = transform;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            //wektor przesuni璚ia wzgl璠em gracza
            Vector3 difference = target.transform.position - player.position;
            //odleg這 od gracza
            float distance = difference.magnitude;

            if (distance < closestDistance && distance < range)
            {
                closestTarget = target.transform;
                closestDistance = distance;
            }
        }
        return closestTarget;
    }

    Transform LegeacyTargeter()
    {
        //znajdz wszystkie colidery w promieniu = range i zapisz je do tablicy collidersInRange
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        //do cel闚 testowych 
        //Debug.Log("Ilo collider闚 w zasi璕u broni: " +  collidersInRange.Length);

        //szukamy najbli窺zego przeciwnika

        Transform target = transform;
        float targetDistance = Mathf.Infinity;

        foreach (Collider collider in collidersInRange)
        {
            //wyci鉚nij transforma od tego coldiera

            //najpierw znajdz kapsu貫/model (w豉iciela colidera)
            GameObject model = collider.gameObject;

            if (model.transform.parent != null)
            {
                //znajdz rodzica modelu czyli przeciwnika
                GameObject enemy = model.transform.parent.gameObject;

                //sprawdz czy to co znalaz貫� jest przeciwnikiem
                if (enemy.CompareTag("Enemy"))
                {
                    //jei to przeciwnik to okre wektor przesuni璚ia
                    Vector3 diference = player.position - enemy.transform.position;
                    //policz d逝go wektora (odleg這)
                    float distance = diference.magnitude;
                    if (distance < targetDistance)
                    {
                        //znaleziono nowy cel bli瞠j
                        target = enemy.transform;
                        targetDistance = distance;
                    }
                }
            }


        }

        //do cel闚 testowych
        //Debug.Log("Celuje do: " + target.gameObject.name);

        return target;
    }
}