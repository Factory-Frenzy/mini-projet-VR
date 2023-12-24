using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class FireManager : MonoBehaviour
{
    [SerializeField]
    private InputActionReference startFire;
    
    [SerializeField]
    private Rigidbody robotHeadRigidbody;

    [SerializeField]
    private ParticleSystem particulesFeu;
    
    [SerializeField]
    private NavMeshAgent robotNavMeshAgent;
    
    [SerializeField]
    private float headExplosionForce = 1f;
    
    private int _firePower = 2; // Nombre d'appels nécessaire pour éteindre complètement le feu

    
    void Start()
    {
        particulesFeu.Stop();
        startFire.action.Enable();
        startFire.action.performed += OnStartFire; // F pressed
    }
    
    private void OnStartFire(InputAction.CallbackContext obj)
    {
        StartFire();
    }

    private void StartFire()
    {
        // Sauter la tête du robot en ajoutant une force au Rigidbody
        if (robotHeadRigidbody != null)
        {
            Vector3 headExplosionDirection = new Vector3(0.5f, 1.0f, 1.0f); // Ajustez cette valeur selon vos besoins
            //robotHeadRigidbody.AddForce(Vector3.up * headExplosionForce, ForceMode.Impulse);
            robotHeadRigidbody.AddForce(headExplosionDirection.normalized * headExplosionForce, ForceMode.Impulse);
        }

        // Émettre des particules pour représenter le feu
        if (particulesFeu != null)
        {
            particulesFeu.Play();
        }
        
        if (robotNavMeshAgent != null)
        {
            robotNavMeshAgent.enabled = false;
        }
    }

    public void StopFire()
    {
        // Stopper les particules de feu
        if (particulesFeu != null)
        {
            particulesFeu.Stop();
        }
        ActivationSocket();
    }

    private void RepairRobot()
    {
        robotNavMeshAgent.enabled = true;
    }

    public void ReduceFirePower(int power)
    {
        // Réduire la puissance du feu
        _firePower -= power;
        
        var mainModule = particulesFeu.main;
        mainModule.maxParticles /= 2;
        
        // Si la puissance du feu atteint zéro, éteindre complètement le feu
        if (_firePower <= 0)
        {
            _firePower = 0;
            StopFire();
        }
    }

    async private void ActivationSocket()
    {
        await Task.Delay(3000);

        // Activation du socket
        GameObject.Find("AttachTete").GetComponent<BoxCollider>().enabled = true;
    }

    private void DesactivateSocket()
    {
        // Desactivation du socket
        GameObject.Find("AttachTete").GetComponent<BoxCollider>().enabled = false;
    }

    public void OnHeadIsBack()
    {
        RepairRobot();
        DesactivateSocket();
    }
}
