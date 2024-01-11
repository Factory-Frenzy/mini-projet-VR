using UnityEngine;
using UnityEngine.InputSystem;

public class FireExtinguisher : MonoBehaviour
{
    [SerializeField]
    private InputActionReference useExtinguisher;

    [SerializeField]
    private ParticleSystem extinguisherParticles;

    [SerializeField]
    private FireManager fireManager;
    
    [SerializeField]
    private float extinguisherRadius = 1f; // Rayon de la sphère de détection des particules
    
    [SerializeField]
    private int extinguishPower = 1;

    private bool _isUsingExtinguisher;

    void Start()
    {
        extinguisherParticles.Stop();
        useExtinguisher.action.Enable();
        useExtinguisher.action.performed += OnUseExtinguisher;
        useExtinguisher.action.canceled += OnStopUsingExtinguisher;
        
        Collider collider = gameObject.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(UseExtinguisher), 0f, 1f); // Appeler UseExtinguisher chaque seconde
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UseExtinguisher)); // Arrêter l'appel de UseExtinguisher lorsqu'il est désactivé
    }
    
    
    private void OnUseExtinguisher(InputAction.CallbackContext obj)
    {
        _isUsingExtinguisher = true;
    }

    private void OnStopUsingExtinguisher(InputAction.CallbackContext obj)
    {
        _isUsingExtinguisher = false;
    }

    private void UseExtinguisher()
    {
        if (_isUsingExtinguisher && fireManager != null)
        {
            // Détection des particules autour de l'extincteur
            Collider[] colliders = Physics.OverlapSphere(transform.position, extinguisherRadius);
            
            foreach (var collider1 in colliders)
            {
                if (collider1.name == "Fire")
                {
                    // Réduire progressivement la puissance du feu dans le script FireManager
                    fireManager.ReduceFirePower(extinguishPower);
                    break;
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le joueur entre en collision avec l'extincteur
        if (other.CompareTag("Player"))
        {
            // Active l'utilisation de l'extincteur uniquement si la touche est enfoncée et le joueur est en collision avec l'extincteur
            if (_isUsingExtinguisher && extinguisherParticles != null)
            {
                extinguisherParticles.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Désactive l'utilisation de l'extincteur lorsque le joueur quitte la collision avec l'extincteur
        if (other.CompareTag("Player"))
        {
            _isUsingExtinguisher = false;
            if (extinguisherParticles != null)
            {
                extinguisherParticles.Stop();
            }
        }
    }
}
