using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.AIBrain
{   
    public abstract class Consideration : ScriptableObject
    {
        public new string name;
        
        private float _score;
        public float score
        {
            get { return _score; }
            set
            {
                this._score = Mathf.Clamp01(value);
            }
        }
        public virtual void Awake()
        {
            score = 0;
        }
        public abstract float ScoreConsideration(NPCAIController npc);
    }
}