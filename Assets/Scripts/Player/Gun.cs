using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Mask")]
    [SerializeField] private LayerMask Mask;

    [Header("Shoot")]
    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private float BulletSpeed = 100;
    [SerializeField] private float dmg;
    [SerializeField] private Transform BulletSpawnPoint;


    [Header("Particle")]
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private ParticleSystem ShootingSystem;

    [Header("Ammo")]
    [SerializeField] private int ammo;
    [SerializeField] private int maxAmmo = 50;
    [SerializeField] private Text ammoTxt;

    private Wallet wallet;

    private float LastShootTime;

    private void Awake()
    {
        wallet = GetComponent<Wallet>();
        ammo = maxAmmo;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1")) Shoot();
        ammoTxt.text = "" + ammo;
    }

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time && ammo >= 1)
        {
            ShootingSystem.Play();
            Vector3 direction = GetDirection();
            RaycastHit hit;

            if (Physics.Raycast(BulletSpawnPoint.position, BulletSpawnPoint.forward, out hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                if(hit.transform.gameObject.GetComponent<HealthSystem>()) hit.transform.gameObject.GetComponent<HealthSystem>().TakeDamage(dmg);

                if(hit.transform.tag == "Enemy")
                    wallet.AddCredits(2);

                LastShootTime = Time.time;
            }
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, transform.forward * 100, Vector3.zero, false));

                LastShootTime = Time.time;
            }

            ammo--;

        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }

        Trail.transform.position = HitPoint;

        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AMMO")
        {
            ammo += maxAmmo;
            Destroy(other.gameObject);
        }
    }

    public void AddPerk()
    {
        dmg *= 1.5f;
    }
}
