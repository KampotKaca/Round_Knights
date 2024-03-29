//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_Assets/Settings/Input/Input_Map.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Input_Map: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Input_Map()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input_Map"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""97156030-2ce6-492e-910a-f9e3afeb1649"",
            ""actions"": [
                {
                    ""name"": ""Camera_Move"",
                    ""type"": ""Value"",
                    ""id"": ""22081c46-74da-499a-bae1-acf6ad248655"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Camera_Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""1ffc65d2-f167-4058-a6dc-c127cafbd37a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Camera_Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""efc33266-ffed-4a93-b1c7-306dfdd7b180"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Go_Back"",
                    ""type"": ""Button"",
                    ""id"": ""b29cdd63-2b5e-40b4-ae49-f34c10bdb82c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a6972501-08f9-4c6b-9e48-76441b7f0be4"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Pitch_Yaw_Axis"",
                    ""id"": ""970945d9-97c7-4b0c-8236-e80b3172ff77"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""109c997a-b330-4d0c-9682-5fcaeab77267"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6eb0d571-ebee-4884-96aa-60906d98277c"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""868aacfe-d9af-49e1-90c9-30be480517d0"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8bc5801d-ca41-4135-a799-d918b261c32d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD_Axis"",
                    ""id"": ""40ece6c5-c75c-4a27-8202-1813987529ae"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""211074cd-fea7-4437-a8ee-6604f75b8731"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""27d4b730-f72f-4d64-8b6d-cf788592e1a1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""393e88f5-92c2-4e62-9dfb-9f9c12cdc885"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""086048eb-5d52-45f3-a6ec-b2488d9e9e74"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow_Axis"",
                    ""id"": ""7060a964-bfb2-405b-98d9-820ce45c61ca"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""94c523d8-c462-48b2-8319-76fc1c4dae96"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e8eec2f5-d618-4c1f-a375-66b1a9ab99c5"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a4ec14f4-191b-4a6a-aaeb-096ec2bcc923"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""da9ec06c-7222-474a-8ee6-67c1eb9dd193"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera_Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""48da4b26-2f44-4087-8cf0-e3c7f183afb8"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Go_Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""815e24b5-5a35-4249-bce8-6a645d486857"",
            ""actions"": [
                {
                    ""name"": ""Go_Back"",
                    ""type"": ""Button"",
                    ""id"": ""922c07b2-79e8-493a-ac45-5a20aef94f7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""eda67749-b7de-4be1-b12f-25cd27bd54a2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Go_Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Camera_Move = m_Gameplay.FindAction("Camera_Move", throwIfNotFound: true);
        m_Gameplay_Camera_Zoom = m_Gameplay.FindAction("Camera_Zoom", throwIfNotFound: true);
        m_Gameplay_Camera_Rotate = m_Gameplay.FindAction("Camera_Rotate", throwIfNotFound: true);
        m_Gameplay_Go_Back = m_Gameplay.FindAction("Go_Back", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Go_Back = m_UI.FindAction("Go_Back", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_Camera_Move;
    private readonly InputAction m_Gameplay_Camera_Zoom;
    private readonly InputAction m_Gameplay_Camera_Rotate;
    private readonly InputAction m_Gameplay_Go_Back;
    public struct GameplayActions
    {
        private @Input_Map m_Wrapper;
        public GameplayActions(@Input_Map wrapper) { m_Wrapper = wrapper; }
        public InputAction @Camera_Move => m_Wrapper.m_Gameplay_Camera_Move;
        public InputAction @Camera_Zoom => m_Wrapper.m_Gameplay_Camera_Zoom;
        public InputAction @Camera_Rotate => m_Wrapper.m_Gameplay_Camera_Rotate;
        public InputAction @Go_Back => m_Wrapper.m_Gameplay_Go_Back;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @Camera_Move.started += instance.OnCamera_Move;
            @Camera_Move.performed += instance.OnCamera_Move;
            @Camera_Move.canceled += instance.OnCamera_Move;
            @Camera_Zoom.started += instance.OnCamera_Zoom;
            @Camera_Zoom.performed += instance.OnCamera_Zoom;
            @Camera_Zoom.canceled += instance.OnCamera_Zoom;
            @Camera_Rotate.started += instance.OnCamera_Rotate;
            @Camera_Rotate.performed += instance.OnCamera_Rotate;
            @Camera_Rotate.canceled += instance.OnCamera_Rotate;
            @Go_Back.started += instance.OnGo_Back;
            @Go_Back.performed += instance.OnGo_Back;
            @Go_Back.canceled += instance.OnGo_Back;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @Camera_Move.started -= instance.OnCamera_Move;
            @Camera_Move.performed -= instance.OnCamera_Move;
            @Camera_Move.canceled -= instance.OnCamera_Move;
            @Camera_Zoom.started -= instance.OnCamera_Zoom;
            @Camera_Zoom.performed -= instance.OnCamera_Zoom;
            @Camera_Zoom.canceled -= instance.OnCamera_Zoom;
            @Camera_Rotate.started -= instance.OnCamera_Rotate;
            @Camera_Rotate.performed -= instance.OnCamera_Rotate;
            @Camera_Rotate.canceled -= instance.OnCamera_Rotate;
            @Go_Back.started -= instance.OnGo_Back;
            @Go_Back.performed -= instance.OnGo_Back;
            @Go_Back.canceled -= instance.OnGo_Back;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_Go_Back;
    public struct UIActions
    {
        private @Input_Map m_Wrapper;
        public UIActions(@Input_Map wrapper) { m_Wrapper = wrapper; }
        public InputAction @Go_Back => m_Wrapper.m_UI_Go_Back;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @Go_Back.started += instance.OnGo_Back;
            @Go_Back.performed += instance.OnGo_Back;
            @Go_Back.canceled += instance.OnGo_Back;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @Go_Back.started -= instance.OnGo_Back;
            @Go_Back.performed -= instance.OnGo_Back;
            @Go_Back.canceled -= instance.OnGo_Back;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IGameplayActions
    {
        void OnCamera_Move(InputAction.CallbackContext context);
        void OnCamera_Zoom(InputAction.CallbackContext context);
        void OnCamera_Rotate(InputAction.CallbackContext context);
        void OnGo_Back(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnGo_Back(InputAction.CallbackContext context);
    }
}
