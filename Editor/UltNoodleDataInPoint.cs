﻿#if UNITY_EDITOR
using NoodledEvents;
using System;
using UltEvents;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class UltNoodleDataInPoint : VisualElement
{
    public new class UxmlFactory : UxmlFactory<UltNoodleDataInPoint, UxmlTraits> { }

    public static UltNoodleDataInPoint New(UltNoodleNodeUI node, NoodleDataInput input)
    {
        var o = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ScriptPath.Replace(".cs", ".uxml")).Instantiate().Q<UltNoodleDataInPoint>();
        o.setupInternal(node, input);
        return o;
    }
    public VisualElement HideWhenConnected;
    private VisualElement ConstVis;
    private void setupInternal(UltNoodleNodeUI node, NoodleDataInput input)
    {
        NodeUI = node; SData = input;
        SData.UI = this.Q("ConnectionPoint");
        // Inform the bowl about drags
        SData.UI.RegisterCallback<MouseEnterEvent>(e => SData.HasMouse = true); SData.UI.RegisterCallback<MouseLeaveEvent>(e => SData.HasMouse = false);
        SData.UI.RegisterCallback<MouseDownEvent>(e => { if (e.button == 0) (NodeUI.Bowl.CurHoveredDataInput = SData).UI.CaptureMouse(); });
        SData.UI.RegisterCallback<MouseMoveEvent>(e => node.Bowl.MousePos = node.Bowl.NodeBG.WorldToLocal(e.mousePosition));
        SData.UI.RegisterCallback<MouseUpEvent>(e =>
        { if (SData.UI.HasMouseCapture()) { node.Bowl.ConnectNodes(); SData.UI.ReleaseMouse(); NodeUI.Bowl.CurHoveredDataInput = null; } });
        // bowl's been informed :)
        Label = this.Q<Label>("InputName");
        
        var SysObjEnum = new EnumField("", SData.ConstInput);
        this.Q("System_Object").Add(SysObjEnum);
        Label.text = input.Name;

        if (SData.Type.Type.IsSubclassOf(typeof(UnityEngine.Object)) || SData.Type == typeof(UnityEngine.Object))
        {
            var icon = EditorGUIUtility.ObjectContent(null, SData.Type).image;
            if (icon != null)
            {
                var iconElement = new VisualElement();
                iconElement.style.backgroundImage = new StyleBackground((Texture2D)icon);
                iconElement.name = "Icon";
                iconElement.style.top = 10;
                iconElement.style.width = 11;
                iconElement.style.height = 11;
                Label.parent.Insert(0, iconElement);
            }
        }   
        void d() // onchange lol
        {
            EditorSceneManager.MarkSceneDirty(node.Node.Bowl.gameObject.scene);
        };
        // also add field for it
        VisualElement newField = null;
        if (input.Type.Type.IsSubclassOf(typeof(UnityEngine.Object)) || input.Type == typeof(UnityEngine.Object))
        {
            var newObj = new ObjectField("") { objectType = input.Type };
            newObj.allowSceneObjects = true;
            newObj.value = input.DefaultObject;
            newObj.RegisterValueChangedCallback(obj => { input.DefaultObject = obj.newValue; d(); });
            newField = newObj;
        }
        else

        

        // Data/Value types
        if (input.Type == typeof(int)) ((newField = new IntegerField("") { value = input.DefaultIntValue }) as INotifyValueChanged<int>).RegisterValueChangedCallback(c => { input.DefaultIntValue = c.newValue; d(); });
        else
        if (input.Type == typeof(float)) ((newField = new FloatField("") { value = input.DefaultFloatValue }) as INotifyValueChanged<float>).RegisterValueChangedCallback(c => { input.DefaultFloatValue = c.newValue; d(); });
        else
        if (input.Type == typeof(string)) ((newField = new TextField("") { value = input.DefaultStringValue }) as INotifyValueChanged<string>).RegisterValueChangedCallback(c => { input.DefaultStringValue = c.newValue; d(); });
        else
        if (input.Type == typeof(Vector2)) ((newField = new Vector2Field("") { value = input.DefaultVector2Value }) as INotifyValueChanged<Vector2>).RegisterValueChangedCallback(c => { input.DefaultVector2Value = c.newValue; d(); });
        else
        if (input.Type == typeof(Vector3)) ((newField = new Vector3Field("") { value = input.DefaultVector3Value }) as INotifyValueChanged<Vector3>).RegisterValueChangedCallback(c => { input.DefaultVector3Value = c.newValue; d(); });
        else
        if (input.Type == typeof(Vector4)) ((newField = new Vector4Field("") { value = input.DefaultVector4Value }) as INotifyValueChanged<Vector4>).RegisterValueChangedCallback(c => { input.DefaultVector4Value = c.newValue; d(); });
        else
        if (input.Type == typeof(Quaternion)) ((newField = new Vector4Field("") { value = input.DefaultVector4Value }) as INotifyValueChanged<Vector4>).RegisterValueChangedCallback(c => { input.DefaultVector4Value = c.newValue; d(); });
        else
        if (input.Type == typeof(bool)) ((newField = new Toggle("") { value = input.DefaultBoolValue }) as INotifyValueChanged<bool>).RegisterValueChangedCallback(c => { input.DefaultBoolValue = c.newValue; d(); });
        else
        if (input.Type.Type.IsEnum) ((newField = new EnumField((Enum)Enum.ToObject(input.Type, input.DefaultIntValue))) as INotifyValueChanged<Enum>).RegisterValueChangedCallback((e) => { input.DefaultIntValue = Convert.ToInt32(e.newValue); d(); });
        else
        if (input.Type == typeof(object))
        {
            // system.object
            SysObjEnum.parent.visible = true;
            SysObjEnum.parent.style.visibility = Visibility.Visible;
            SysObjEnum.parent.style.display = DisplayStyle.Flex;
            SysObjEnum.parent.style.top = 5;
            SysObjEnum.parent.style.flexDirection = FlexDirection.Row;
            HideWhenConnected = SysObjEnum.parent;


            void genout(PersistentArgumentType newValue) 
            {
                if (ConstVis != null)
                {
                    ConstVis.parent.Remove(ConstVis);
                    ConstVis = null;
                }
                SData.ConstInput = newValue;
                VisualElement nf = null;
                switch (newValue)
                {
                    case PersistentArgumentType.None:
                        break;
                    case PersistentArgumentType.Bool:
                        ((nf = new Toggle("") { value = input.DefaultBoolValue }) as INotifyValueChanged<bool>).RegisterValueChangedCallback(c => { input.DefaultBoolValue = c.newValue; d(); });
                        break;
                    case PersistentArgumentType.String:
                        ((nf = new TextField("") { value = input.DefaultStringValue }) as INotifyValueChanged<string>).RegisterValueChangedCallback(c => { input.DefaultStringValue = c.newValue; d(); });
                        break;
                    case PersistentArgumentType.Int:
                        ((nf = new IntegerField("") { value = input.DefaultIntValue }) as INotifyValueChanged<int>).RegisterValueChangedCallback(c => { input.DefaultIntValue = c.newValue; d(); });
                        break;
                    case PersistentArgumentType.Enum:
                        break;
                    case PersistentArgumentType.Float:
                        ((nf = new FloatField("") { value = input.DefaultFloatValue }) as INotifyValueChanged<float>).RegisterValueChangedCallback(c => { input.DefaultFloatValue = c.newValue; d(); });
                        break;
                    case PersistentArgumentType.Vector2:
                        ((nf = new Vector2Field("") { value = input.DefaultVector2Value }) as INotifyValueChanged<Vector2>).RegisterValueChangedCallback(c => { input.DefaultVector2Value = c.newValue; d(); });
                        break;
                    case PersistentArgumentType.Vector3:
                        ((nf = new Vector3Field("") { value = input.DefaultVector3Value }) as INotifyValueChanged<Vector3>).RegisterValueChangedCallback(c => { input.DefaultVector3Value = c.newValue; d(); });
                        break;
                    case PersistentArgumentType.Vector4:
                        ((nf = new Vector4Field("") { value = input.DefaultVector4Value }) as INotifyValueChanged<Vector4>).RegisterValueChangedCallback(c => { input.DefaultVector4Value = c.newValue; d(); });
                        break;
                    case PersistentArgumentType.Quaternion:
                        ((nf = new Vector4Field("") { value = input.DefaultVector4Value }) as INotifyValueChanged<Vector4>).RegisterValueChangedCallback(c => { input.DefaultVector4Value = c.newValue; d(); });
                        break;
                    case PersistentArgumentType.Color:
                        break;
                    case PersistentArgumentType.Color32:
                        break;
                    case PersistentArgumentType.Rect:
                        break;
                    case PersistentArgumentType.Object:
                        var newObj = new ObjectField("") { objectType = typeof(UnityEngine.Object) };
                        newObj.allowSceneObjects = true;
                        newObj.value = input.DefaultObject;
                        newObj.RegisterValueChangedCallback(obj => { input.DefaultObject = obj.newValue; d(); });
                        nf = newObj;
                        break;
                    case PersistentArgumentType.Parameter:
                        break;
                    case PersistentArgumentType.ReturnValue:
                        break;
                    default:
                        break;
                }
                if (nf != null)
                {
                    SysObjEnum.parent.Insert(0,nf);
                    ConstVis = nf;
                }
            }
            SysObjEnum.RegisterValueChangedCallback(e => 
            {
                genout((PersistentArgumentType)e.newValue);
            });
            genout(SData.ConstInput);
        }
        else { }

        if (newField != null)
        {
            HideWhenConnected = newField;
            newField.style.top = 5;
            Label.parent.Add(newField);

            
            if (input.Source != null)
            {
                newField.visible = false;
            }
        }

        NodeUI.InPoints.Add(this);
        Line = this.Q("Line");

        NodeUI.InputsElement.Add(this);
    }
    public UltNoodleNodeUI NodeUI;
    public NoodleDataInput SData;

    Label Label;

    public static string ScriptPath
        => s_scriptPath ??= AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:Script {nameof(UltNoodleDataInPoint)}")[0]);
    private static string s_scriptPath;

    VisualElement Line;
    public void UpdateLine() // draw connection lines
    {
        Line.visible = false;
        if (HideWhenConnected != null)
        HideWhenConnected.visible = (SData.Source == null);
        if (SData.Source != null)
        {
            Line.visible = true;
            Vector2 start = Line.parent.WorldToLocal(SData.Source.UI.LocalToWorld(Vector2.zero));
            Vector2 end = Line.parent.WorldToLocal(SData.UI.LocalToWorld(Vector2.zero)) - new Vector2(7, 0);
            Line.style.left = start.x + 8;
            Line.style.top = start.y + 3;
            Line.style.minWidth = Vector2.Distance(start, end);

            //rotat...

            // figure angle, make start 0 0 0
            Vector2 a = end - start;
            Line.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(a.y, a.x));
        }
    }
}
#endif