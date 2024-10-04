using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace MessyAIPlugin.MessyAI
{
    public class AIMemoryPool
    {
        private static AIMemoryPool _instance;

        public static AIMemoryPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AIMemoryPool();
                }

                return _instance;
            }
        }

        private Dictionary<Type, Stack<AIMemory>> _poolCached;

        private AIMemoryPool()
        {
            _poolCached = new Dictionary<Type, Stack<AIMemory>>();
        }

        public AIMemory AllocMemory(Type memoryType)
        {
            
            Debug.Assert(typeof(AIMemory).IsAssignableFrom(memoryType), $"{memoryType} is not AIMemory, it must be inherit from from AIMemory");
            if (memoryType == typeof(AIMemory))
            {
                return AIMemory.Instance;
            }

            if (_poolCached.TryGetValue(memoryType, out var cachedStack))
            {
                if (cachedStack.TryPop(out var memory))
                {
                    return memory;
                }
            }
            return (AIMemory)Activator.CreateInstance(memoryType);

        }

        public void FreeMemory(AIMemory memory)
        {
            Debug.Assert(memory != null);
            if (memory.GetType() != typeof(AIMemory))
            {
                if (_poolCached.TryGetValue(memory.GetType(), out var cachedStack))
                {
                    cachedStack.Push(memory);
                }
                else
                {
                    var newCachedStack = new Stack<AIMemory>();
                    newCachedStack.Push(memory);
                    _poolCached.Add(memory.GetType(),newCachedStack);
                }
            }
        }
    }

    public class AIMemory
    {
        public static AIMemory _instance;

        public static AIMemory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AIMemory();
                }

                return _instance;
            }
        }
        
        // Usually, AI Memory just only create by Instance method, but it's children can be created in CachePool 
        public AIMemory()
        {
            
        }
    }
}