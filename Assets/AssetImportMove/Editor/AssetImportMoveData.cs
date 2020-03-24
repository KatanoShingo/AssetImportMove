using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class AssetImportMoveData : ScriptableObject
{
    public enum ObjectType
    {
        GameObject,
        Prefab,
        PrefabInstane,
        Other,
    }

    [System.Serializable]
    public class Item
    {
        public Object obj;
        //[System.NonSerialized]
        public string name = "";
        //[System.NonSerialized]
        public int instance_id;
        // [System.NonSerialized]
        public string path = "";
        //[System.NonSerialized]
        public GUIContent gui_content;

        //[System.NonSerialized]
        public ObjectType object_type;

        public void ResetInfo(Object obj)
        {
            this.obj = obj;
            if (Application.isPlaying && obj == null) return;

            instance_id = obj.GetInstanceID();
            path = AssetDatabase.GetAssetPath(instance_id);
            name = obj.name;
            obj = EditorUtility.InstanceIDToObject(instance_id);
            gui_content = new GUIContent(EditorGUIUtility.ObjectContent(obj, null));
        }
    }

    public List<Item> datas = new List<Item>();
  
    public void AddLast(Object obj)
    {
        if (obj == null) return;
        int find_index = datas.FindIndex((item) => item.obj == obj);
        if (find_index >= 0)
        {
            Item it = datas[find_index];

            datas.RemoveAt(find_index);
            it.ResetInfo(it.obj);
            datas.Add(it);
            return;
        }

        Item it2 = new Item();
        it2.ResetInfo(obj);
        datas.Add(it2);
    }


    public void RemoveAt(int index)
    {
        datas.RemoveAt(index);
    }

    /// <summary>
    /// すべての情報をリセット
    /// </summary>
    public void ResetAllInfo()
    {
        for (int i = datas.Count - 1; i >= 0; i--)
        {
            Item it = datas[i];
            if (!Application.isPlaying && it.obj == null)
            {
                datas.RemoveAt(i);
                continue;
            }
            it.ResetInfo(it.obj);
        }
    }

}
