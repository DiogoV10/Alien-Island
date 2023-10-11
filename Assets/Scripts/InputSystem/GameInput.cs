// Ignore Spelling: Melee

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace V10
{
    public class GameInput : MonoBehaviour
    {


        private const string PLAYER_PREFS_BINDINGS = "InputBindings";


        public static GameInput Instance { get; private set; }


        public event EventHandler OnAttackMeleeAction;
        public event EventHandler OnAttackMeleeHoldAction;
        public event EventHandler OnJumpAction;


        public enum Binding
        {
            Move_Up,
            Move_Down,
            Move_Left,
            Move_Right,
            Run,
            Jump,
            Dash,
            Camera_Rotation_Left,
            Camera_Rotation_Right,
        }


        private InputSystem playerInputActions;


        private void Awake()
        {
            Instance = this;

            playerInputActions = new InputSystem();

            if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
            {
                playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
            }

            playerInputActions.Movement.Enable();
            playerInputActions.Camera.Enable();
            playerInputActions.Combat.Enable();

            playerInputActions.Combat.AttackMelee.canceled += AttackMelee_canceled;
            playerInputActions.Combat.AttackMeleeHold.performed += AttackMeleeHold_performed;
            playerInputActions.Movement.Jump.performed += Jump_performed;
        }

        private void AttackMeleeHold_performed(InputAction.CallbackContext obj)
        {
            OnAttackMeleeHoldAction?.Invoke(this, EventArgs.Empty);
        }

        private void Jump_performed(InputAction.CallbackContext obj)
        {
            OnJumpAction?.Invoke(this, EventArgs.Empty);
        }

        private void AttackMelee_canceled(InputAction.CallbackContext obj)
        {
            OnAttackMeleeAction?.Invoke(this, EventArgs.Empty);
        }

        private void OnDestroy()
        {
            playerInputActions.Dispose();
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = playerInputActions.Movement.Move.ReadValue<Vector2>();

            inputVector = inputVector.normalized;

            return inputVector;
        }

        public string GetBindingText(Binding binding)
        {
            switch (binding)
            {
                default:
                case Binding.Move_Up:
                    return playerInputActions.Movement.Move.bindings[1].ToDisplayString();
                case Binding.Move_Down:
                    return playerInputActions.Movement.Move.bindings[2].ToDisplayString();
                case Binding.Move_Left:
                    return playerInputActions.Movement.Move.bindings[3].ToDisplayString();
                case Binding.Move_Right:
                    return playerInputActions.Movement.Move.bindings[4].ToDisplayString();                
            }
        }

        public void RebindBinding(Binding binding, Action onActionRebound)
        {
            playerInputActions.Movement.Disable();
            playerInputActions.Camera.Disable();
            playerInputActions.Combat.Disable();

            InputAction inputAction;
            int bindingIndex;

            switch (binding)
            {
                default:
                case Binding.Move_Up:
                    inputAction = playerInputActions.Movement.Move;
                    bindingIndex = 1;
                    break;
                case Binding.Move_Down:
                    inputAction = playerInputActions.Movement.Move;
                    bindingIndex = 2;
                    break;
                case Binding.Move_Left:
                    inputAction = playerInputActions.Movement.Move;
                    bindingIndex = 3;
                    break;
                case Binding.Move_Right:
                    inputAction = playerInputActions.Movement.Move;
                    bindingIndex = 4;
                    break;                
            }

            inputAction.PerformInteractiveRebinding(bindingIndex)
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    playerInputActions.Movement.Enable();
                    playerInputActions.Camera.Enable();
                    playerInputActions.Combat.Enable();
                    onActionRebound();

                    PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();

                    //OnBindingRebind?.Invoke(this, EventArgs.Empty);
                })
                .Start();
        }


    }
}
