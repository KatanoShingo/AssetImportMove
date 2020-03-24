using UnityEditor;  // UnityEditorを追記してね
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class AssetImportMove// : AssetPostprocessor   //!< AssetPostprocessorを継承するよ！
{
    string path;
    string file_name;
    string file_ext;

    //full_path must be a file path
    public AssetImportMove(string full_path)
    {
        Paser(full_path);
    }

    void Paser(string full_path)
    {
        path = GetPath(full_path);
        string full_name = GetFullFileName(full_path);
        SeprateFileName(full_name, out file_name, out file_ext);
    }
    

    /// <summary>
    /// ディレクトリ名を返します
    /// </summary>
    /// <param name="full_path"></param>
    /// <returns></returns>
    public static string GetPath(string full_path, bool end_with_sperator = true)
    {
        full_path = RegularPath(full_path);
        int find = -1;
        for (int i = full_path.Length - 1; i >= 0; i--)
        {
            if (full_path[i] == '/' || full_path[i] == '\\' || full_path[i] == System.IO.Path.DirectorySeparatorChar)
            {
                find = i;
                break;
            }
        }
        string path = full_path;
        if (find != -1) path = full_path.Substring(0, find);

        if (end_with_sperator && !path.EndsWith("" + System.IO.Path.DirectorySeparatorChar))
        {
            path += System.IO.Path.DirectorySeparatorChar;
        }
        return path;
    }

    static string RegularPath(string full_path)
    {
        full_path = full_path.Replace('/', System.IO.Path.DirectorySeparatorChar);
        full_path = full_path.Replace('\\', System.IO.Path.DirectorySeparatorChar);
        return full_path;
    }


    /// <summary>
    /// 拡張子を含むファイル名を返す
    /// </summary>
    /// <param name="full_path"></param>
    /// <returns></returns>
    public static string GetFullFileName(string full_path)
    {
        full_path = RegularPath(full_path);
        int find = -1;
        for (int i = full_path.Length - 1; i >= 0; i--)
        {
            if (full_path[i] == System.IO.Path.DirectorySeparatorChar)
            {
                find = i;
                break;
            }
        }
        if (find == -1) return full_path;
        return full_path.Substring(find + 1, full_path.Length - find - 1);
    }

    /// <summary>
    /// 拡張子を含むファイル名を返す
    /// </summary>
    /// <param name="full_path"></param>
    /// <returns></returns>
    public static void SeprateFileName(string full_file_name, out string file_name, out string file_extention)
    {
        int find = -1;
        for (int i = full_file_name.Length - 1; i >= 0; i--)
        {
            if (full_file_name[i] == '.')
            {
                find = i;
                break;
            }
        }
        if (find == -1)
        {
            file_name = full_file_name;
            file_extention = "";
        }
        else
        {
            file_name = full_file_name.Substring(0, find);
            file_extention = full_file_name.Substring(find + 1, full_file_name.Length - find - 1);
        }
    }
}

