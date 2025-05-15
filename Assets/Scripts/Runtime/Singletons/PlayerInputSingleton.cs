using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Singletons
{
    public class PlayerInputSingleton : MonoBehaviour
    {
        #region Singleton

        public static PlayerInputSingleton Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        

        #endregion

        #region Variables

        [FoldoutGroup("SerializedField Variables"), SerializeField] private InputSystem_Actions _inputSystemActions; 

        #endregion

        public InputSystem_Actions Controls()
        {
            return _inputSystemActions;
        }

        private void OnEnable()
        {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Enable();
        }

        private void OnDisable()
        {
            _inputSystemActions.Disable();
        }
    }
}