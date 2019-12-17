using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    public Transform Mussle;
    public float LoadingTime;
    public float bulletvelocity;

    public GameObject Bullet;

    float TimeSinceLastShoot;

    private void Update()
    {
        TimeSinceLastShoot += Time.deltaTime;
    }

    public bool CanShoot()
    {
        if (TimeSinceLastShoot > LoadingTime)
        {
            return true;
        }

        return false;
    }

    public bool Shoot()
    {
        if (TimeSinceLastShoot < LoadingTime)
            return false;

        print("SHOOOOOOOT");

        TimeSinceLastShoot = 0f;

        var bullet = Instantiate(Bullet);

        bullet.transform.rotation = Mussle.rotation;
        bullet.transform.position = Mussle.position;

        bullet.GetComponent<Rigidbody>().velocity = Mussle.forward * bulletvelocity;

        return true;
    }
}
