using Game;
using Game.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollower : MonoBehaviour
{
    [SerializeField] PlayerEntity entity;
    HumanoidStateMachine stateMachine;
    [SerializeField] Transform player;

    public float baseSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = (HumanoidStateMachine)entity.stateMachine; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //First,   clamp,  then  FOV
        if(stateMachine.IsAiming)
        {
            Vector3 mousePositionPixels = Input.mousePosition;

            // Calculate the screen dimensions in pixels
            float screenWidthPixels = Screen.width;
            float screenHeightPixels = Screen.height;

            // Calculate the ratio
            float mouseXRatio = mousePositionPixels.x / screenWidthPixels;
            float mouseYRatio = mousePositionPixels.y / screenHeightPixels;

            // Output the ratios
            Debug.Log("Mouse X Ratio: " + mouseXRatio);
            Debug.Log("Mouse Y Ratio: " + mouseYRatio);

            var target = (stateMachine.aimingPoint + player.position) / 2;
            var vel = (baseSpeed * Time.deltaTime);
            vel += Vector3.Distance(target, transform.position)/3 * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, target, vel);
        } else
        transform.position = player.position;
    }
}
