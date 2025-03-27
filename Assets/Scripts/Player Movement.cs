using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerscript : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform firePoint; // The point where bullets are spawned

    // Start is called before the first frame update
    void Start()
    {
        // Optional initialization logic
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        // Get horizontal input and move the player
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(move, 0, 0);

        // Restrict the player to screen bounds
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -screenWidth, screenWidth),
            transform.position.y,
            transform.position.z
        );
    }

    void HandleShooting()
    {
        // Shoot a bullet when the SpaceBar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the firePoint position
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
}
