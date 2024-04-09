using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //zasięg broni
    public float range = 25f;

    //transform gracza
    Transform player;

    //prefab pocisku
    public GameObject projectilePrefab;

    public GameObject projectileSpawnGO;

    //spawn pocisku
    Transform projectileSpawn;

    //czestotliwosc strzalu (/sek)
    public float rateOfFire = 1;

    //czas od ostatniego wystrzalu
    float timeSinceLastFire = 0;

    //moc wystrzał€ (prędkość początkowa)
    public float projectileForce = 20;

    // Start is called before the first frame update
    void Start()
    {
        // pozycja gracza
        player = GameObject.FindWithTag("Player").transform;

        //znajdz w hierarchii obieku miejsce z ktorego staruje pocisk
        projectileSpawn = projectileSpawnGO.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform target = TagTargeter("Enemy");
        if (target != transform)
        {
            //Debug.Log("Celuje do: " + target.gameObject.name);
            transform.LookAt(target.position + Vector3.up);
            
            //jeśli minęło więcej od ostatniego strzału niż wskazuje na to prędkość srzelania
            if (timeSinceLastFire > rateOfFire)
            {
                //stworz pocisk i dodaj dźwięk
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
                

                //znajdz rigidbody dla pocisku
                Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
                //"popchnij" pocisk do przodu
                //sila dziala w kierunku przodu działa (pojectilespawn.z) * siła wystrzału
                projectileRB.AddForce(projectileSpawn.transform.forward * projectileForce, ForceMode.VelocityChange);

                //jeżeli strzelisz to wyzeruj czas 
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
        //tablica wszystkich obiektów pasujących do taga podanego jako agument
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        //szukamy najbli窺zego
        Transform closestTarget = transform;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            //wektor przesunięcia względem gracza
            Vector3 difference = target.transform.position - player.position;
            //odległość od gracza
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

        //do celów testowych 
        //Debug.Log("Ilość colliderów w zasięgu broni: " +  collidersInRange.Length);

        //szukamy najbliższego przeciwnika

        Transform target = transform;
        float targetDistance = Mathf.Infinity;

        foreach (Collider collider in collidersInRange)
        {
            //wyciągnij transforma od tego coldiera

            //najpierw znajdz kapsułe/model (właściciela colidera)
            GameObject model = collider.gameObject;

            if (model.transform.parent != null)
            {
                //znajdz rodzica modelu czyli przeciwnika
                GameObject enemy = model.transform.parent.gameObject;

                //sprawdz czy to co znalazłeś jest przeciwnikiem
                if (enemy.CompareTag("Enemy"))
                {
                    //jeśi to przeciwnik to określ wektor przesunięcia
                    Vector3 diference = player.position - enemy.transform.position;
                    //policz długość wektora (odległość)
                    float distance = diference.magnitude;
                    if (distance < targetDistance)
                    {
                        //znaleziono nowy cel bliżej
                        target = enemy.transform;
                        targetDistance = distance;
                    }
                }
            }


        }

        //do celów testowych
        //Debug.Log("Celuje do: " + target.gameObject.name);

        return target;
    }
}