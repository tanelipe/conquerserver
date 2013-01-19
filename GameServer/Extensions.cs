using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public static class Extensions
    {
        public static void ThreadSafeAdd<T, T1>(this Dictionary<T, T1> dictionary, T Key, T1 Value)
        {
            lock (dictionary)
            {
                if (dictionary.ContainsKey(Key))
                {
                    dictionary[Key] = Value;
                }
                else
                {
                    dictionary.Add(Key, Value);
                }
            }
        }
        public static void ThreadSafeRemove<T, T1>(this Dictionary<T, T1> dictionary, T Key)
        {
            lock (dictionary)
            {
                if(dictionary.ContainsKey(Key))
                    dictionary.Remove(Key);
            }
        }
    }

}
