using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(TestMapController))]
    public class TestMapControllerEditor : Editor
    {
        private string _result = "";
        private GUIStyle _boxStyle = GUIStyle.none;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load"))
            {
                var controller = target as TestMapController;

                try
                {
                    _result = controller.Load();
                    _boxStyle = new GUIStyle(GUI.skin.box);
                    _boxStyle.normal.textColor = Color.green;
                }
                catch (Exception e)
                {
                    _boxStyle = new GUIStyle(GUI.skin.box);
                    _boxStyle.normal.textColor = Color.red;

                    _result = e.ToString();
                }
            }

            if (GUILayout.Button("Save"))
            {
                var controller = target as TestMapController;

                try
                {
                    _result = controller.Save();
                    _boxStyle = new GUIStyle(GUI.skin.box);
                    _boxStyle.normal.textColor = Color.green;
                }
                catch (Exception e)
                {
                    _boxStyle = new GUIStyle(GUI.skin.box);
                    _boxStyle.normal.textColor = Color.red;

                    _result = e.ToString();
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Box(_result, _boxStyle, GUILayout.ExpandWidth(true));
        }
    }
}
