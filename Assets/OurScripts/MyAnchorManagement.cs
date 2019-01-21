using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;

public class MyAnchorManagement : MonoBehaviour {

    WorldAnchorStore store;
    WorldAnchor anchor;
    public GameObject theGameObjectIWantAnchored;

    // Use this for initialization
    void Start () {
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);
    }    
	
	// Update is called once per frame
	void Update () {
		
	}

    private void AnchorStoreLoaded(WorldAnchorStore store)
    {
        this.store = store;
        LoadAnchors();
    }

    private void LoadAnchors()
    {
        bool retTrue = this.store.Load("theGameObjectIWantAnchored", theGameObjectIWantAnchored);
        if (!retTrue)
        {
            // Until the gameObjectIWantAnchored has an anchor saved at least once it will not be in the AnchorStore
        }
    }

    private void SaveAnchor()
    {
        bool retTrue;
        anchor = theGameObjectIWantAnchored.AddComponent<WorldAnchor>();
        // Remove any previous worldanchor saved with the same name so we can save new one
        this.store.Delete(theGameObjectIWantAnchored.name.ToString());
        retTrue = this.store.Save(theGameObjectIWantAnchored.name.ToString(), anchor);
        if (!retTrue)
        {
            Debug.Log("Anchor save failed.");
        }
    }

    private void ClearAnchor()
    {
        anchor = theGameObjectIWantAnchored.GetComponent<WorldAnchor>();
        if (anchor)
        {
            // remove any world anchor component from the game object so that it can be moved
            DestroyImmediate(anchor);
        }
    }
}
