using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelPreparation
{
    [MenuItem("CAPP-APP/Convert to placeable object")]
    private static void PrepareModel()
    {
        GameObject objectToPrepare = Selection.activeTransform.gameObject;

        //TODO:Search to see if there is any assembly manager in scene
        //PlayerController.instance.gameObject.GetComponent<AssemblyManager>().ModelToAssemble = objectToPrepare;

        //Adding a Part Spreader to spread them at start;
        ModelPartSpreader spreader = objectToPrepare.AddComponent<ModelPartSpreader>();

        //Adding colliders to the models if it doesn't have one already
        for (int i = 0; i < objectToPrepare.transform.childCount; i++)
        {
            GameObject currentChild = objectToPrepare.transform.GetChild(i).gameObject;
            if (currentChild.GetComponent<Collider>() == null)
            {
                currentChild.AddComponent<MeshCollider>();
            }
            currentChild.AddComponent<Part>();

            currentChild.layer = LayerMask.NameToLayer("Parts");
            spreader.PartsToSpread.Add(currentChild);
        }

        
    }
}
