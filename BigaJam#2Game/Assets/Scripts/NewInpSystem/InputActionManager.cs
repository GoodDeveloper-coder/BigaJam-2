using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActionAsset;
    private void Awake() {
        _inputActionAsset.Enable();
    }
    private void OnDestroy() {
        _inputActionAsset.Disable();     
    }
}
