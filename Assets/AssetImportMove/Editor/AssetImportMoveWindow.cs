using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetImportMoveWindow : EditorWindow
{

    [MenuItem("Window/Asset Import Move Setting")]
    public static void ShowWindow()
    {
        GetWindow<AssetImportMoveWindow>("Asset Import Move");

    }

    //選択が変更されるたび呼び出されます
    void OnSelectionChange()
    {
        if (data != null)
        {
            data.ResetAllInfo();

            //インスペクター再描画
            Repaint();
        }
    }

    //ヒエラルキーのオブジェクトまたはオブジェクトのグループが変更されたとき
    void OnHierarchyChange()
    {
        if (data != null)
        {
            data.ResetAllInfo();
            Repaint();
        }
    }

    //プロジェクトが変更されるたび
    void OnProjectChange()
    {
        if (data != null)
        {
            data.ResetAllInfo();
            Repaint();
        }
    }

    class FavoritesTabAssetPostprocessor : AssetPostprocessor
    {
        //任意の数のアセットのインポートが完了した後
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (deletedAssets.Length > 0 || movedAssets.Length > 0)
            {
                LoadAll();
            }

            Debug.Log("importedAssets"+importedAssets.Length+ "deletedAssets" + deletedAssets.Length+ "movedAssets" + movedAssets.Length + "movedFromAssetPaths" + movedFromAssetPaths.Length );
            if (deletedAssets.LongLength > 0 || movedAssets.LongLength > 0 || movedFromAssetPaths.LongLength > 0)
            {
                return;
            }

            string path = "";

            // インポートされたものリストをチェック
            foreach (string assetSpath in importedAssets)
            {
                var extension = System.IO.Path.GetExtension(assetSpath);
                if (extension != "")
                {
                    continue;
                }

                if (data == null || data.datas.Count == 0)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(path) || path.Length > assetSpath.Length)
                {
                    path = assetSpath;
                }
            }

            Debug.Log(path);
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                Directory.Move(Path.GetFullPath(path), Path.GetFullPath(data.datas[0].path) + "/" + System.IO.Path.GetFileNameWithoutExtension(path));
            }
        }
    }

    public static void LoadAll()
    {
        if (data == null) return;

        data.ResetAllInfo();
        EditorUtility.SetDirty(data);
    }

    int selected_item_index = -1;
    int drag_item_index = -1;
    Vector2 mouse_down_pos;

    bool drag_from_self;

    static AssetImportMoveData data;

    void Init()
    {
        if (data == null)
        {
            // このスクリプトファイルのディレクトリパスの取得
            var mono = MonoScript.FromScriptableObject(this);
            var filePath = AssetDatabase.GetAssetPath(mono);
            string directoryPath = System.IO.Path.GetDirectoryName(filePath);

            string path = directoryPath + "/assetImportMoveSetting.asset";

            //dataアセットのロード
            AssetImportMoveData data = AssetDatabase.LoadAssetAtPath(path, typeof(AssetImportMoveData)) as AssetImportMoveData;
            if (data == null)
            {
                //ScriptableObjectの作成
                data = CreateInstance<AssetImportMoveData>();
                //アセットの保存
                AssetDatabase.CreateAsset(data, path);
                AssetDatabase.Refresh();
            }
            data.ResetAllInfo();
            AssetImportMoveWindow.data = data;
        }
    }

    void OnGUI()
    {
        Init();

        DrawHead();

        Event current = Event.current;

        bool repaint = false;
        bool list_modified = false;

        // Windowの中でドラッグ中
        if (current.type == EventType.DragUpdated)
        {
            if (drag_from_self)
            {
                //ドラッグ表示変更（四角）
                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            }
            else
            {
                if (DragAndDrop.objectReferences.Length == 1)
                {
                    //ドラッグ表示変更（矢印）
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                }
                else
                {
                    //ドラッグ表示変更（バツ）
                    DragAndDrop.visualMode = DragAndDropVisualMode.None;
                    return;
                }
            }
        }

        // AssetImportMoveWindowのアイテムをドラッグし始めた時
        if (current.type == EventType.MouseDrag && drag_item_index >= 0 && drag_item_index < data.datas.Count)
        {
            AssetImportMoveData.Item it = data.datas[drag_item_index];

            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = new Object[1] { it.obj };
            DragAndDrop.StartDrag(it.name);

            drag_from_self = true;
            drag_item_index = -1;
        }

        EditorGUIUtility.SetIconSize(new Vector2(24f, 24f));

        // dataが無いとき
        if (data.datas.Count == 0)
        {
            GUILayout.Label("ここにフォルダをドラッグ");

            // ドラッグ＆ドロップした
            if (Event.current.type == EventType.DragPerform)
            {
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    var filePath = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[i]);
                    var extension = System.IO.Path.GetExtension(filePath);
                    if (extension == "")
                    {
                        // ドラッグしたオブジェクト追加
                        data.AddLast(DragAndDrop.objectReferences[i]);
                    }
                }

                repaint = true;
                drag_from_self = false;
            }
        }
        else
        {
            for (int index = 0; index < data.datas.Count; index++)
            {
                AssetImportMoveData.Item item = data.datas[index];
                if (item.obj == null) continue;

                //表示
                GUILayout.Label(item.gui_content);

                Rect rt = GUILayoutUtility.GetLastRect();

                if (current.type == EventType.MouseDown)
                {
                    if (rt.Contains(current.mousePosition))
                    {
                        selected_item_index = index;
                        mouse_down_pos = current.mousePosition;
                        drag_item_index = index;
                    }
                }
                else if (current.type == EventType.MouseUp)
                {
                    drag_item_index = -1;
                    
                    // 中クリック
                    if (current.button == 2 && index == selected_item_index)
                    {
                        if (rt.Contains(current.mousePosition))
                        {
                            data.RemoveAt(index);
                            repaint = true;
                            break;
                        }
                    }
                }

                // ドラッグ＆ドロップした
                if (Event.current.type == EventType.DragPerform)
                {
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        var filePath = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[i]);
                        var extension = System.IO.Path.GetExtension(filePath);
                        if (extension == "")
                        {
                            data.RemoveAt(index);
                            // ドラッグしたオブジェクト追加
                            data.AddLast(DragAndDrop.objectReferences[i]);
                        }
                    }
                    list_modified = true;
                    repaint = true;
                    drag_from_self = false;
                }
            }
        }

        GUILayout.Space(20);

        if (data.datas.Count != 0)
        {
            AssetImportMoveData.Item it = data.datas[0];
            string path = it.path;
            string asset_head = "Assets";

            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;

            if (path.StartsWith(asset_head)) path = path.Substring(asset_head.Length, path.Length - asset_head.Length);
            if (path == "") path = it.name;

            GUI.Label(detial_rc, path, style);
        }

        // 最終更新日
        if (list_modified)
        {
            selected_item_index = -1;
            data.ResetAllInfo();
            EditorUtility.SetDirty(data);
        }

        //塗り直す
        if (repaint || list_modified)
        {
            Repaint();
        }
    }

    Rect detial_rc;
    void DrawHead()
    {
        GUILayout.Space(10f);
        GUILayout.Label("");
        detial_rc = GUILayoutUtility.GetLastRect();

        Rect rt = detial_rc;
        rt.xMin = rt.xMax - 40f;
        if (GUI.Button(rt, "help"))
        {
            GetWindow<HelpWindow>("Asset Import Move");
        }
    }
}

class HelpWindow : EditorWindow
{
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;

        GUILayout.Label("操作説明:");

        GUILayout.Label("<color=yellow>オブジェクトをウィンドウにドラッグ </color>: アイテムを追加", style);
        GUILayout.Label("<color=yellow>中クリック</color>: アイテムを削除", style);
    }
}