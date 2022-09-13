using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public int collected;

        public event Action Changed;

        public void Collect(Loot loot)
        {
            collected += loot.value;
            Changed?.Invoke();
        }
        
        public void Add(int loot)
        {
            collected += loot;
            Changed?.Invoke();
        }
    }
}