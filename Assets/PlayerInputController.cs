using Game.StateMachine.Player;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace Game
{
    public enum InputMode
    {
        PC, CONTROLLER, MOBILE
    }
    public class PlayerInputController : MonoBehaviour
    {
        public GameObject mobileControlGameObject;
        public PStateMachine stateMachine;
        InputMode inputMode;

        public KeyCode pauseKeyCode = KeyCode.Escape;
        public KeyCode pauseKeyCodeBackUP = KeyCode.Return;
        public KeyCode inventoryKeyCode = KeyCode.I;

        // Start is called before the first frame update
        void Start()
        {
            EnablePCControl();
        }

        public void MobileSetRunning(bool running)
        {
            stateMachine.IsRunning= running;
        }
        public void Attack()
        {
            stateMachine.entity.abilityController.TriggerPrimaryAttack(); 
        }
        public void PCControlUpdate()
        {
            void HandleInGameControls()
            {
                if (GameSystem.Paused) return;

                stateMachine.IsRunning = stateMachine.sprintHold;
                stateMachine.IsMoving =
                    Input.GetButton("Horizontal") ||
                    Input.GetButton("Vertical");

                stateMachine.AssertInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                //stateMachine.GetAnimator().SetBool("moving", stateMachine.IsMoving);

                stateMachine.IsAiming = Input.GetButton("Fire2");
                if (stateMachine.IsAiming)
                {
                    Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Plane groundPlane = new(Vector3.up, Vector3.zero);
                    if (groundPlane.Raycast(cameraRay, out float rayLength))
                    {
                        Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                        Debug.DrawLine(cameraRay.origin, pointToLook, Color.yellow);

                        stateMachine.aimingPoint = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
                    }
                }
                if (Input.GetButtonDown("Fire1"))
                    Attack();
            }

            HandleInGameControls();
            if (Input.GetKeyDown(inventoryKeyCode)) GameUIManager.OpenItemPanel();
            else if (Input.GetKeyDown(KeyCode.P)) GameUIManager.OpenEquipmentPanel();
            else if (Input.GetKeyDown(pauseKeyCode)) GameUIManager.TogglePause();

        }

        public void MobileMovementInput(Vector2 input)
        {
            stateMachine.inputVector2 = input.normalized;
            stateMachine.inputVector3 = new(stateMachine.inputVector2.x, 0, stateMachine.inputVector2.y);
            stateMachine.lastInput3 = new(stateMachine.inputVector3.x, stateMachine.inputVector3.y, stateMachine.inputVector3.z);

            stateMachine.IsMoving = true;
        }

        public void MobileAimInput(Vector2 input)
        {
            var lookPos = transform.position;
            lookPos.x += input.x;
            lookPos.z += input.y;
            stateMachine.aimingPoint = lookPos;
            stateMachine.IsAiming = true;
        }

        public  void LiftUpAimJoyStick()
        {
            stateMachine.aimingPoint = Vector3.zero;
            stateMachine.IsAiming = false;
        }

        public void LateUpdate()
        {

        }

        public void LiftUpJoyStick()
        {  
            stateMachine.IsMoving = false;
            stateMachine.inputVector2 = Vector2.zero;
            stateMachine.inputVector3 = Vector3.zero;
        }


        void EnableMobileControl()
        {
            mobileControlGameObject.SetActive(true);
            inputMode = InputMode.MOBILE;
        }
        void CheckMobileControl()
        {
            // Check if touch input is supported and there are touches on the screen
            if (Input.touchSupported && Input.touchCount > 0)
            {
                EnableMobileControl(); 
            }
        }
        void EnablePCControl()
        { 
            mobileControlGameObject.SetActive(false);
            inputMode = InputMode.PC;
        }
        void CheckPCControl()
        {
            if (Input.anyKeyDown&& !(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)))
            {
                EnablePCControl();
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            switch (inputMode)
            {
                case InputMode.PC:
                    PCControlUpdate();

                    CheckMobileControl();
                    break;
                case InputMode.CONTROLLER:
                    break;
                case InputMode.MOBILE:
                    CheckPCControl();
                    break;
            }

        }
    }
}