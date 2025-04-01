using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NRandom.Unity.Editor
{
    [CustomPropertyDrawer(typeof(SerializableWeightedList<>))]
    public class SerializableWeightedListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new PropertyField(property.FindPropertyRelative("values"));
        }
    }
}