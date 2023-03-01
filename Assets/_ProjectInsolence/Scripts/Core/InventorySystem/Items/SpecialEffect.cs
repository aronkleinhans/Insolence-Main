using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    [Serializable]
    //item effects like procs, buffs, debuffs, etc.
    public abstract class SpecialEffect : ScriptableObject
    {
        [SerializeField] public string effectID;
        [SerializeField] new string name;
        [SerializeField] string description;
        [SerializeField] int duration;
        [SerializeField] bool OnUse;
        [SerializeField] bool OnEquip;
        [SerializeField] bool OnBlock;
        [SerializeField] bool OnHit;
        [SerializeField] float specialEffectChance;

        [SerializeField] GameObject visualEffect;
        [SerializeField] AudioClip soundEffect;

        public abstract void ApplyEffect(GameObject target);
    }
}
