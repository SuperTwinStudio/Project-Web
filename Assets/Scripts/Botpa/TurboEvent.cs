using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Botpa {

    [Serializable]
    public class TurboEvent {

        //Target
        #if UNITY_EDITOR
        [SerializeField] private UnityEngine.Object targetOld;
        [SerializeField] private UnityEngine.Object target;
        #endif

        //Script & method
        [SerializeField] private UnityEngine.Object script;
        [SerializeField] private string method;

        //Parameters
        [SerializeField] private TurboParameter[] parameters = new TurboParameter[0];
        
        public Type[] parameterTypes {
            get {
                Type[] types = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++) types[i] = parameters[i].value.type;
                return types;
            }
        }


        ///<summary>
        ///Invokes the event by calling its function with the specified parameters.
        ///</summary>
        public void Invoke() {
            //Invalid script
            if (!script || method.Equals("")) return;

            //Get arguments
            object[] arguments = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++) arguments[i] = parameters[i].value.GetValue();

            //Run method
            MethodInfo methodInfo = script.GetType().GetMethod(method, parameterTypes);
            methodInfo.Invoke(script, arguments);
        }



        //Parameters & Values
        [Serializable]
        private class TurboParameter {

            [SerializeField] private string _name;
            [SerializeField] private TurboValue _value;

            public string name => _name;
            public TurboValue value => _value;


            public TurboParameter(string name, TurboValue value) {
                _name = name;
                _value = value;
            }

        }

        [Serializable]
        private class TurboValue {

            //Serialized type & value
            [SerializeField] private string typeName;
            [SerializeField] private string serializedValue;
            [SerializeField] private UnityEngine.Object serializedObjectValue;

            public Type type => Type.GetType(typeName);


            //Constructor
            public TurboValue(Type type) {
                //Save type
                typeName = type.AssemblyQualifiedName;

                //Check if type is supported
                if (!IsValid(type)) throw new ArgumentException($"Type '{type.Name}' is not supported");

                //Save value
                SetValue(GetDefaultValue(type));
            }

            //Valid types
            public static bool IsValid(Type type) {
                //Check if the type is valid
                return 
                    type.IsEnum ||
                    IsObjectType(type) ||
                    IsParsableType(type) ||
                    type == typeof(Vector2) ||
                    type == typeof(Vector3) ||
                    type == typeof(Vector4) ||
                    type == typeof(Vector2Int) ||
                    type == typeof(Vector3Int) ||
                    type == typeof(Color) ||
                    type == typeof(LayerMask);/* ||
                    type == typeof(Gradient) ||
                    type == typeof(Rect);*/
            }
            
            public static bool IsObjectType(Type type) {
                //Check if the type is unity object
                return type.IsSubclassOf(typeof(UnityEngine.Object));
            }
            
            public static bool IsParsableType(Type type) {
                return type == typeof(string) || type.GetMethod("Parse", new[] { typeof(string) }) != null;
            }

            //Get value
            private static object GetDefaultValue(Type type) {
                if (IsObjectType(type)) {
                    //Unity objects
                    return null;
                } else if (type == typeof(string)) {
                    //Init as default string
                    return "";
                } else if (type == typeof(LayerMask)) {
                    //Init as default layer
                    return ~0;
                } else {
                    //Other types
                    return Activator.CreateInstance(type);
                }
            }

            private static object ParseValueFromString(Type type, string serializedValue) {
                //Type is string -> No need to do anything
                if (type == typeof(string)) return serializedValue;

                //Try to parse value
                try {
                    return type.GetMethod("Parse", new[] { typeof(string) }).Invoke(null, new[] { serializedValue });
                } catch {
                    return GetDefaultValue(type);
                }
            }

            private object GetValue(Type type) {
                //Enums
                if (type.IsEnum) return Enum.Parse(type, serializedValue);

                //Unity objects
                if (IsObjectType(type)) return serializedObjectValue;

                //String & value types
                if (IsParsableType(type)) return ParseValueFromString(type, serializedValue);

                //Other (Special)
                if (type == typeof(LayerMask)) return (LayerMask) int.Parse(serializedValue);

                //Other
                return JsonUtility.FromJson(serializedValue, type);
            }

            private T GetValue<T>(Type type) {
                return (T) GetValue(type);
            }

            public object GetValue() {
                return GetValue(type);
            }
            
            public T GetValue<T>() {
                return (T) GetValue(type);
            }

            //Set value
            public void SetValue(object value) {
                if (IsObjectType(type)) {
                    //Unity objects
                    serializedObjectValue = (UnityEngine.Object) value;
                } else {
                    //Other types
                    serializedValue = SerializeValue(type, value);
                }
            }

            public static string SerializeValue(Type type, object value) {
                //Empty object
                if (value == null) return "";

                //Type has parse string function -> Do to string
                if (type.IsEnum || IsParsableType(type)) return value.ToString();

                //Other (Special)
                if (type == typeof(LayerMask)) return value.ToString();

                //Other (Serialize with JSON)
                return JsonUtility.ToJson(value); 
            }

            //Inspector
            #if UNITY_EDITOR
            public object CreateField(Rect rect, GUIContent name) {
                //Get type
                Type type = this.type;

                //Enums
                if (type.IsEnum) return EditorGUI.EnumPopup(rect, name, GetValue<Enum>());

                //Unity objects
                if (IsObjectType(type)) return EditorGUI.ObjectField(rect, name, GetValue<UnityEngine.Object>(type), type, true);

                //String & value types
                if (type == typeof(string)) return EditorGUI.TextField(rect, name, GetValue<string>());
                if (type == typeof(int)) return EditorGUI.IntField(rect, name, GetValue<int>());
                if (type == typeof(bool)) return EditorGUI.Toggle(rect, name, GetValue<bool>());
                if (type == typeof(float)) return EditorGUI.FloatField(rect, name, GetValue<float>());
                if (type == typeof(double)) return EditorGUI.DoubleField(rect, name, GetValue<double>());
                if (type == typeof(long)) return EditorGUI.LongField(rect, name, GetValue<long>());

                //Other
                if (type == typeof(Vector2)) return EditorGUI.Vector2Field(rect, name, GetValue<Vector2>());
                if (type == typeof(Vector3)) return EditorGUI.Vector3Field(rect, name, GetValue<Vector3>());
                if (type == typeof(Vector4)) return EditorGUI.Vector4Field(rect, name, GetValue<Vector4>());
                if (type == typeof(Vector2Int)) return EditorGUI.Vector2IntField(rect, name, GetValue<Vector2Int>());
                if (type == typeof(Vector3Int)) return EditorGUI.Vector3IntField(rect, name, GetValue<Vector3Int>());
                if (type == typeof(Color)) return EditorGUI.ColorField(rect, name, GetValue<Color>());
                if (type == typeof(LayerMask)) return EditorGUI.LayerField(rect, name, GetValue<LayerMask>());
                //if (type == typeof(Gradient)) return EditorGUI.GradientField(rect, name, GetValue<Gradient>()); //Aparently not serializable or something
                //if (type == typeof(Rect)) return EditorGUI.RectField(rect, name, GetValue<Rect>()); //Two lines so me da pereza cambiarlo todo 😽
                
                //Not found -> Invalid type (show parameter name)
                EditorGUI.LabelField(rect, name);
                return null;
            }
            #endif

        }



         /*$$$$$                                                     /$$
        |_  $$_/                                                    | $$
          | $$   /$$$$$$$   /$$$$$$$  /$$$$$$   /$$$$$$   /$$$$$$$ /$$$$$$    /$$$$$$   /$$$$$$
          | $$  | $$__  $$ /$$_____/ /$$__  $$ /$$__  $$ /$$_____/|_  $$_/   /$$__  $$ /$$__  $$
          | $$  | $$  \ $$|  $$$$$$ | $$  \ $$| $$$$$$$$| $$        | $$    | $$  \ $$| $$  \__/
          | $$  | $$  | $$ \____  $$| $$  | $$| $$_____/| $$        | $$ /$$| $$  | $$| $$
         /$$$$$$| $$  | $$ /$$$$$$$/| $$$$$$$/|  $$$$$$$|  $$$$$$$  |  $$$$/|  $$$$$$/| $$
        |______/|__/  |__/|_______/ | $$____/  \_______/ \_______/   \___/   \______/ |__/
                                    | $$
                                    | $$
                                    |_*/

        //Event inspector
        #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(TurboEvent))]
        private class TurboEventInspector : PropertyDrawer {

            //Reset from enum
            private enum From {
                Target,
                Script,
                Method,
            }

            //Container size
            private float lineX = 0f;
            private float lineWidth = 0f;
            private const float lineHeight = 18f;


            //Property drawer
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
                //Calculate height
                SerializedProperty parametersProperty = property.FindPropertyRelative("parameters");
                return lineHeight * 2 + TurboParameterInspector.lineHeight * parametersProperty.arraySize;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                //Get info
                lineX = position.x;
                lineWidth = position.width;

                //Event
                var turboEvent = (TurboEvent) property.boxedValue;

                //Target(s)
                Rect targetRect = new(lineX, position.y, lineWidth, lineHeight);
                SerializedProperty targetProperty = property.FindPropertyRelative("target");
                SerializedProperty targetOldProperty = property.FindPropertyRelative("targetOld");
                UnityEngine.Object target = targetProperty.objectReferenceValue;
                UnityEngine.Object targetOld = targetOldProperty.objectReferenceValue;

                //Script
                Rect scriptRect = new(lineX, targetRect.y + lineHeight, lineWidth / 2, lineHeight);
                SerializedProperty scriptProperty = property.FindPropertyRelative("script");
                UnityEngine.Object script = (UnityEngine.Object) scriptProperty.boxedValue;

                //Method
                Rect methodRect = new(lineX + lineWidth / 2, scriptRect.y, lineWidth / 2, lineHeight);
                SerializedProperty methodProperty = property.FindPropertyRelative("method");
                string method = methodProperty.stringValue;

                //Parameters & arguments
                SerializedProperty parametersProperty = property.FindPropertyRelative("parameters");
                Type[] parameterTypes = turboEvent.parameterTypes;
                


                 /*$$$$$$$ /$$                              
                | $$_____/|__/                              
                | $$       /$$ /$$   /$$  /$$$$$$   /$$$$$$$
                | $$$$$   | $$|  $$ /$$/ /$$__  $$ /$$_____/
                | $$__/   | $$ \  $$$$/ | $$$$$$$$|  $$$$$$ 
                | $$      | $$  >$$  $$ | $$_____/ \____  $$
                | $$      | $$ /$$/\  $$|  $$$$$$$ /$$$$$$$/
                |__/      |__/|__/  \__/ \_______/|______*/ 

                //Target changed -> Reset info
                if (target != targetOld) Reset(property, From.Target);

                //No script but has method -> Reset info
                if (script == null && method != "") Reset(property, From.Method);

                //Has script but method does not exist -> Reset info
                if (script != null && method != "" && script.GetType().GetMethod(method, parameterTypes) == null) Reset(property, From.Method);
        


                 /*$$$$$$$                                        /$$    
                |__  $$__/                                       | $$    
                   | $$  /$$$$$$   /$$$$$$   /$$$$$$   /$$$$$$  /$$$$$$  
                   | $$ |____  $$ /$$__  $$ /$$__  $$ /$$__  $$|_  $$_/  
                   | $$  /$$$$$$$| $$  \__/| $$  \ $$| $$$$$$$$  | $$    
                   | $$ /$$__  $$| $$      | $$  | $$| $$_____/  | $$ /$$
                   | $$|  $$$$$$$| $$      |  $$$$$$$|  $$$$$$$  |  $$$$/
                   |__/ \_______/|__/       \____  $$ \_______/   \___/  
                                            /$$  \ $$                    
                                           |  $$$$$$/                    
                                            \_____*/   

                //Begin
                EditorGUI.BeginProperty(position, label, property);                  
                
                //Target field
                position.height += lineHeight;
                EditorGUI.PropertyField(targetRect, targetProperty, GUIContent.none);



                  /*$$$$$                      /$$             /$$    
                 /$$__  $$                    |__/            | $$    
                | $$  \__/  /$$$$$$$  /$$$$$$  /$$  /$$$$$$  /$$$$$$  
                |  $$$$$$  /$$_____/ /$$__  $$| $$ /$$__  $$|_  $$_/  
                 \____  $$| $$      | $$  \__/| $$| $$  \ $$  | $$    
                 /$$  \ $$| $$      | $$      | $$| $$  | $$  | $$ /$$
                |  $$$$$$/|  $$$$$$$| $$      | $$| $$$$$$$/  |  $$$$/
                 \______/  \_______/|__/      |__/| $$____/    \___/  
                                                  | $$                
                                                  | $$                
                                                  |_*/                
                
                if (EditorGUI.DropdownButton(scriptRect, new GUIContent(GetScriptName(script)), FocusType.Passive)) {
                    //Missing target
                    if (!target) return;

                    //Select script
                    Component[] components;
                    if (target is GameObject @object) {
                        components = @object.GetComponents<Component>();
                    } else {
                        components = ((Component) target).GetComponents<Component>();
                    }

                    //Create menu
                    GenericMenu menu = new();
                    for (int i = 0; i < components.Length; i++) {
                        //Get script
                        Component component = components[i];

                        //Add script to menu
                        menu.AddItem(new GUIContent(GetScriptName(component)), false, () => {
                            scriptProperty.boxedValue = component;
                            Reset(property, From.Method);
                        });
                    }
                    menu.ShowAsContext();
                }



                 /*$      /$$             /$$     /$$                       /$$
                | $$$    /$$$            | $$    | $$                      | $$
                | $$$$  /$$$$  /$$$$$$  /$$$$$$  | $$$$$$$   /$$$$$$   /$$$$$$$
                | $$ $$/$$ $$ /$$__  $$|_  $$_/  | $$__  $$ /$$__  $$ /$$__  $$
                | $$  $$$| $$| $$$$$$$$  | $$    | $$  \ $$| $$  \ $$| $$  | $$
                | $$\  $ | $$| $$_____/  | $$ /$$| $$  | $$| $$  | $$| $$  | $$
                | $$ \/  | $$|  $$$$$$$  |  $$$$/| $$  | $$|  $$$$$$/|  $$$$$$$
                |__/     |__/ \_______/   \___/  |__/  |__/ \______/  \______*/

                //Show method selection dropdown
                if (EditorGUI.DropdownButton(methodRect, new GUIContent(method == "" ? "Method" : method), FocusType.Passive)) {
                    //Missing target or script
                    if (!target || !script) return;

                    //Select method
                    MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

                    //Create menu & add items
                    GenericMenu menu = new();
                    foreach (var info in methods) {
                        //Get parameters
                        ParameterInfo[] parameters = info.GetParameters();

                        //Check if parameters are valid
                        bool valid = true;
                        foreach (var parameter in parameters) {
                            //Check if is valid
                            if (TurboValue.IsValid(parameter.ParameterType)) continue;

                            //Invalid -> Stop loop
                            valid = false;
                            break;
                        }
                        if (!valid) continue;

                        //Get method name
                        StringBuilder itemName = new();
                        itemName.Append($"{info.Name}(");
                        for (int i = 0; i < parameters.Length; i++) itemName.Append($"{(i == 0 ? "" : ", ")}{parameters[i].ParameterType.Name} {parameters[i].Name}");
                        itemName.Append(")");

                        //Add method to menu
                        menu.AddItem(new GUIContent(itemName.ToString()), false, () => {
                            //Update method name
                            methodProperty.stringValue = info.Name;
                            
                            //Clear parameters array
                            parametersProperty.ClearArray();

                            //Add items to parameters array
                            for (int i = 0; i < parameters.Length; i++) {
                                //Get parameter info
                                ParameterInfo parameter = parameters[i];

                                //Add new item
                                parametersProperty.InsertArrayElementAtIndex(i);
                                parametersProperty.GetArrayElementAtIndex(i).boxedValue = new TurboParameter(parameter.Name, new TurboValue(parameter.ParameterType));
                            }

                            //Save
                            Save(property);
                        });
                    }
                    menu.ShowAsContext();
                }



                 /*$$$$$$                                                        /$$                                  
                | $$__  $$                                                      | $$                                  
                | $$  \ $$ /$$$$$$   /$$$$$$  /$$$$$$  /$$$$$$/$$$$   /$$$$$$  /$$$$$$    /$$$$$$   /$$$$$$   /$$$$$$$
                | $$$$$$$/|____  $$ /$$__  $$|____  $$| $$_  $$_  $$ /$$__  $$|_  $$_/   /$$__  $$ /$$__  $$ /$$_____/
                | $$____/  /$$$$$$$| $$  \__/ /$$$$$$$| $$ \ $$ \ $$| $$$$$$$$  | $$    | $$$$$$$$| $$  \__/|  $$$$$$ 
                | $$      /$$__  $$| $$      /$$__  $$| $$ | $$ | $$| $$_____/  | $$ /$$| $$_____/| $$       \____  $$
                | $$     |  $$$$$$$| $$     |  $$$$$$$| $$ | $$ | $$|  $$$$$$$  |  $$$$/|  $$$$$$$| $$       /$$$$$$$/
                |__/      \_______/|__/      \_______/|__/ |__/ |__/ \_______/   \___/   \_______/|__/      |______*/ 
                
                //Show parameters
                if (parametersProperty.arraySize > 0) {
                    //Create rect
                    Rect argumentRect = new(lineX, methodRect.y, lineWidth, lineHeight);

                    //Add parameters
                    for (int i = 0; i < parametersProperty.arraySize; i++) {
                        argumentRect.y += TurboParameterInspector.lineHeight;
                        EditorGUI.PropertyField(argumentRect, parametersProperty.GetArrayElementAtIndex(i));
                    }
                }


                //End
                EditorGUI.EndProperty();
            }

            //Util
            private void Reset(SerializedProperty property, From from) {
                //Get properties
                SerializedProperty targetProperty = property.FindPropertyRelative("target");
                SerializedProperty targetOldProperty = property.FindPropertyRelative("targetOld");
                SerializedProperty scriptProperty = property.FindPropertyRelative("script");
                SerializedProperty methodProperty = property.FindPropertyRelative("method");
                SerializedProperty parametersProperty = property.FindPropertyRelative("parameters");
                
                //Get target object
                var targetObject = targetProperty.objectReferenceValue;

                //Reset
                switch (from) {
                    case From.Target:
                        //Update target
                        targetOldProperty.objectReferenceValue = targetProperty.objectReferenceValue;

                        //Update script
                        scriptProperty.objectReferenceValue = targetObject ? null : targetProperty.objectReferenceValue;

                        //Reset method & parameters
                        scriptProperty.objectReferenceValue = targetObject ? null : targetProperty.objectReferenceValue;
                        methodProperty.stringValue = "";
                        parametersProperty.ClearArray();
                        break;

                    case From.Script:
                        //Update script
                        scriptProperty.objectReferenceValue = targetObject ? null : targetProperty.objectReferenceValue;

                        //Reset method & parameters
                        methodProperty.stringValue = "";
                        parametersProperty.ClearArray();
                        break;

                    case From.Method:
                        //Reset method & parameters
                        methodProperty.stringValue = "";
                        parametersProperty.ClearArray();
                        break;
                }

                //Save
                Save(property);
            }

            private string GetScriptName(UnityEngine.Object script) {
                //Invalid script
                if (!script) return "Script";
                
                //Get name
                return script.GetType().Name;
            }

            private void Save(SerializedProperty property) {
                //Save
                property.serializedObject.ApplyModifiedProperties();
                //EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        
        }

        //Property inspector
        [CustomPropertyDrawer(typeof(TurboParameter))]
        private class TurboParameterInspector : PropertyDrawer {

            //Container size
            public const float lineHeight = 20f;


            //Property drawer
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
                return lineHeight;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                //Begin
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.BeginChangeCheck();

                //Create parameter field
                TurboParameter parameter = property.boxedValue as TurboParameter;
                TurboValue value = parameter.value;
                object newValue = value.CreateField(position, new GUIContent(Util.Capitalize(parameter.name)));

                //Save parameter field
                SerializedProperty valueProperty = property.FindPropertyRelative("_value");
                if (TurboValue.IsObjectType(value.type)) {
                    //Serialize unity object
                    valueProperty.FindPropertyRelative("serializedObjectValue").boxedValue = newValue;
                } else {
                    //Serialize other types
                    valueProperty.FindPropertyRelative("serializedValue").stringValue = TurboValue.SerializeValue(value.type, newValue);
                }

                //Finish & check for changes
                if (EditorGUI.EndChangeCheck()) {
                    property.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
                EditorGUI.EndProperty();
            }

        }
        #endif

    }

    [Serializable]
    public class TurboEventGroup {

        [SerializeField] private List<TurboEvent> events = new();
        
        ///<summary>
        ///Invokes all of the events.
        ///</summary>
        public void Invoke() {
            foreach (var e in events) e.Invoke();
        }

        //Property inspector
        #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(TurboEventGroup))]
        private class TurboEventGroupInspector : PropertyDrawer {

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
                return 0;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                //Begin
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.BeginChangeCheck();

                //Create list
                EditorGUILayout.PropertyField(property.FindPropertyRelative("events"), label);
                
                //Finish & check for changes
                if (EditorGUI.EndChangeCheck()) {
                    property.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
                EditorGUI.EndProperty();
            }

        }
        #endif

    }

}

