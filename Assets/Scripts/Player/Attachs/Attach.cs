using UnityEngine;

namespace Game.Ammunitions
{
    public abstract class Attach : ScriptableObject
    {
        public abstract void Accept(GameObject gameObject);
    }
}
