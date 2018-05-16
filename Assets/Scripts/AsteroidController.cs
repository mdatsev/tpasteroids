﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    public float appliedForce = 20f;
    public float angularSpeed = 5f;
    public string tagToDestroyOnCollision = "Player";

    private int stage = 1;
    void Start()
    {
        //The Asteroid Manager will handle all movement of the asteroids
        AsteroidsManager.Instance.RegisterAsteroid(gameObject);
            
        RadomizeDirection(transform.position);
        
    }
    private void Update()
    {
        UpdateShooting();
    }
    private void UpdateShooting()
    {
        //Fire as rapidly as our weapon allows us
        GetComponent<Weapon>().Fire();
    }
    private void OnDestroy()
    {
        AsteroidsManager.Instance.UnregisterAsteroid(gameObject);
        if (stage == 0)
            return;
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            var child = Instantiate(this.gameObject, this.transform.position, Random.rotation);
            child.transform.localScale *= .5f;
            AsteroidsManager.Instance.RegisterAsteroid(child);
            var childCtrlr = child.GetComponent<AsteroidController>();
            childCtrlr.RadomizeDirection(transform.position);
            childCtrlr.stage = stage - 1;
            children.Add(child);
            child.SetActive(true);
        }
    }

    public void RadomizeDirection(Vector3 newPosition)
    {
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));

        Vector3 newDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        newDirection.Normalize();

        //Caching the Rigid Body to avoid the slowness of getting it every time
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(newDirection * appliedForce);
        RandomizeAngularVelocity();
    }
    private void RandomizeAngularVelocity()
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        float rotationInRad = angularSpeed * Mathf.Deg2Rad;
        Vector3 angularVelocity = new Vector3(0f, Random.Range(-rotationInRad, rotationInRad), 0);
        rigidBody.angularVelocity = angularVelocity;
    }
}
