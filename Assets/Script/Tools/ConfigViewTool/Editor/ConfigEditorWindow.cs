using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Alchemy.Editor;
using Alchemy.Inspector;
using Script.SODataScript.TbConfig;

namespace Script.Tools.ConfigViewTool.Editor
{
    public class ConfigEditorWindow : AlchemyEditorWindow
    {
        [MenuItem("Tools/Config View Tool")]
        public static void ShowWindow()
        {
            GetWindow<ConfigEditorWindow>("Config View Tool");
        }

        [System.NonSerialized]
        private List<ScriptableObject> _cachedConfigs = new List<ScriptableObject>();

        [InlineEditor]
        [LabelText("Current Config")]
        [ReadOnly]
        public ScriptableObject CurrentConfig;

        private MultiColumnListView _tableView;
        private VisualElement _contentContainer;
        private IList _currentDataList;
        private Type _currentDataType;
        private IDictionary _currentDictionary;
        private FieldInfo _dataField;
        
        private ScriptableObject _mainConfig;

        protected override void CreateGUI()
        {
            base.CreateGUI();
            
            var root = rootVisualElement;
            if (root.childCount > 0) root[0].RemoveFromHierarchy();

            var mainContainer = new VisualElement();
            mainContainer.style.flexDirection = FlexDirection.Row;
            mainContainer.style.flexGrow = 1;
            root.Add(mainContainer);

            // Sidebar
            var sidebar = new VisualElement();
            sidebar.style.width = 250;
            sidebar.style.borderRightWidth = 1;
            sidebar.style.borderRightColor = new Color(0.1f, 0.1f, 0.1f);
            sidebar.style.backgroundColor = new Color(0.22f, 0.22f, 0.22f);
            sidebar.style.paddingTop = 5;
            sidebar.style.paddingBottom = 5;
            sidebar.style.paddingLeft = 5;
            sidebar.style.paddingRight = 5;
            mainContainer.Add(sidebar);

            var mainConfigBtn = new Button(SelectMainConfig) { text = "主配置" };
            mainConfigBtn.style.height = 30;
            mainConfigBtn.style.marginBottom = 10;
            mainConfigBtn.style.unityFontStyleAndWeight = FontStyle.Bold;
            sidebar.Add(mainConfigBtn);
            
            var sep = new VisualElement();
            sep.style.height = 1;
            sep.style.backgroundColor = Color.gray;
            sep.style.marginBottom = 10;
            sidebar.Add(sep);

            var header = new Label("配置文件列表");
            header.style.fontSize = 14;
            header.style.unityFontStyleAndWeight = FontStyle.Bold;
            header.style.marginBottom = 5;
            sidebar.Add(header);

            var btnRow = new VisualElement();
            btnRow.style.flexDirection = FlexDirection.Row;
            btnRow.style.marginBottom = 5;
            
            var createBtn = new Button(ShowCreateMenu) { text = "+" };
            createBtn.style.width = 30;
            btnRow.Add(createBtn);

            var refreshBtn = new Button(RefreshUI) { text = "刷新列表" };
            refreshBtn.style.flexGrow = 1;
            btnRow.Add(refreshBtn);
            sidebar.Add(btnRow);

            RefreshConfigList();
            var listView = new ListView();
            listView.itemsSource = _cachedConfigs;
            listView.makeItem = () => 
            {
                var label = new Label();
                label.style.paddingLeft = 5;
                label.style.unityTextAlign = TextAnchor.MiddleLeft;
                return label;
            };
            listView.bindItem = (element, index) => 
            {
                var label = (Label)element;
                if (index >= 0 && index < _cachedConfigs.Count) label.text = _cachedConfigs[index].name;
            };
            listView.selectionType = SelectionType.Single;
            listView.fixedItemHeight = 25;
            listView.style.flexGrow = 1;
            listView.onSelectionChange += OnSelectionChanged;

            if (CurrentConfig != null && IsAbsDicScriptableObject(CurrentConfig))
            {
                int index = _cachedConfigs.IndexOf(CurrentConfig);
                if (index >= 0) listView.SetSelection(index);
            }
            sidebar.Add(listView);

            // Content
            _contentContainer = new VisualElement();
            _contentContainer.style.flexGrow = 1;
            _contentContainer.style.paddingLeft = 10;
            _contentContainer.style.paddingRight = 10;
            _contentContainer.style.paddingTop = 10;
            _contentContainer.style.paddingBottom = 10;
            mainContainer.Add(_contentContainer);

            if (CurrentConfig != null) RebuildContent();
            else _contentContainer.Add(new Label("请选择一个配置文件进行编辑。"));
        }

