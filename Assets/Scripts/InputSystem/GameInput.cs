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
        public event EventHandler OnAttackRangeStartAction;
        public event EventHandler OnAttackRangeFinishAction;
        public event EventHandler OnUltimateMeleeAction;
        public event EventHandler OnUltimateRangeAction;
        public event EventHandler OnChangeMeleeWeapon;
        public event EventHandler OnChangeRangeWeapon;
        public event EventHandler OnSkillAction;
        public event EventHandler OnLockOn;
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
            playerInputActions.Combat.AttackRangeStart.performed += AttackRangeStart_performed;
            playerInputActions.Combat.AttackRangeFinish.performed += AttackRangeFinish_performed;
            playerInputActions.Combat.UltimateMelee.performed += UltimateMelee_performed;
            playerInputActions.Combat.UltimateRange.performed += UltimateRange_performed;
            playerInputActions.Combat.Skill.performed += Skill_performed;
            playerInputActions.Combat.ChangeMeleeWeapon.performed += ChangeMeleeWeapon_performed;
            playerInputActions.Combat.ChangeRangeWeapon.performed += ChangeRangeWeapon_performed;
            playerInputActions.Combat.LockOn.performed += LockOn_performed;

            playerInputActions.Movement.Jump.performed += Jump_performed;
        }

        private void AttackRangeFinish_performed(InputAction.CallbackContext obj)
        {
            OnAttackRangeFinishAction?.Invoke(this, EventArgs.Empty);
        }

        private void LockOn_performed(InputAction.CallbackContext obj)
        {
            OnLockOn?.Invoke(this, EventArgs.Empty);
        }

        private void ChangeMeleeWeapon_performed(InputAction.CallbackContext obj)
        {
            OnChangeMeleeWeapon?.Invoke(this, EventArgs.Empty);
        }

        private void ChangeRangeWeapon_performed(InputAction.CallbackContext obj)
        {
            OnChangeRangeWeapon?.Invoke(this, EventArgs.Empty);
        }

        private void Skill_performed(InputAction.CallbackContext obj)
        {
            OnSkillAction?.Invoke(this, EventArgs.Empty);
        }

        private void UltimateRange_performed(InputAction.CallbackContext obj)
        {
            OnUltimateRangeAction?.Invoke(this, EventArgs.Empty);
        }

        private void UltimateMelee_performed(InputAction.CallbackContext obj)
        {
            OnUltimateMeleeAction?.Invoke(this, EventArgs.Empty);
        }

        private void AttackRangeStart_performed(InputAction.CallbackContext obj)
        {
            OnAttackRangeStartAction?.Invoke(this, EventArgs.Empty);
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

        public float GetMouseWheelScrollY()
        {
            float inputVector = playerInputActions.Camera.MouseScrollY.ReadValue<float>();

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
