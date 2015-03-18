using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public class TreeBuilder
    {
        public static T GetTree<T, Y>(Y[] entityList, Func<Y, long?> getParentId, Action<T, T[]> assignChildren, Func<Y, long> getValue, Action<T, Y> evaluate) where T : class,new()
        {

            T root = new T();
            Dictionary<long, T> dic = new Dictionary<long, T>();
            List<T> children = new List<T>();

            for (int i = 0; i < entityList.Length; i++)
            {
                long val = getValue(entityList[i]);
                if (dic.ContainsKey(val)) continue;
                long? parentId = getParentId(entityList[i]);
                if (parentId != null && !dic.ContainsKey(parentId.Value)) continue;

                T node = new T();
                evaluate(node, entityList[i]);

                dic.Add(val, node);
                children.Add(node);
                if (i == entityList.Length - 1 || parentId != getParentId(entityList[i + 1]))
                {
                    if (parentId == null)
                    {
                        assignChildren(root, children.ToArray());
                    }
                    else
                    {
                        assignChildren(dic[parentId.Value], children.ToArray());
                    }

                    if (dic.Count == entityList.Length) break;
                    children = new List<T>();
                }




                if (dic.Count != entityList.Length)
                {
                    if (i == entityList.Length - 1) i = 0;
                }
            }
            return root;
        }
    }
}
