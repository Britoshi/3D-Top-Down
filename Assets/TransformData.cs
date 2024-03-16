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
    public SortedDictionary<EquippableArea, Transform> targets;

    public void Initialize(Entity entity)
    {
        owner = entity;
        targets = new()
        {
            [EquippableArea.HEAD] = head,
            [EquippableArea.BODY] = torso
        };

        root = owner.gameObject;

        
    }
}
