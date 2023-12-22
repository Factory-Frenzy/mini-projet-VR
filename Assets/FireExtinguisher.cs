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
    
    private float extinguishPower = 5f;

    private bool isUsingExtinguisher = false;

    void Start()
    {
        extinguisherParticles.Stop();
        useExtinguisher.action.Enable();
        useExtinguisher.action.performed += OnUseExtinguisher; // Touche B enfoncée
        useExtinguisher.action.canceled += OnStopUsingExtinguisher; // Touche B relâchée
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
        isUsingExtinguisher = true;
        if (extinguisherParticles != null)
        {
            extinguisherParticles.Play();
        }
    }

    private void OnStopUsingExtinguisher(InputAction.CallbackContext obj)
    {
        isUsingExtinguisher = false;
        if (extinguisherParticles != null)
        {
            extinguisherParticles.Stop();
        }
    }

    private void UseExtinguisher()
    {
        if (isUsingExtinguisher && fireManager != null)
        {
            // Réduire progressivement la puissance du feu dans le script FireManager
            fireManager.ReduceFirePower(extinguishPower);
        }
    }
}
