using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Vector2 speed = new Vector2(5, 5);

    public Transform[] firePoints = new Transform[5];
    public GameObject playerbulletPrefab;

    public float bulletSpeed = 15f;
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;

    public GameObject EntityRecoil;
    public float RecoilForce = 0.75f;

    public static float health;

    int i = 0;
    public float boost;

    void Start()
    {

        health = 3.0f;

        /*
        spr = DamageSprite.GetComponent<SpriteRenderer>();
        matRed = Resources.Load("RedFlash", typeof(Material)) as Material;
        matDefault = spr.material;
        */
    }

    void Update()
    {
        //Movement
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        movement *= Time.deltaTime;

        transform.Translate(movement, Space.World);

        //Follow Mouse
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        /*
        Vector3 mouse = Input.mousePosition;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, transform.position.z));
        transform.right = mouseWorld - transform.position;
        */

        //Shoot
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        foreach (Transform firePoint in firePoints)
        {
            var randomNumberZ = Random.Range(-20.0f, 20.0f);
            GameObject Player_Bullet = Instantiate(playerbulletPrefab, firePoint.position, firePoint.rotation);
            Player_Bullet.transform.Rotate(0, 0, randomNumberZ);
            Rigidbody2D rb = Player_Bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
            Rigidbody2D rbp = EntityRecoil.GetComponent<Rigidbody2D>();
            if(i==2 || i==3)
                rbp.AddForce(-firePoint.up * RecoilForce * boost, ForceMode2D.Impulse);
            else
                rbp.AddForce(-firePoint.up * RecoilForce, ForceMode2D.Impulse);
            if (i >= 4)
                i = 0;
            else
                i++;
        }
    }

    /*
    public void Damage(float damage)
    {
        spr.material = matRed;
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else Invoke("ResetMaterial", .075f);
    }
    

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Level1");
    }

    
    void ResetMaterial()
    {
        spr.material = matDefault;
    }
    */
}
