//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/01. Scripts/phjh/System/InputSystem/Controls.inputactions
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

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""798d1370-70e2-4b07-805c-b7cc0c8cef07"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""4c36ff63-460f-40e6-a5a9-1c60e1147fb3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""84bf0026-80c8-4818-95b6-afd65e2d700f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""a21aee4b-641f-42ac-96da-0dba8c542454"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BaseAttack"",
                    ""type"": ""Button"",
                    ""id"": ""c3ad526c-6009-493a-afec-75323bf2263b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChargeAttack"",
                    ""type"": ""Button"",
                    ""id"": ""d4cc4a1f-6a05-4836-8fb6-ef0a2339b231"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SKill"",
                    ""type"": ""Button"",
                    ""id"": ""390705ef-137e-4678-aedc-9f52add6ec39"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""e7d714e4-9048-4d6d-b7db-ae8af5a093b9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3952214e-31fa-40b4-b735-25d715ab3e1e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6da63520-3dec-42ea-ac15-39e6283f731a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""72a42d68-e159-427f-9fbf-53e52bc89441"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""19d36031-63a6-42f3-97cb-0cb51f82920e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bf9c20ad-e70b-4b19-b048-585cefeb6670"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d2d39bd-b96e-4cf2-a60d-23bd4ca96b3d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c690b8e-74cc-4182-be88-e7dac03ff32d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BaseAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1aa26b2d-d970-4ae3-aaa0-cddddb2eba08"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChargeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49fd4740-e6bf-482a-b386-2ac1b0527b0f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SKill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GameSystem"",
            ""id"": ""cca4947d-a5e3-4103-8d2d-c4fa8dbfde9e"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f6a53eb7-a7b1-457f-96c3-4141a23d2365"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""bad66c1d-0b08-41f8-8ca2-70cbb0d943f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a54bc4e5-d860-4d8b-8e85-7a81b3195ff1"",
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
                    ""id"": ""2c8a4dba-b7c7-49a9-a74a-081f81de8661"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyMouse"",
            ""bindingGroup"": ""KeyMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_Dodge = m_Player.FindAction("Dodge", throwIfNotFound: true);
        m_Player_BaseAttack = m_Player.FindAction("BaseAttack", throwIfNotFound: true);
        m_Player_ChargeAttack = m_Player.FindAction("ChargeAttack", throwIfNotFound: true);
        m_Player_SKill = m_Player.FindAction("SKill", throwIfNotFound: true);
        // GameSystem
        m_GameSystem = asset.FindActionMap("GameSystem", throwIfNotFound: true);
        m_GameSystem_Pause = m_GameSystem.FindAction("Pause", throwIfNotFound: true);
        m_GameSystem_Inventory = m_GameSystem.FindAction("Inventory", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_Dodge;
    private readonly InputAction m_Player_BaseAttack;
    private readonly InputAction m_Player_ChargeAttack;
    private readonly InputAction m_Player_SKill;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @Dodge => m_Wrapper.m_Player_Dodge;
        public InputAction @BaseAttack => m_Wrapper.m_Player_BaseAttack;
        public InputAction @ChargeAttack => m_Wrapper.m_Player_ChargeAttack;
        public InputAction @SKill => m_Wrapper.m_Player_SKill;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
            @Dodge.started += instance.OnDodge;
            @Dodge.performed += instance.OnDodge;
            @Dodge.canceled += instance.OnDodge;
            @BaseAttack.started += instance.OnBaseAttack;
            @BaseAttack.performed += instance.OnBaseAttack;
            @BaseAttack.canceled += instance.OnBaseAttack;
            @ChargeAttack.started += instance.OnChargeAttack;
            @ChargeAttack.performed += instance.OnChargeAttack;
            @ChargeAttack.canceled += instance.OnChargeAttack;
            @SKill.started += instance.OnSKill;
            @SKill.performed += instance.OnSKill;
            @SKill.canceled += instance.OnSKill;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
            @Dodge.started -= instance.OnDodge;
            @Dodge.performed -= instance.OnDodge;
            @Dodge.canceled -= instance.OnDodge;
            @BaseAttack.started -= instance.OnBaseAttack;
            @BaseAttack.performed -= instance.OnBaseAttack;
            @BaseAttack.canceled -= instance.OnBaseAttack;
            @ChargeAttack.started -= instance.OnChargeAttack;
            @ChargeAttack.performed -= instance.OnChargeAttack;
            @ChargeAttack.canceled -= instance.OnChargeAttack;
            @SKill.started -= instance.OnSKill;
            @SKill.performed -= instance.OnSKill;
            @SKill.canceled -= instance.OnSKill;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // GameSystem
    private readonly InputActionMap m_GameSystem;
    private List<IGameSystemActions> m_GameSystemActionsCallbackInterfaces = new List<IGameSystemActions>();
    private readonly InputAction m_GameSystem_Pause;
    private readonly InputAction m_GameSystem_Inventory;
    public struct GameSystemActions
    {
        private @Controls m_Wrapper;
        public GameSystemActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_GameSystem_Pause;
        public InputAction @Inventory => m_Wrapper.m_GameSystem_Inventory;
        public InputActionMap Get() { return m_Wrapper.m_GameSystem; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameSystemActions set) { return set.Get(); }
        public void AddCallbacks(IGameSystemActions instance)
        {
            if (instance == null || m_Wrapper.m_GameSystemActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameSystemActionsCallbackInterfaces.Add(instance);
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
            @Inventory.started += instance.OnInventory;
            @Inventory.performed += instance.OnInventory;
            @Inventory.canceled += instance.OnInventory;
        }

        private void UnregisterCallbacks(IGameSystemActions instance)
        {
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
            @Inventory.started -= instance.OnInventory;
            @Inventory.performed -= instance.OnInventory;
            @Inventory.canceled -= instance.OnInventory;
        }

        public void RemoveCallbacks(IGameSystemActions instance)
        {
            if (m_Wrapper.m_GameSystemActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameSystemActions instance)
        {
            foreach (var item in m_Wrapper.m_GameSystemActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameSystemActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameSystemActions @GameSystem => new GameSystemActions(this);
    private int m_KeyMouseSchemeIndex = -1;
    public InputControlScheme KeyMouseScheme
    {
        get
        {
            if (m_KeyMouseSchemeIndex == -1) m_KeyMouseSchemeIndex = asset.FindControlSchemeIndex("KeyMouse");
            return asset.controlSchemes[m_KeyMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnBaseAttack(InputAction.CallbackContext context);
        void OnChargeAttack(InputAction.CallbackContext context);
        void OnSKill(InputAction.CallbackContext context);
    }
    public interface IGameSystemActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
    }
}
