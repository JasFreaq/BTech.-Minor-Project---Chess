using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 20f;
    [SerializeField] private LayerMask _rayLayerMask;

    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _rayLayerMask))
            {
                print(hit.transform.name);
            }
        }
    }
}
