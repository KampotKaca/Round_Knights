using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/Settings/InputReader", fileName = "InputReader")]
    public class InputReader : ScriptableObject, Input_Map.IGameplayActions, Input_Map.IUIActions
    {
        Input_Map m_InputMap;

        void OnEnable()
        {
            if (m_InputMap == null)
            {
                m_InputMap = new();
                m_InputMap.Gameplay.SetCallbacks(this);
                m_InputMap.UI.SetCallbacks(this);
                
                EnableGameplay();
            }
        }

        public void EnableGameplay()
        {
            m_InputMap.UI.Disable();
            m_InputMap.Gameplay.Enable();
        }
        
        public void EnableUI()
        {
            m_InputMap.UI.Enable();
            m_InputMap.Gameplay.Disable();
        }

        public void ResetEvents()
        {
            On_CameraMove = null;
            On_CameraRotate = null;
            On_CameraZoom = null;
            On_GameBack = null;
            On_UIBack = null;
        }

        #region Gameplay

        public event Action<Vector2> On_CameraMove; 
        public event Action<Vector2> On_CameraRotate;
        public event Action<float> On_CameraZoom;
        
        public event Action On_GameBack;
        
        public void OnCamera_Move(InputAction.CallbackContext context)
        {
            On_CameraMove?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnCamera_Zoom(InputAction.CallbackContext context)
        {
            On_CameraZoom?.Invoke(context.ReadValue<Vector2>().y);
        }

        public void OnCamera_Rotate(InputAction.CallbackContext context)
        {
            On_CameraRotate?.Invoke(context.ReadValue<Vector2>());
        }

        void Input_Map.IGameplayActions.OnGo_Back(InputAction.CallbackContext context)
        {
            On_GameBack?.Invoke();
            EnableUI();
        }
        #endregion

        #region UI
        public event Action On_UIBack;
        void Input_Map.IUIActions.OnGo_Back(InputAction.CallbackContext context)
        {
            On_UIBack?.Invoke();
            EnableGameplay();
        }
        #endregion
    }
}