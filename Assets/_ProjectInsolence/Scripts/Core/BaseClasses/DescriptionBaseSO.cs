using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    /// <summary>
    /// Base class for ScriptableObjects that need a public description field.
    /// </summary>
    public class DescriptionBaseSO : SerializableScriptableObject
    {
        [TextArea] public string description;
    }
}
