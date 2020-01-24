using System.Collections.Generic;
using System.Text;

namespace LRUCache.Core
{
    public class LRUCache<T> : ICache<T>
    {
        private class CacheNode
        {
            public CacheNode Previous { get; set; }
            public CacheNode Next { get; set; }

            public string Key { get; set; }
            public T Value { get; set; }

            public override string ToString() => $"Key {Key}";
        }

        private CacheNode Head;
        private CacheNode Tail;
        private int count = 0;

        private readonly Dictionary<string, CacheNode> cache;

        private readonly int size;

        public LRUCache(int size)
        {
            this.size  = size;
            this.cache = new Dictionary<string, CacheNode>();

            this.Head = null;
            this.Tail = null;
        }

        public void Add(string key, T value)
        {
            if (this.cache.ContainsKey(key))
                // TODO Shall we bring item in front ???
                return;

            var node = new CacheNode
            {
                Key   = key,
                Next  = Head,
                Value = value
            };

            if (this.Head != null)
                this.Head.Previous = node;

            this.Head = node;
            if (this.Tail == null)
                this.Tail = node;

            this.cache.Add(key, node);
            count++;

            if (count > size)
            {
                this.cache.Remove(this.Tail.Key);
                this.Tail      = this.Tail.Previous;
                this.Tail.Next = null;

                count--;
            }
        }

        public T Get(string key)
        {
            if (!this.cache.ContainsKey(key))
                throw new KeyNotFoundException($"Key '{key}' not found in cache");

            var node = this.cache[key];
            if (node != this.Head)
            {
                node.Previous.Next = node.Next;
                if (node.Next != null)
                    node.Next.Previous = node.Previous;

                node.Next = this.Head;
                this.Head.Previous = node;
                this.Head = node;

                if (node == this.Tail)
                {
                    this.Tail = node.Previous;
                }
            }
            return node.Value;
        }

        public string DebugInfo()
        {
            if (this.Head == null && this.Tail == null)
                return "Empty";

            var sb = new StringBuilder();
            sb.Append($"HEAD: {this.Head} - TAIL: {this.Tail} > ");

            var node = this.Head;
            do
            {
                sb.Append($"{node.Key}");
                node = node.Next;
                if (node != null)
                    sb.Append(", ");
            }
            while (node != null);

            return sb.ToString();
        }
    }
}
