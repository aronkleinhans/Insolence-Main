using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insolence.Core;

namespace Insolence.AIBrain
{
    public abstract class Action : ScriptableObject
    {
        public new string name;
        private float _score;

        public float score
        {
            get { return _score; }
            set 
            {
                _score = Mathf.Clamp01(value);
            }
        }

        public Consideration[] considerations;

        public virtual void Awake()
        {
            score = 0;  
        }

        public abstract void Execute(NPCAIController npc);
    }
}