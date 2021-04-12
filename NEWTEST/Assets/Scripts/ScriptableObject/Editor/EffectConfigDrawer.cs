using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(EffectConfig))]
public class EffectConfigDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var baseType = property.FindPropertyRelative("BaseEffectType");
        int enumIndex = (int)(EffectType)EditorGUI.EnumPopup(new Rect(position.x, position.y, position.width, 16), "EffectType", (EffectType)baseType.enumValueIndex);
        baseType.enumValueIndex = enumIndex;
        switch ((EffectType)baseType.enumValueIndex)
        {
            case EffectType.AttributeEffect:
                var attributeType = property.FindPropertyRelative("AttributeType");
                attributeType.enumValueIndex = (int)(AttributeType)EditorGUI.EnumPopup(new Rect(position.x, position.y + 20, position.width, 16), "AttributeType", (AttributeType)attributeType.enumValueIndex);
                break;
            case EffectType.AttackEffect:
                var attackEffectType = property.FindPropertyRelative("AttackEffectType");
                attackEffectType.enumValueIndex = (int)(AttackEffectType)EditorGUI.EnumPopup(new Rect(position.x, position.y + 20, position.width, 16), "AttackEffectType", (AttackEffectType)attackEffectType.enumValueIndex);
                break;
            case EffectType.EnemyBuff:
                var enemyBuffType = property.FindPropertyRelative("EnemyBuffType");
                enemyBuffType.enumValueIndex = (int)(EnemyBuffType)EditorGUI.EnumPopup(new Rect(position.x, position.y + 20, position.width, 16), "EnemyBuffType", (EnemyBuffType)enemyBuffType.enumValueIndex);
                break;
            case EffectType.NoTargetBuff:
                var noTargetBuffType = property.FindPropertyRelative("NoTargetBuffType");
                noTargetBuffType.enumValueIndex = (int)(NoTargetBuffType)EditorGUI.EnumPopup(new Rect(position.x, position.y + 20, position.width, 16), "NoTargetBuffType", (NoTargetBuffType)noTargetBuffType.enumValueIndex);
                break;
        }
        position = EditorGUI.PrefixLabel(new Rect(position.x, position.y + 40, position.width, 16), GUIUtility.GetControlID(FocusType.Passive), new GUIContent("KeyValue"));
        float value = property.FindPropertyRelative("KeyValue").floatValue;
        float newValue = EditorGUI.FloatField(new Rect(position.x, position.y, position.width, 16), value);
        property.FindPropertyRelative("KeyValue").floatValue = newValue;

        EditorGUI.EndProperty();
        
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3 + 6;
    }


}
