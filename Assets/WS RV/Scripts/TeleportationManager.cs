using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{

    [SerializeField]
    private TeleportationProvider provider;

    [SerializeField]
    private InputActionReference activate;

    [SerializeField]
    private InputActionReference cancel;

    [SerializeField]
    private XRRayInteractor teleportRay;

    private bool isActive;

    XRInteractorLineVisual lineVisual;


    // Start is called before the first frame update
    void Start()
    {
        teleportRay.enabled = false;

        activate.action.Enable();
        activate.action.performed += OnTeleportActivate; // thumbstick pressed
        activate.action.canceled += OnTeleportRequested; // = z released

        cancel.action.Enable();
        cancel.action.performed += OnTeleportCancel;

        try
        {
            lineVisual = teleportRay.GetComponent<XRInteractorLineVisual>();
        }
        catch (Exception e)
        {
            Debug.LogError("Error : " + e.Message);
        }
    }


    private void OnTeleportRequested(InputAction.CallbackContext context)
    {
        print("OnTeleportRequested");
        if (!isActive)
            return;

        RaycastHit hit;
        
        if (!teleportRay.TryGetCurrent3DRaycastHit(out hit))
        {
            teleportRay.enabled = false;
            isActive = false;
            return;
        }

        var interactable = hit.collider.GetComponentInParent<BaseTeleportationInteractable>();
        var t = interactable.GetAttachTransform(teleportRay);

        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point,
        };

        if (interactable is TeleportationAnchor)
        {
            request = new TeleportRequest()
            {
                destinationPosition = t.position,
                destinationRotation = t.rotation,
                matchOrientation = interactable.matchOrientation
            };
        }

        provider.QueueTeleportRequest(request);
        setActiveTeleport(false);
    }

    private void setActiveTeleport(bool active)
    {
        teleportRay.enabled = lineVisual.enabled = isActive = active;
    }

    private void OnTeleportActivate(InputAction.CallbackContext ctx)
    {
        print("OnTeleportActivate");
        setActiveTeleport(true);
    }

    private void OnTeleportCancel(InputAction.CallbackContext ctx)
    {
        print("OnTeleportCancel");
        setActiveTeleport(false);
    }
}