using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YeKostenko.ContractClicker.Data.Modifiers;

namespace YeKostenko.ContractClicker.Editor.Modifiers
{
    [CustomPropertyDrawer(typeof(ModifierConfigBaseWrapper))]
    public class ModifierConfigBaseWrapperDrawer : PropertyDrawer
    {
        private static List<Type> s_modifierConfigTypes;
        private static string[] s_modifierConfigNames;
        private static bool s_typesInitialized;

        private const float LineHeight = 18f;
        private const float Spacing = 2f;

        static ModifierConfigBaseWrapperDrawer()
        {
            InitializeTypes();
        }

        private static void InitializeTypes()
        {
            if (s_typesInitialized)
            {
                return;
            }

            // Знайти всі типи що наслідуються від ModifierConfigBase
            s_modifierConfigTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => !t.IsAbstract && t.IsClass && typeof(ModifierConfigBase).IsAssignableFrom(t))
                .OrderBy(t => t.Name)
                .ToList();

            s_modifierConfigNames = s_modifierConfigTypes
                .Select(t => t.Name.Replace("Config", ""))
                .ToArray();

            s_typesInitialized = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty configProperty = property.FindPropertyRelative("_config");

            if (configProperty?.managedReferenceValue == null)
            {
                // Тільки dropdown для вибору типу
                return LineHeight + Spacing;
            }

            // Dropdown + всі поля конфігу
            float height = LineHeight + Spacing;

            // Використовуємо стандартну висоту для SerializedProperty
            height += EditorGUI.GetPropertyHeight(configProperty, true) + Spacing;

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!s_typesInitialized)
            {
                InitializeTypes();
            }

            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty configProperty = property.FindPropertyRelative("_config");

            Rect currentRect = new Rect(position.x, position.y, position.width, LineHeight);

            // Показати label
            currentRect = EditorGUI.PrefixLabel(currentRect, label);

            // Визначити поточний тип
            int selectedIndex = -1;
            Type currentType = configProperty?.managedReferenceValue?.GetType();

            if (currentType != null)
            {
                selectedIndex = s_modifierConfigTypes.IndexOf(currentType);
            }

            // Dropdown для вибору типу
            EditorGUI.BeginChangeCheck();

            // Додаємо опцію "None" на початок
            List<string> displayOptions = new List<string> { "None" };
            displayOptions.AddRange(s_modifierConfigNames);

            int newIndex = EditorGUI.Popup(currentRect, selectedIndex + 1, displayOptions.ToArray()) - 1;

            if (EditorGUI.EndChangeCheck())
            {
                if (newIndex < 0)
                {
                    // Вибрано "None"
                    if (configProperty != null)
                    {
                        configProperty.managedReferenceValue = null;
                    }
                }
                else
                {
                    // Створити новий екземпляр обраного типу
                    Type newType = s_modifierConfigTypes[newIndex];
                    object newInstance = Activator.CreateInstance(newType);
                    if (configProperty != null)
                    {
                        configProperty.managedReferenceValue = newInstance;
                    }
                }

                if (configProperty != null)
                {
                    configProperty.serializedObject.ApplyModifiedProperties();
                }
            }

            // Відобразити поля конфігу, якщо він існує
            if (configProperty?.managedReferenceValue != null)
            {
                currentRect.y += LineHeight + Spacing;
                currentRect.x = position.x;
                currentRect.width = position.width;
                currentRect.height = EditorGUI.GetPropertyHeight(configProperty, true);

                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(currentRect, configProperty, GUIContent.none, true);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }
    }
}




