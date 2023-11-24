using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class PlanetPrefabDictionary : SerializableDictionaryBase<string, GameObject> {}

public class PlanetsMainMode : MonoBehaviour
{
    [SerializeField] private PlanetPrefabDictionary planetPrefabs;
    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private TMP_Text planetName;
    [SerializeField] private Toggle infoButton;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private TMP_Text detailsText;

    private Camera camera;
    private int layerMask;
    
    
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        layerMask = 1 << LayerMask.NameToLayer("PlacedObjects");
    }

    // Update is called once per frame
    void Update()
    {
        if (imageManager.trackables.count == 0)
        {
            InteractionConteroller.EnableMode("Scan");
        }
        else
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Planet planet = hit.collider.GetComponentInParent<Planet>();
                planetName.text = planet.planetName;
                infoButton.interactable = true;
                detailsText.text = planet.descriotion;
            }
            else
            {
                planetName.text = "";
                infoButton.interactable = false;
                detailsText.text = "";
            }
        }
    }

    private void OnEnable()
    {
        UIController.ShowUI("Main");
        planetName.text = "";
        infoButton.interactable = false;
        detailsPanel.SetActive(false);
        
        foreach (ARTrackedImage image in imageManager.trackables)
        {
            InstantiatePlanet(image);
        }

        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void InstantiatePlanet(ARTrackedImage image)
    {
        string name = image.referenceImage.name.Split('-')[0];
        if (image.transform.childCount == 0)
        {
            GameObject planet = Instantiate(planetPrefabs[name]);
            planet.transform.SetParent(image.transform, false);
        }
        else
        {
            Debug.Log("${name} already instantiated");
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage newImage in eventArgs.added)
        {
            InstantiatePlanet(newImage);
        }
    }
}