        private void SelectMainConfig()
        {
            if (_mainConfig == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:SOMainConfig");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _mainConfig = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                }
            }

            if (_mainConfig != null)
            {
                CurrentConfig = _mainConfig;
                var listView = rootVisualElement.Q<ListView>();
                if (listView != null) listView.ClearSelection();
                RebuildContent();
            }
            else Debug.LogWarning("MainConfig not found!");
        }

        private void ShowCreateMenu()
        {
            var menu = new GenericMenu();
            var types = TypeCache.GetTypesDerivedFrom(typeof(ScriptableObject));
            foreach (var type in types)
            {
                if (type.IsAbstract) continue;
                if (IsAbsDicScriptableObject(type)) menu.AddItem(new GUIContent(type.Name), false, () => CreateConfig(type));
            }
            menu.ShowAsContext();
        }

        private void CreateConfig(Type type)
        {
            string path = "Assets/Resources/SOData/ConfigUitlity";
            if (!AssetDatabase.IsValidFolder(path))
            {
                System.IO.Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            string filename = "New" + type.Name.Replace("SO", "") + ".asset";
            string fullPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + filename);
            var asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshUI();
            CurrentConfig = asset;
            var listView = rootVisualElement.Q<ListView>();
            int index = _cachedConfigs.IndexOf(asset);
            if (index >= 0) listView.SetSelection(index);
        }

        private void RefreshUI()
        {
            RefreshConfigList();
            var listView = rootVisualElement.Q<ListView>();
            if (listView != null)
            {
                listView.itemsSource = _cachedConfigs;
                listView.Rebuild();
            }
        }

        private void OnSelectionChanged(IEnumerable<object> selected)
        {
            var item = selected.FirstOrDefault() as ScriptableObject;
            if (item != null && item != CurrentConfig)
            {
                CurrentConfig = item;
                RebuildContent();
            }
        }

        private void RefreshConfigList()
        {
            _cachedConfigs.Clear();
            string path = "Assets/Resources/SOData/ConfigUitlity";
            if (!AssetDatabase.IsValidFolder(path)) return;
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { path });
            foreach(var guid in guids)
            {
                var pathAsset = AssetDatabase.GUIDToAssetPath(guid);
                var so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(pathAsset);
                if(so != null && IsAbsDicScriptableObject(so)) _cachedConfigs.Add(so);
            }
            _cachedConfigs.Sort((a,b) => string.Compare(a.name, b.name));
        }

        private bool IsAbsDicScriptableObject(ScriptableObject so) => IsAbsDicScriptableObject(so.GetType());

