// LRUCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace CrossUI.Touch.Dialog.Utilities {
    public class LRUCache<TKey, TValue> where TValue : class, IDisposable {

        /// <summary>
        /// The dict. The cache reporsitory
        /// </summary>
        private readonly Dictionary<TKey, LinkedListNode<TValue>> _dict;

        /// <summary>
        /// The revdict. Use in Evict, get key to remove element at _dict
        /// </summary>
        private readonly Dictionary<LinkedListNode<TValue>, TKey> _revdict;

        /// <summary>
        /// The list. Use in Evict, remove last(old) element
        /// </summary>
        private readonly LinkedList<TValue> list;

        /// <summary>
        /// The entry limit. Maxinum solts in the _dict 
        /// </summary>
        private readonly int _entryLimit;

        /// <summary>
        /// The size limit.
        /// </summary>
        private readonly int _sizeLimit;

        /// <summary>
        /// The size of the current.
        /// </summary>
        private int _currentSize;

        /// <summary>
        /// The slot size func. Use this function to calculate size of TValue
        /// </summary>
        private readonly Func<TValue, int> _slotSizeFunc;

        public LRUCache(int entryLimit)
            : this(entryLimit, 0, null) {
        }

        public LRUCache(int entryLimit, int sizeLimit, Func<TValue, int> slotSizer) {
            list = new LinkedList<TValue>();
            _dict = new Dictionary<TKey, LinkedListNode<TValue>>();
            _revdict = new Dictionary<LinkedListNode<TValue>, TKey>();

            if (sizeLimit != 0 && slotSizer == null)
                throw new ArgumentNullException("If sizeLimit is set, the slotSizer must be provided");

            this._entryLimit = entryLimit;
            this._sizeLimit = sizeLimit;
            this._slotSizeFunc = slotSizer;
        }

        //Evict->驱逐
        private void Evict() {
            LinkedListNode<TValue> last = list.Last;
            TKey key = _revdict[last];

            if (_sizeLimit > 0) {
                int size = _slotSizeFunc(last.Value);
                _currentSize -= size;
            }

            _dict.Remove(key);
            _revdict.Remove(last);
            list.RemoveLast();
            last.Value.Dispose();

            Console.WriteLine("Evicted, got: {0} bytes and {1} slots", _currentSize, list.Count);
        }

        //Purge->清除
        public void Purge() {
            foreach (var element in list)
                element.Dispose();

            _dict.Clear();
            _revdict.Clear();
            list.Clear();
            _currentSize = 0;
        }

        public TValue this [TKey key] {
            get {
                LinkedListNode<TValue> node;

                if (_dict.TryGetValue(key, out node)) {
                    list.Remove(node);
                    list.AddFirst(node);

                    return node.Value;
                }
                return null;
            }

            set {
                LinkedListNode<TValue> node;
                int size = _sizeLimit > 0 ? _slotSizeFunc(value) : 0;

                if (_dict.TryGetValue(key, out node)) {
                    if (_sizeLimit > 0 && node.Value != null) {
                        int repSize = _slotSizeFunc(node.Value);
                        _currentSize -= repSize;
                        _currentSize += size;
                    }

                    // If we already have a key, move it to the front
                    list.Remove(node);
                    list.AddFirst(node);

                    // Remove the old value
                    if (node.Value != null)
                        node.Value.Dispose();
                    node.Value = value;
                    while (_sizeLimit > 0 && _currentSize > _sizeLimit && list.Count > 1)
                        Evict();
                    return;
                }
                if (_sizeLimit > 0) {
                    while (_sizeLimit > 0 && _currentSize + size > _sizeLimit && list.Count > 0)
                        Evict();
                }
                if (_dict.Count >= _entryLimit)
                    Evict();
                // Adding new node
                node = new LinkedListNode<TValue>(value);
                list.AddFirst(node);
                _dict[key] = node;
                _revdict[node] = key;
                _currentSize += size;
                Console.WriteLine("new size: {0}m with {1}", _currentSize / (1024 * 1024), list.Count);
            }
        }

        //TODO: print something?
        public override string ToString() {
            return "LRUCache dict={0} revdict={1} list={2}";
        }
    }
}