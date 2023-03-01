using Insolence;
using UnityEngine;

namespace Insolence.Core
{
    [CreateAssetMenu(fileName = "Lesser Heal", menuName = "Insolence/Inventory/Items/Special Effects/Lesser Heal")]
    public class LesserHeal : SpecialEffect
    {
        [Header("Effect Specific")]
        [SerializeField] int healAmount;

        public override void ApplyEffect(GameObject target)
        {
            //heal the user
            target.GetComponent<CharacterStatus>().currentHealth += healAmount;
            //show visual effect
            //play sound effect
        }
    }
}
