using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (CollisionTester))]
public class CollisionTestEditor : Editor {

    public override void OnInspectorGUI () {
        DrawDefaultInspector ();

        if (GUILayout.Button ("Test Collision")) {
            var tester = ((CollisionTester) target);
            tester.TestCollision ();
        }

        if (GUILayout.Button ("Reset")) {
            var tester = ((CollisionTester) target);
            tester.Reset();
        }
    }
}