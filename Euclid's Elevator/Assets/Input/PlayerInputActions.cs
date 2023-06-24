//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Input/PlayerInputActions.inputactions
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

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""General"",
            ""id"": ""5ddf8197-b2ad-466f-9441-aaaa5a47d935"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""c1a51c9c-2c20-4472-a329-d8c6685c0382"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""3449320a-19a7-4f46-bfa6-790d238bc143"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Use"",
                    ""type"": ""Button"",
                    ""id"": ""e7bf78a3-187a-4bbe-80f0-bb3a2d5fc84f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""70555ad0-0ba6-4ca3-a70d-e3beab93ac61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sneak"",
                    ""type"": ""Button"",
                    ""id"": ""ded79894-4e2b-4729-a862-55a6eb3e4e2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""562a4ef1-0e50-4e09-bac4-b816702d8db9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Inventory1"",
                    ""type"": ""Button"",
                    ""id"": ""f052d98e-de12-4921-91d4-86894516f957"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory2"",
                    ""type"": ""Button"",
                    ""id"": ""1da5fccd-55b0-4087-9298-2d1815970daa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory3"",
                    ""type"": ""Button"",
                    ""id"": ""59190e49-be0b-4e4e-8a99-f0cda50388b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory4"",
                    ""type"": ""Button"",
                    ""id"": ""2419a776-0363-4671-af28-5ee97ec12b6a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""d66e419d-13f7-47be-bdc9-9a49958ca4be"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9efbdea8-4cc7-418c-8ee4-aa31b6f8d3e6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c2d095f6-ae96-4274-ab0e-a4ee884cdc37"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f79bc49d-c97f-4cb6-9034-c75e694c39af"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1a8bf0b2-1652-44cf-aa24-2bf782cc2453"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c7763cbb-0b49-424b-b29c-ca256a879715"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13afe4d1-1c56-41ce-b279-9a55fa26184a"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""149d1b2b-f1f5-4805-842c-a21648da594b"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0faafe10-5a63-4307-a5db-bb8958fd025f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sneak"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71d4a184-7150-436f-8566-f024214de885"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dab4ff8d-25ba-4540-80e3-39a33ec5ae5d"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5de50814-b83f-4fdf-b1d1-0d1f664448c8"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18cd74d5-1781-452f-b406-9f36563c50b7"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d408516-482d-4c53-b3fd-5e1bc1f955b7"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Realtime"",
            ""id"": ""9533c225-3f1f-4acf-a887-4b07cff54d2d"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""7d8712cf-8981-4bd6-8c83-84cb6de08553"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PageLeft"",
                    ""type"": ""Button"",
                    ""id"": ""3d267a5a-cc1a-45a0-878d-cae4ba22208a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PageRight"",
                    ""type"": ""Button"",
                    ""id"": ""d27b5a9f-7ea1-4116-9bc4-c69b97c06471"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Journal"",
                    ""type"": ""Button"",
                    ""id"": ""952f6354-4333-470f-9d9b-f464b6e939ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5e8c3bd4-9f31-4ecd-984c-b56a32782798"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40ce3872-8614-4b37-ad08-4517526b22c5"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Journal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1ed52d8-4156-49e6-868c-0494ab0b6dc8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PageRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""592ae1ad-a033-44d0-b0ac-669027172ace"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PageRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4a7c3cf-a70b-4610-86ff-b0b9896b66df"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PageLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8c166e8-53df-46b6-8871-011d1ef91c6c"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PageLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // General
        m_General = asset.FindActionMap("General", throwIfNotFound: true);
        m_General_Movement = m_General.FindAction("Movement", throwIfNotFound: true);
        m_General_Interact = m_General.FindAction("Interact", throwIfNotFound: true);
        m_General_Use = m_General.FindAction("Use", throwIfNotFound: true);
        m_General_Drop = m_General.FindAction("Drop", throwIfNotFound: true);
        m_General_Sneak = m_General.FindAction("Sneak", throwIfNotFound: true);
        m_General_Look = m_General.FindAction("Look", throwIfNotFound: true);
        m_General_Inventory1 = m_General.FindAction("Inventory1", throwIfNotFound: true);
        m_General_Inventory2 = m_General.FindAction("Inventory2", throwIfNotFound: true);
        m_General_Inventory3 = m_General.FindAction("Inventory3", throwIfNotFound: true);
        m_General_Inventory4 = m_General.FindAction("Inventory4", throwIfNotFound: true);
        // Realtime
        m_Realtime = asset.FindActionMap("Realtime", throwIfNotFound: true);
        m_Realtime_Pause = m_Realtime.FindAction("Pause", throwIfNotFound: true);
        m_Realtime_PageLeft = m_Realtime.FindAction("PageLeft", throwIfNotFound: true);
        m_Realtime_PageRight = m_Realtime.FindAction("PageRight", throwIfNotFound: true);
        m_Realtime_Journal = m_Realtime.FindAction("Journal", throwIfNotFound: true);
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

    // General
    private readonly InputActionMap m_General;
    private IGeneralActions m_GeneralActionsCallbackInterface;
    private readonly InputAction m_General_Movement;
    private readonly InputAction m_General_Interact;
    private readonly InputAction m_General_Use;
    private readonly InputAction m_General_Drop;
    private readonly InputAction m_General_Sneak;
    private readonly InputAction m_General_Look;
    private readonly InputAction m_General_Inventory1;
    private readonly InputAction m_General_Inventory2;
    private readonly InputAction m_General_Inventory3;
    private readonly InputAction m_General_Inventory4;
    public struct GeneralActions
    {
        private @PlayerInputActions m_Wrapper;
        public GeneralActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_General_Movement;
        public InputAction @Interact => m_Wrapper.m_General_Interact;
        public InputAction @Use => m_Wrapper.m_General_Use;
        public InputAction @Drop => m_Wrapper.m_General_Drop;
        public InputAction @Sneak => m_Wrapper.m_General_Sneak;
        public InputAction @Look => m_Wrapper.m_General_Look;
        public InputAction @Inventory1 => m_Wrapper.m_General_Inventory1;
        public InputAction @Inventory2 => m_Wrapper.m_General_Inventory2;
        public InputAction @Inventory3 => m_Wrapper.m_General_Inventory3;
        public InputAction @Inventory4 => m_Wrapper.m_General_Inventory4;
        public InputActionMap Get() { return m_Wrapper.m_General; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GeneralActions set) { return set.Get(); }
        public void SetCallbacks(IGeneralActions instance)
        {
            if (m_Wrapper.m_GeneralActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnMovement;
                @Interact.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInteract;
                @Use.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnUse;
                @Use.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnUse;
                @Use.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnUse;
                @Drop.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnDrop;
                @Sneak.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnSneak;
                @Sneak.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnSneak;
                @Sneak.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnSneak;
                @Look.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnLook;
                @Inventory1.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory1;
                @Inventory1.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory1;
                @Inventory1.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory1;
                @Inventory2.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory2;
                @Inventory2.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory2;
                @Inventory2.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory2;
                @Inventory3.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory3;
                @Inventory3.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory3;
                @Inventory3.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory3;
                @Inventory4.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory4;
                @Inventory4.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory4;
                @Inventory4.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnInventory4;
            }
            m_Wrapper.m_GeneralActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Use.started += instance.OnUse;
                @Use.performed += instance.OnUse;
                @Use.canceled += instance.OnUse;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @Sneak.started += instance.OnSneak;
                @Sneak.performed += instance.OnSneak;
                @Sneak.canceled += instance.OnSneak;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Inventory1.started += instance.OnInventory1;
                @Inventory1.performed += instance.OnInventory1;
                @Inventory1.canceled += instance.OnInventory1;
                @Inventory2.started += instance.OnInventory2;
                @Inventory2.performed += instance.OnInventory2;
                @Inventory2.canceled += instance.OnInventory2;
                @Inventory3.started += instance.OnInventory3;
                @Inventory3.performed += instance.OnInventory3;
                @Inventory3.canceled += instance.OnInventory3;
                @Inventory4.started += instance.OnInventory4;
                @Inventory4.performed += instance.OnInventory4;
                @Inventory4.canceled += instance.OnInventory4;
            }
        }
    }
    public GeneralActions @General => new GeneralActions(this);

    // Realtime
    private readonly InputActionMap m_Realtime;
    private IRealtimeActions m_RealtimeActionsCallbackInterface;
    private readonly InputAction m_Realtime_Pause;
    private readonly InputAction m_Realtime_PageLeft;
    private readonly InputAction m_Realtime_PageRight;
    private readonly InputAction m_Realtime_Journal;
    public struct RealtimeActions
    {
        private @PlayerInputActions m_Wrapper;
        public RealtimeActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_Realtime_Pause;
        public InputAction @PageLeft => m_Wrapper.m_Realtime_PageLeft;
        public InputAction @PageRight => m_Wrapper.m_Realtime_PageRight;
        public InputAction @Journal => m_Wrapper.m_Realtime_Journal;
        public InputActionMap Get() { return m_Wrapper.m_Realtime; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RealtimeActions set) { return set.Get(); }
        public void SetCallbacks(IRealtimeActions instance)
        {
            if (m_Wrapper.m_RealtimeActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPause;
                @PageLeft.started -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPageLeft;
                @PageLeft.performed -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPageLeft;
                @PageLeft.canceled -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPageLeft;
                @PageRight.started -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPageRight;
                @PageRight.performed -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPageRight;
                @PageRight.canceled -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnPageRight;
                @Journal.started -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnJournal;
                @Journal.performed -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnJournal;
                @Journal.canceled -= m_Wrapper.m_RealtimeActionsCallbackInterface.OnJournal;
            }
            m_Wrapper.m_RealtimeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @PageLeft.started += instance.OnPageLeft;
                @PageLeft.performed += instance.OnPageLeft;
                @PageLeft.canceled += instance.OnPageLeft;
                @PageRight.started += instance.OnPageRight;
                @PageRight.performed += instance.OnPageRight;
                @PageRight.canceled += instance.OnPageRight;
                @Journal.started += instance.OnJournal;
                @Journal.performed += instance.OnJournal;
                @Journal.canceled += instance.OnJournal;
            }
        }
    }
    public RealtimeActions @Realtime => new RealtimeActions(this);
    public interface IGeneralActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnUse(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnSneak(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnInventory1(InputAction.CallbackContext context);
        void OnInventory2(InputAction.CallbackContext context);
        void OnInventory3(InputAction.CallbackContext context);
        void OnInventory4(InputAction.CallbackContext context);
    }
    public interface IRealtimeActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnPageLeft(InputAction.CallbackContext context);
        void OnPageRight(InputAction.CallbackContext context);
        void OnJournal(InputAction.CallbackContext context);
    }
}
