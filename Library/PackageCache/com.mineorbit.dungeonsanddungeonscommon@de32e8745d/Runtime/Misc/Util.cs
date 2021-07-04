using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Google.Protobuf;
using NetLevel;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Util
    {
        
        public static Func<Location, Vector3> LocationToVector = (l) => { return new Vector3(l.X, l.Y, l.Z);};
        public static Func<Vector3, Location> VectorToLocation = (v) => { var l = new Location();
            l.X = v.x;
            l.Y = v.y;
            l.Z = v.z;
            return l;
        };
        
         class HuffNode
        {
            public HuffNode l;
            public HuffNode r;
            public HuffNode p;
            public int type = 0;
            public float w = 0;

            public int CompareTo(HuffNode other)
            {
                if (w <= other.w)
                {
                    return 1;
                }
                return 0;
            }
        }
        class HuffNodeComparer : IComparer<HuffNode>
        {
            public int Compare(HuffNode x, HuffNode y)
            {
                // TODO: Handle x or y being null, or them not having names
                return x.CompareTo(y);
            }
        }

        class PriorityQueue
        {
            private List<HuffNode> values = new List<HuffNode>();
            public void Enqueue(HuffNode d)
            {
                values.Add(d);
                values.Sort( new HuffNodeComparer());
            }
            
            public HuffNode Dequeue()
            {
                var v = values.First();
                values.RemoveAt(0);
                return v;
            }

            public int Count()
            {
                return values.Count;
            }
        }
        
        public static List<HuffmanEntry> BuildHuffmanTable(List<LevelObject> levelObjects)
        {
            Dictionary<int, float> weights = new Dictionary<int, float>();
            foreach (LevelObject l in levelObjects)
            {
                float freq;
                if(weights.TryGetValue(l.levelObjectDataType, out freq))
                {
                    weights[l.levelObjectDataType] = freq + 1;
                }else
                {
                    weights.Add(l.levelObjectDataType,1);
                }
            }

            float count = levelObjects.Count;

            PriorityQueue todo = new PriorityQueue();
            Dictionary<int, HuffNode> nodes = new Dictionary<int, HuffNode>();

            foreach (var type in weights.Keys)
            {
                
                float weight = weights[type] / count;
                Debug.Log("Weight: "+type+" "+weight);
                HuffNode huffNode = new HuffNode();
                huffNode.type = type;
                huffNode.w = weight;
                todo.Enqueue(huffNode);
                nodes.Add(type,huffNode);
            }

            while (1 < todo.Count())
            {
                var h1 = todo.Dequeue();
                var h2 = todo.Dequeue();
                HuffNode huffNode = new HuffNode();
                huffNode.l = h1;
                huffNode.r = h2;
                huffNode.w = h1.w + h2.w;
                h1.p = huffNode;
                h2.p = huffNode;
                todo.Enqueue(huffNode);
            }
            
            HuffNode root = todo.Dequeue();
            List<HuffmanEntry> huffmanEntries = new List<HuffmanEntry>();
            foreach (var type in weights.Keys)
            {
                List<bool> pathToType = BuildPath(nodes[type]);
                BitArray b = new BitArray(pathToType.ToArray());
                int numOfBytes = (int) Math.Ceiling((double) pathToType.Count / 8);
                byte[] bytes = new byte[numOfBytes];
                b.CopyTo(bytes,0);
                HuffmanEntry huffmanEntry = new HuffmanEntry();
                ByteString s = ByteString.CopyFrom(bytes);
                //QUICK FIX HIER
                huffmanEntry.Code = 0;
                huffmanEntry.Type = type;
                huffmanEntries.Add(huffmanEntry);
            }

            return huffmanEntries;
        }

        private static List<HuffmanEntry> currentChunkHuffmanTable;
        public static int CodeToType(ByteString code,  List<HuffmanEntry> huffmanEntries)
        {
            foreach (var x in huffmanEntries)
            {
                if (x.Code.Equals(code))
                {
                    return x.Type;
                }
            }

            // HIER SPÄTER Type VON ERROR BLOCK
            return 0;
        }

        public static int CodeToType(ByteString code)
        {
            return CodeToType(code, currentChunkHuffmanTable);
        }


        public static ByteString TypeToCode(int type)
        {
            return TypeToCode(type, currentChunkHuffmanTable);
        }
        
        public static ByteString TypeToCode(int type, List<HuffmanEntry> huffmanEntries)
        {
            foreach (var x in huffmanEntries)
            {
                if (x.Type.Equals(type))
                {
                    //QUICK FIX
                    return ByteString.Empty;
                }
            }

            // HIER SPÄTER Code VON ERROR BLOCK
            return ByteString.Empty;
        }
        
        private static List<bool> BuildPath(HuffNode node)
        {
            List<bool> path = new List<bool>();
            if(node.p != null)
            {
            path = BuildPath(node.p);
            if (node == node.p.l)
            {
                path.Add(true);
            }
            else
            {
                path.Add(false);
            }
            
            }

            return path;
        }

    }
}