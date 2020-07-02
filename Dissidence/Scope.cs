using System;
using System.Linq;
using System.Collections.Generic;

namespace Metatron.Dissidence {
    [Serializable]
    public class Scope : IDictionary<String, Object> {
        public Scope? Parent;
        private Dictionary<String, Object> lookup = new Dictionary<String, Object>();

        public Scope(Scope? parent=null) {
            Parent = parent;
        }

        public Object this[String name] {
            get {
                if (lookup.ContainsKey(name)) {
                    return lookup[name];
                } else if (Parent?.ContainsKey(name) ?? false) {
                    return Parent[name];
                } else {
                    throw new ArgumentException($"Variable {name} not found");
                }
            }

            set {
                if (lookup.ContainsKey(name)) {
                    lookup[name] = value;
                } else if (Parent?.ContainsKey(name) ?? false) {
                    Parent[name] = value;
                } else {
                    lookup[name] = value;
                }
            }
        }

        public void Add(String name, Object value) {
            this[name] = value;
        }

        public bool ContainsKey(String name) {
            return lookup.ContainsKey(name) || (Parent?.ContainsKey(name) ?? false);
        }

        public bool Remove(String name) {
            if (lookup.ContainsKey(name)) {
                lookup.Remove(name);
                return true;
            } else {
                return Parent?.Remove(name) ?? false;
            }
        }

        public bool TryGetValue(String name, out Object value) {
            if (lookup.ContainsKey(name)) {
                value = lookup[name];
                return true;
            } else if (Parent?.ContainsKey(name) ?? false) {
                value = Parent[name];
                return true;
            } else {
                value = default;
                return false;
            }
        }

        public ICollection<String> Keys {
            get {
                return lookup.Keys.Concat(Parent?.Keys ?? Enumerable.Empty<String>()).ToList();
            }
        }

        public ICollection<Object> Values {
            get {
                return lookup.Values.Concat(Parent?.Values ?? Enumerable.Empty<Object>()).ToList();
            }
        }

        public void Add(KeyValuePair<String, Object> entry) {
            Add(entry.Key, entry.Value);
        }

        public void Clear() {
            // TODO: should i clear parent too?
            lookup.Clear();
        }

        public bool Contains(KeyValuePair<String, Object> entry) {
            return ContainsKey(entry.Key) && this[entry.Key] == entry.Value;
        }

        public bool Remove(KeyValuePair<String, Object> entry) {
            if (lookup.ContainsKey(entry.Key) && lookup[entry.Key] == entry.Value) {
                lookup.Remove(entry.Key);
                return true;
            } else {
                return Parent?.Remove(entry) ?? false;
            }
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int index) {
            if (array == null) {
                throw new ArgumentOutOfRangeException("Array is null");
            }
            if (index < 0) {
                throw new ArgumentOutOfRangeException("Index is less than zero");
            }
            if (Count > array.Length - index) {
                throw new ArgumentException("Array does not have enough space");
            }
            var i = index;
            foreach (var element in this) {
                array[index++] = element;
            }
        }

        public int Count {
            get {
                // NOTE: may have duplicate counts because of shadowing.
                return lookup.Values.Count + (Parent?.Count ?? 0);
            }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator() {
            return lookup.Concat(Parent).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
