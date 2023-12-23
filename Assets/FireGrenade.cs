using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireGrenade : MonoBehaviour
{
    [SerializeField]
    public ParticleSystem explosionParticles;

    [SerializeField]
    private FireManager fireManager;
    
    [SerializeField]
    private XRGrabInteractable grabInteractable;

    protected bool IsActive;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.AddListener(OnGrabbed);
        }
        else
        {
            Debug.LogError("XR Grab Interactable component not found on the grenade.");
        }
    }
    
    private void OnGrabbed(XRBaseInteractor interactor)
    {
        IsActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsActive) return;
        Explode();
    }

    private void Explode()
    {
        // Émettre des particules d'explosion
        if (explosionParticles != null)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
        }

        // Vérifier si la grenade a touché la zone de l'incendie (vous devrez ajuster cela en fonction de votre scène)
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);
        foreach (Collider col in colliders)
        {
            if (col.name == "Fire")
            {
                fireManager.StopFire();
                break;
            }
        }

        // Détruire la grenade
       Destroy(gameObject);
    }
}
