using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPlacementSystem : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float maxSlopeAngle = 5f;
    [SerializeField] private bool placementModeEnabled;

    [SerializeField] private DeviceCatalog prefabCatalog;
    [SerializeField] private Vector3 previewPrefabsIdlePos;
    [SerializeField] private int selectedPrefabIndex; //For dev purposes
    [SerializeField] private ConnectionsManager connectionsManager;
    
    private List<Transform> _previewObjects = new List<Transform>();
    private Camera _camera;

    private bool _validPos;
    private Vector3 _currentPreviewPos;
    
    //TODO Add material preview handler

    private void Start()
    {
        _camera = Camera.main;
        InstantiatePreviewObjects();
    }

    private void Update()
    {
        if (placementModeEnabled)
        {
            ShowObjectPreview();
            
            if (Input.GetMouseButtonDown(0))
                PlaceObject();

            if (Input.GetKeyDown(KeyCode.Q))
                SwitchIndex(0);

            if (Input.GetKeyDown(KeyCode.E))
                SwitchIndex(1);
        }
    }

    private void ShowObjectPreview()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, math.INFINITY))
        {

            if (((1 << hit.collider.gameObject.layer) & placementLayer) != 0)
            {
                float surfaceAngle = Vector3.Angle(hit.normal, Vector3.up);

                if (surfaceAngle > maxSlopeAngle)
                {
                    _validPos = false;
                    return;
                }
            
                _previewObjects[selectedPrefabIndex].position = hit.point;
                _validPos = true;
                _currentPreviewPos = hit.point;
            }
            else
            {
                _validPos = false;
            }
        }
    }

    private void InstantiatePreviewObjects()
    {
        if (prefabCatalog.allAvailableDevices.Count > 0)
        {
            foreach (var prefab in prefabCatalog.allAvailableDevices)
            {
                GameObject previewPrefab =  Instantiate(prefab.previewPrefab);
                previewPrefab.transform.position = previewPrefabsIdlePos;
                _previewObjects.Add(previewPrefab.transform);
            }
        }
    }

    private void PlaceObject()
    {
        if (_validPos)
        {
            GameObject placedObject = Instantiate(prefabCatalog.allAvailableDevices[selectedPrefabIndex].devicePrefab, _currentPreviewPos,  Quaternion.identity);
            connectionsManager.LinkNewDevice(placedObject, prefabCatalog.allAvailableDevices[selectedPrefabIndex].deviceType);
        }
    }

    private void SwitchIndex(int newIndex)
    {
        _previewObjects[selectedPrefabIndex].position = previewPrefabsIdlePos;
        selectedPrefabIndex = newIndex;
    }

    private void DisablePlacement()
    {
        foreach (var previewObject in _previewObjects)
        {
           previewObject.position = previewPrefabsIdlePos;
        }
    }
}