        private bool IsAbsDicScriptableObject(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(AbsDicScriptableObjectBase<>)) return true;
                type = type.BaseType;
            }
            return false;
        }

        private void RebuildContent()
        {
            _contentContainer.Clear();
            if (CurrentConfig == null) return;

            var toolbar = new VisualElement();
            toolbar.style.flexDirection = FlexDirection.Row;
            toolbar.style.marginBottom = 10;
            var title = new Label(CurrentConfig.name);
            title.style.fontSize = 18;
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.marginRight = 20;
            toolbar.Add(title);

            if (IsAbsDicScriptableObject(CurrentConfig))
            {
                toolbar.Add(new Button(SaveCurrentConfig) { text = "保存配置" });
                toolbar.Add(new Button(AddNewItem) { text = "添加条目" });
                toolbar.Add(new Button(RemoveSelectedItem) { text = "移除选中" });
                _contentContainer.Add(toolbar);
                BuildTableView();
            }
            else
            {
                toolbar.Add(new Button(SaveCurrentConfig) { text = "保存配置" });
                _contentContainer.Add(toolbar);
                var inspector = new UnityEditor.UIElements.InspectorElement(CurrentConfig);
                _contentContainer.Add(inspector);
            }
        }

        private void BuildTableView()
        {
            if (!ExtractData(CurrentConfig, out _currentDictionary, out _currentDataType, out _currentDataList))
            {
                _contentContainer.Add(new Label("无法从该配置中提取数据。"));
                return;
            }

            _tableView = new MultiColumnListView();
            _tableView.itemsSource = _currentDataList;
            _tableView.style.flexGrow = 1;

            var idColumn = new Column();
            idColumn.name = "Key (ID)";
            idColumn.title = "键 (ID)";
            idColumn.width = 80;
            idColumn.makeCell = () => new IntegerField();
            idColumn.bindCell = (e, i) => 
            {
                if (i >= 0 && i < _currentDataList.Count)
                {
                    var obj = _currentDataList[i];
                    var idField = _currentDataType.GetField("Id");
                    if (idField != null)
                    {
                        var intField = (IntegerField)e;
                        int currentId = (int)idField.GetValue(obj);
                        intField.SetValueWithoutNotify(currentId);
                        
                        bool isKeyValid = _currentDictionary.Contains(currentId) && _currentDictionary[currentId] == obj;
                        intField.style.backgroundColor = isKeyValid ? StyleKeyword.Null : new Color(1, 0, 0, 0.3f);

                        intField.RegisterValueChangedCallback(evt => 
                        {
                            int newKey = evt.newValue;
                            if (_currentDictionary.Contains(newKey))
                            {
                                intField.SetValueWithoutNotify(evt.previousValue);
                                Debug.LogError($"Key {newKey} already exists!");
                                return;
                            }

                            int oldKey = evt.previousValue;
                            if (_currentDictionary.Contains(oldKey)) _currentDictionary.Remove(oldKey);
                            _currentDictionary[newKey] = obj;
                            
                            idField.SetValue(obj, newKey);
                            MarkDirty();
                        });
                    }
                }
            };
            _tableView.columns.Add(idColumn);

            var fields = _currentDataType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.Name == "Id") continue;

                var column = new Column();
                column.name = field.Name;
                column.title = ObjectNames.NicifyVariableName(field.Name);
                column.width = 150;
                column.makeCell = () => CreateCellElement(field.FieldType);
                column.bindCell = (e, i) => BindCellElement(e, i, field);
                _tableView.columns.Add(column);
            }
            _contentContainer.Add(_tableView);
        }

        private VisualElement CreateCellElement(Type type)
        {
            if (type == typeof(int)) { var f = new IntegerField(); f.isDelayed = true; return f; }
            if (type == typeof(float)) { var f = new FloatField(); f.isDelayed = true; return f; }
            if (type == typeof(string)) { var f = new TextField(); f.isDelayed = true; return f; }
            if (type == typeof(bool)) return new Toggle();
            if (type.IsEnum) return new EnumField(Activator.CreateInstance(type) as Enum);
            if (typeof(UnityEngine.Object).IsAssignableFrom(type)) { var f = new ObjectField(); f.objectType = type; return f; }
            return new Button() { text = "编辑" };
        }

        private void BindCellElement(VisualElement element, int index, FieldInfo fieldInfo)
        {
            if (index < 0 || index >= _currentDataList.Count) return;
            var dataObj = _currentDataList[index];
            var val = fieldInfo.GetValue(dataObj);

            if (element is IntegerField intField)
            {
                intField.SetValueWithoutNotify((int)val);
                intField.RegisterValueChangedCallback(evt => { fieldInfo.SetValue(dataObj, evt.newValue); MarkDirty(); });
            }
            else if (element is FloatField floatField)
            {
                floatField.SetValueWithoutNotify((float)val);
                floatField.RegisterValueChangedCallback(evt => { fieldInfo.SetValue(dataObj, evt.newValue); MarkDirty(); });
            }
            else if (element is TextField textField)
            {
                textField.SetValueWithoutNotify((string)val);
                textField.RegisterValueChangedCallback(evt => { fieldInfo.SetValue(dataObj, evt.newValue); MarkDirty(); });
            }
            else if (element is Toggle toggle)
            {
                toggle.SetValueWithoutNotify((bool)val);
                toggle.RegisterValueChangedCallback(evt => { fieldInfo.SetValue(dataObj, evt.newValue); MarkDirty(); });
            }
            else if (element is EnumField enumField)
            {
                enumField.Init((Enum)val);
                enumField.RegisterValueChangedCallback(evt => { fieldInfo.SetValue(dataObj, evt.newValue); MarkDirty(); });
            }
            else if (element is ObjectField objField)
            {
                objField.SetValueWithoutNotify((UnityEngine.Object)val);
                objField.RegisterValueChangedCallback(evt => { fieldInfo.SetValue(dataObj, evt.newValue); MarkDirty(); });
            }
            else if (element is Button btn)
            {
                btn.clicked += () => NestedEditorWindow.Open(dataObj, fieldInfo.Name, () => {
                    MarkDirty();
                    _tableView?.RefreshItems();
                });
            }
        }

        private bool ExtractData(ScriptableObject config, out IDictionary dictionary, out Type dataType, out IList list)
        {
            dictionary = null;
            dataType = null;
            list = null;
            Type type = config.GetType();
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(AbsDicScriptableObjectBase<>))
                {
                    dataType = type.GetGenericArguments()[0];
                    _dataField = type.GetField("_data", BindingFlags.Public | BindingFlags.Instance);
                    if (_dataField != null)
                    {
                        dictionary = _dataField.GetValue(config) as IDictionary;
                        if (dictionary != null)
                        {
                            var listType = typeof(List<>).MakeGenericType(dataType);
                            list = (IList)Activator.CreateInstance(listType);
                            foreach (var val in dictionary.Values) list.Add(val);
                            return true;
                        }
                    }
                }
                type = type.BaseType;
            }
            return false;
        }

        private void SaveCurrentConfig()
        {
            if (CurrentConfig != null)
            {
                EditorUtility.SetDirty(CurrentConfig);
                AssetDatabase.SaveAssets();
                var method = CurrentConfig.GetType().GetMethod("SaveConfig");
                if (method != null) method.Invoke(CurrentConfig, null);
                Debug.Log($"Saved {CurrentConfig.name}");
            }
        }

        private void MarkDirty() { if (CurrentConfig != null) EditorUtility.SetDirty(CurrentConfig); }

        private void AddNewItem()
        {
            if (_currentDictionary == null || _currentDataType == null) return;
            int newId = 0;
            if (_currentDictionary.Count > 0)
            {
                int maxId = -1;
                foreach(var key in _currentDictionary.Keys) if (key is int k && k > maxId) maxId = k;
                newId = maxId + 1;
            }
            var newItem = Activator.CreateInstance(_currentDataType);
            var idField = _currentDataType.GetField("Id");
            if (idField != null) idField.SetValue(newItem, newId);
            _currentDictionary.Add(newId, newItem);
            _currentDataList.Add(newItem);
            _tableView.RefreshItems();
            MarkDirty();
        }

        private void RemoveSelectedItem()
        {
             if (_tableView.selectedIndex < 0 || _tableView.selectedIndex >= _currentDataList.Count) return;
             var itemToRemove = _currentDataList[_tableView.selectedIndex];
             var idField = _currentDataType.GetField("Id");
             if (idField != null)
             {
                 int id = (int)idField.GetValue(itemToRemove);
                 if (_currentDictionary.Contains(id)) _currentDictionary.Remove(id);
             }
             _currentDataList.RemoveAt(_tableView.selectedIndex);
             _tableView.RefreshItems();
             MarkDirty();
        }
    }

    public class NestedEditorWindow : EditorWindow
    {
        private class NavState { public object target; public string title; }
        private Stack<NavState> _navStack = new Stack<NavState>();
        private Action _onChange;
        private Vector2 _scroll;
        private Vector2 _listScroll;
        private Vector2 _detailScroll;
        private int _selectedListIndex = -1;

        public static NestedEditorWindow Open(object target, string title, Action onChange)
        {
            if (HasOpenInstances<NestedEditorWindow>())
            {
                var w = GetWindow<NestedEditorWindow>();
                w.titleContent = new GUIContent(title);
                w._navStack.Clear();
                w._navStack.Push(new NavState{ target = target, title = title });
                w._onChange = onChange;
                w.Show();
                return w;
            }
            var win = CreateInstance<NestedEditorWindow>();
            win.titleContent = new GUIContent(title);
            win._navStack.Push(new NavState{ target = target, title = title });
            win._onChange = onChange;
            win.ShowUtility();
            return win;
        }

        private void Push(object target, string title)
        {
            _navStack.Push(new NavState{ target = target, title = title });
            _selectedListIndex = -1;
        }

        private void Pop()
        {
            if (_navStack.Count > 1) _navStack.Pop();
            _selectedListIndex = -1;
        }

        private void OnGUI()
        {
            if (_navStack.Count == 0) { Close(); return; }
            var current = _navStack.Peek();

            EditorGUILayout.BeginHorizontal("Toolbar");
            if (GUILayout.Button("返回", EditorStyles.toolbarButton, GUILayout.Width(50)) && _navStack.Count > 1) Pop();
            GUILayout.Label(string.Join(" > ", _navStack.Select(x => x.title).Reverse()), EditorStyles.toolbarButton);
            EditorGUILayout.EndHorizontal();

            if (current.target is IList list)
            {
                EditorGUI.BeginChangeCheck();
                DrawSplitViewList(list);
                if (EditorGUI.EndChangeCheck()) _onChange?.Invoke();
            }
            else
            {
                _scroll = EditorGUILayout.BeginScrollView(_scroll);
                EditorGUI.BeginChangeCheck();
                DrawFields(current.target);
                if (EditorGUI.EndChangeCheck()) _onChange?.Invoke();
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawSplitViewList(IList list)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.BeginVertical("box", GUILayout.Width(200));
            if (GUILayout.Button("添加条目"))
            {
                Type itemType = list.GetType().IsGenericType ? list.GetType().GetGenericArguments()[0] : typeof(object);
                object newItem = itemType == typeof(string) ? "" : Activator.CreateInstance(itemType);
                list.Add(newItem);
                _selectedListIndex = list.Count - 1;
                _onChange?.Invoke();
            }

            _listScroll = EditorGUILayout.BeginScrollView(_listScroll, GUILayout.ExpandHeight(true));
            for (int i = 0; i < list.Count; i++)
            {
                var rect = EditorGUILayout.GetControlRect(false, 20);
                if (i == _selectedListIndex) EditorGUI.DrawRect(rect, new Color(0.24f, 0.37f, 0.58f));
                
                if (GUI.Button(new Rect(rect.x, rect.y, rect.width - 55, rect.height), $"Element {i}", EditorStyles.label))
                {
                    _selectedListIndex = i;
                    GUI.FocusControl(null);
                }
                
                if (GUI.Button(new Rect(rect.x + rect.width - 50, rect.y, 50, rect.height), "删除"))
                {
                    list.RemoveAt(i);
                    _onChange?.Invoke();
                    if (_selectedListIndex >= list.Count) _selectedListIndex = list.Count - 1;
                    break;
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            _detailScroll = EditorGUILayout.BeginScrollView(_detailScroll);
            if (_selectedListIndex >= 0 && _selectedListIndex < list.Count)
            {
                var item = list[_selectedListIndex];
                if (item == null) EditorGUILayout.LabelField("null");
                else if (IsSimpleType(item.GetType()))
                {
                    var newVal = DrawSimpleField(item, item.GetType(), "值");
                    if (!object.Equals(newVal, item)) list[_selectedListIndex] = newVal;
                }
                else DrawFields(item);
            }
            else
            {
                EditorGUILayout.HelpBox("请选择一个条目", MessageType.Info);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFields(object obj)
        {
            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var val = field.GetValue(obj);
                if (IsSimpleType(field.FieldType))
                {
                    var newVal = DrawSimpleField(val, field.FieldType, field.Name);
                    if (!object.Equals(newVal, val)) field.SetValue(obj, newVal);
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(field.Name);
                    if (val == null)
                    {
                        if (GUILayout.Button("创建"))
                        {
                            val = Activator.CreateInstance(field.FieldType);
                            field.SetValue(obj, val);
                            _onChange?.Invoke();
                        }
                    }
                    else
                    {
                        string btnText = val is IList l ? $"列表 ({l.Count})" : "打开";
                        if (GUILayout.Button(btnText)) Push(val, field.Name);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private bool IsSimpleType(Type t) => t.IsPrimitive || t == typeof(string) || t.IsEnum || typeof(UnityEngine.Object).IsAssignableFrom(t);

        private object DrawSimpleField(object val, Type t, string label)
        {
            if (t == typeof(int)) return EditorGUILayout.IntField(label, (int)val);
            if (t == typeof(float)) return EditorGUILayout.FloatField(label, (float)val);
            if (t == typeof(string)) return EditorGUILayout.TextField(label, (string)val);
            if (t == typeof(bool)) return EditorGUILayout.Toggle(label, (bool)val);
            if (t.IsEnum) return EditorGUILayout.EnumPopup(label, (Enum)val);
            if (typeof(UnityEngine.Object).IsAssignableFrom(t)) return EditorGUILayout.ObjectField(label, (UnityEngine.Object)val, t, false);
            return val;
        }
    }
}
