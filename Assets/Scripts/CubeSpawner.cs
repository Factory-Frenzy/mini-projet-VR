using System.Collections.Generic;
using UnityEngine;
public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; // Assignez votre prefab de cube rouge ici dans l'inspecteur
    public float distanceFromPlayer = 1f; // Distance pour instancier le cube
    private Camera cam;
    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && cam.enabled) // 1 est pour le clic droit de la souris
        {
            Vector3 spawnPosition = transform.position + transform.forward * distanceFromPlayer;
            Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
