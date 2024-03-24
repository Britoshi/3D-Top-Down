using Game;
using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformData
{
    public Entity owner;
    public GameObject root;

    public Transform head, torso;
    public Transform leftHand, rightHand;
    public Transform leftFoot, rightFoot;
    public Transform leftHandWeapon, rightHandWeapon;
    public SortedDictionary<EquipmentLocation, Transform> targets;

    public void Initialize(Entity entity)
    {
        owner = entity;
        targets = new()
        {
            [EquipmentLocation.HEAD] = head,
            [EquipmentLocation.BODY] = torso,
            [EquipmentLocation.LEFT_HAND] = leftHandWeapon,
            [EquipmentLocation.RIGHT_HAND] = rightHandWeapon,
            
        };

        root = owner.gameObject;

        
    }
}
