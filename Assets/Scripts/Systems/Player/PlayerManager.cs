using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerCharacter PlayerCharacter;
    [SerializeField]
    private PlayerCharacterCollider PlayerCharacterCollider;
    [SerializeField]
    private PlayerCamera PlayerCamera;
    [SerializeField]
    private PlayerInteraction PlayerInteraction;
    [SerializeField]
    private PlayerInventory PlayerInventory;
    [SerializeField]
    private PlayerEquipment PlayerEquipment;
    [SerializeField]
    private PlayerHotbar PlayerHotbar;
    [SerializeField]
    private PlayerStats PlayerStats;

    private PlayerInputActions _PlayerInputActions;
    private GameplayTimeManager _GameplayTimeManager;

    private bool FixedUpdateOccurredThisFrame;

    public Player Player => new Player(
            PlayerCharacter,
            PlayerCharacterCollider,
            PlayerCamera,
            PlayerInteraction,
            PlayerInventory,
            PlayerEquipment,
            PlayerHotbar,
            PlayerStats
        );

    private void Start()
    {
        FocusInput();

        _PlayerInputActions = new PlayerInputActions();
        _PlayerInputActions.Enable();

        PlayerCharacter.Initialize();
        PlayerCharacterCollider.Initialize();
        PlayerCamera.Initialize(PlayerCharacter.CameraTarget);
        PlayerInteraction.Initialize(Player, PlayerCamera.transform);
        PlayerInventory.Initialize();
        PlayerEquipment.Initialize(PlayerInventory);
        PlayerHotbar.Initialize(PlayerInventory);
        PlayerStats.Health.OnDeplete.AddListener(PlayerStatsHeahlthDeplete);

        _GameplayTimeManager = GameObject.FindAnyObjectByType<GameplayTimeManager>();
        if (_GameplayTimeManager != null)
        {
            _GameplayTimeManager.OnPause += GameplayPause;
            _GameplayTimeManager.OnResume += GameplayResume;
        }
    }

    private void OnDestroy()
    {
        if (_GameplayTimeManager != null)
        {
            _GameplayTimeManager.OnPause -= GameplayPause;
            _GameplayTimeManager.OnResume -= GameplayResume;
        }

        PlayerStats.Health.OnDeplete.RemoveListener(PlayerStatsHeahlthDeplete);
        PlayerCharacterCollider.Cleanup();

        _PlayerInputActions.Disable();

        Cursor.lockState = CursorLockMode.None;
    }

    private void FixedUpdate()
    {
        bool isInputActive = Cursor.lockState == CursorLockMode.Locked;

        var input = _PlayerInputActions.Gameplay;

        PlayerCameraInput cameraInput = isInputActive ? new()
        {
            Look = input.Look.ReadValue<Vector2>(),
        } : default;

        PlayerCamera.UpdateRotation(cameraInput);

        PlayerCharacterInput characterInput = isInputActive ? new()
        {
            Move = input.Move.ReadValue<Vector2>(),
            Rotation = PlayerCamera.transform.rotation,
            Jump = input.Jump.WasPressedThisFrame(),
            JumpSustain = input.Jump.IsPressed(),
            Crouch = input.Crouch.WasPressedThisFrame()
        } : default;

        PlayerCharacter.UpdateInput(characterInput);
        PlayerCharacter.PerformUpdate(Time.deltaTime);
        PlayerCharacterCollider.Copy();

        PlayerInteractionInput interactionInput = isInputActive ? new()
        {
            ActionCycleOffset = (input.InteractionCycleNegative.WasPressedThisFrame() ? -1 : 0) + (input.InteractionCyclePositive.WasPressedThisFrame() ? 1 : 0),
            PerformAction = input.InteractionPerform.WasPressedThisFrame(),
        } : default;

        PlayerInteraction.UpdateInput(interactionInput);

        PlayerInventoryInput inventoryInput = isInputActive ? new()
        {
            ToggleMenu = input.Inventory.WasPressedThisFrame(),
        } : default;

        PlayerInventory.UpdateInput(inventoryInput);

        PlayerHotbarInput hotbarInput = isInputActive ? new()
        {
            SelectSlotOne = input.HotbarSlotOne.WasPressedThisFrame(),
            SelectSlotTwo = input.HotbarSlotTwo.WasPressedThisFrame(),
            SelectSlotThree = input.HotbarSlotThree.WasPressedThisFrame(),
            SelectSlotFour = input.HotbarSlotFour.WasPressedThisFrame(),
            PerformPrimaryAction = input.WeaponPrimary.WasPerformedThisFrame(),
            PerformSecondaryAction = input.WeaponPrimary.WasPerformedThisFrame(),
        } : default;

        PlayerHotbar.UpdateInput(hotbarInput);

        if (_GameplayTimeManager != null)
        {
            if (input.TogglePause.WasPressedThisFrame())
            {
                _GameplayTimeManager.TogglePause();
            }
        }

        FixedUpdateOccurredThisFrame = true;
    }

    private void LateUpdate()
    {
        if (FixedUpdateOccurredThisFrame)
        {
            PlayerCamera.UpdatePosition(PlayerCharacter.CameraTarget);
            PlayerInteraction.UpdateTransform(PlayerCamera.transform);
        }

        FixedUpdateOccurredThisFrame = false;
    }

    private void GameplayPause()
    {
        UnfocusInput();
    }

    private void GameplayResume()
    {
        FocusInput();
    }

    private void PlayerStatsHeahlthDeplete()
    {
        Destroy(gameObject);
    }

    private static void UnfocusInput()
    {
        Cursor.lockState = CursorLockMode.None;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    }

    private static void FocusInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    }
}
