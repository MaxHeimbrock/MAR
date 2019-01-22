using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class HandTracking : MonoBehaviour {

    private GameObject indicator = null;
    private TextMesh textMesh = null;

    private Manager manager;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

        if (manager != null)
            Debug.Log("manager found with tag by HandTracking");

        else
            Debug.Log("manager not found with tag by HandTracking");
        
        CreateIndicator();
        CreateText();

        InteractionManager.InteractionSourceLost += InteractionManager_SourceLost;
        InteractionManager.InteractionSourceDetected += InteractionManager_SourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_SourceUpdated;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void InteractionManager_SourceDetected(InteractionSourceDetectedEventArgs arg)
    {
        if (arg.state.source.kind == InteractionSourceKind.Hand)
        {
            ShowObjects(true);
        }
    }

    private void InteractionManager_SourceLost(InteractionSourceLostEventArgs arg)
    {
        if (arg.state.source.kind == InteractionSourceKind.Hand)
        {
            ShowObjects(false);

            manager.SetHandPos(new Vector3(0, 0, 0));
        }
    }

    private void InteractionManager_SourceUpdated(InteractionSourceUpdatedEventArgs arg)
    {
        if (arg.state.source.kind == InteractionSourceKind.Hand)
        {

            Vector3 handPosition;
            Vector3 handVelocity;

            arg.state.sourcePose.TryGetPosition(out handPosition);
            arg.state.sourcePose.TryGetVelocity(out handVelocity);

            manager.SetHandPos(handPosition);

            UpdateText(handPosition, handVelocity);
            UpdateIndicator(handPosition);
        }
    }

    private void CreateIndicator()
    {
        if (indicator == null)
        {
            indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            indicator.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        }
    }

    private void UpdateIndicator(Vector3 position)
    {
        if (indicator != null)
        {
            indicator.transform.position = position;
        }
    }

    private void CreateText()
    {
        GameObject text = new GameObject();
        textMesh = text.AddComponent<TextMesh>();
        text.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    private void UpdateText(Vector3 position, Vector3 velocity)
    {
        if (textMesh != null)
        {
            position = new Vector3(position.x, position.y + 0.1f, position.z);

            textMesh.gameObject.transform.position = position;
            var gazeDirection = Camera.main.transform.forward;
            textMesh.gameObject.transform.rotation = Quaternion.LookRotation(gazeDirection);
            textMesh.text = string.Format("Position:{0:0.00},{1:0.00},{2:0.00}\n Velocity: {3:0.00},{4:0.00},{5:0.00}", position.x, position.y, position.z, velocity.x, velocity.y, velocity.z);
        }
    }

    public void ShowObjects(bool show)
    {
        if (indicator != null && textMesh != null)
        {
            indicator.SetActive(show);
            textMesh.gameObject.SetActive(show);
        }
    }
}
