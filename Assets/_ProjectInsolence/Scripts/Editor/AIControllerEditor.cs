using UnityEngine;
using UnityEditor;
using Insolence.AIBrain;

[CustomEditor(typeof(NPCAIController))]
public class AIControllerEditor : Editor
{
    private bool[] foldout = new bool[0];

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NPCAIController aiController = (NPCAIController)target;

        if (aiController.availableActions.Length != foldout.Length)
        {
            foldout = new bool[aiController.availableActions.Length];
        }

        for (int i = 0; i < aiController.availableActions.Length; i++)
        {
            if(aiController.availableActions[i] != null)
            {
                Action action = aiController.availableActions[i];
                EditorGUILayout.BeginVertical("Box");
                action.name = EditorGUILayout.TextField("Action Name", action.name);
                action.score = EditorGUILayout.FloatField("Action Score", action.score);

                foldout[i] = EditorGUILayout.Foldout(foldout[i], aiController.availableActions[i].name + " Considerations");
                EditorGUILayout.EndVertical();
            }


            if (foldout[i])
            {
                EditorGUI.indentLevel++;

                for (int j = 0; j < aiController.availableActions[i].considerations.Length; j++)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.ObjectField(aiController.availableActions[i].considerations[j], typeof(Consideration), false);
                    EditorGUILayout.FloatField("Score", aiController.availableActions[i].considerations[j].score);
                    EditorGUILayout.EndVertical();
                    
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}


