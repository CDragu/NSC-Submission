using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyManager : MonoBehaviour
{
    public float MarginOfErrorForPlacing = 1;
    public bool HasToAssembleInOrder;

    public GameObject ModelToAssemble;
    public Material HologramMaterial;
    public Material NextPartHologramMaterial;

    public List<Part> OrderOfAssembly;

    public void Start()
    {
        for (int i = 0; i < ModelToAssemble.transform.childCount; i++)
        {
            OrderOfAssembly.Add(ModelToAssemble.transform.GetChild(i).gameObject.GetComponent<Part>());
        }

        for (int i = 0; i < OrderOfAssembly.Count; i++)
        {
            GameObject hologram = Instantiate(OrderOfAssembly[i].gameObject, ModelToAssemble.transform);
            hologram.GetComponent<MeshRenderer>().material = HologramMaterial;
            hologram.layer = LayerMask.NameToLayer("Holograms");
            Hologram hologramComponent = hologram.AddComponent<Hologram>();
            OrderOfAssembly[i].hologram = hologramComponent;
        }

        ModelToAssemble.GetComponent<ModelPartSpreader>().Spread();

        if (HasToAssembleInOrder)
            OrderOfAssembly[0].hologram.GetComponent<MeshRenderer>().material = NextPartHologramMaterial;
    }

    public void HighlightObject(Vector3 Pos)
    {
       
    }

    Part target;
    bool IsDragging = false;
    Vector3 screenPosition;
    Vector3 offset;
    public void PickObjectUp()
    {
        target = null;
        RaycastHit hitInfo;
        target = ReturnClickedObject(out hitInfo, LayerMask.NameToLayer("Parts"))?.GetComponent<Part>();
        if (target != null)
        {
            IsDragging = true;
            screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
            offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
        }
    }

    private void Update()
    {
        if (IsDragging)
        {
            //track mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            //It will update target gameobject's current postion.
            target.transform.position = currentPosition;
        }
    }

    public void PlaceObject()
    {
        IsDragging = false;

        if(target == null)
        {
            return;
        }

        if (HasToAssembleInOrder)
        {
            if (target == OrderOfAssembly[0])
            {
                if (Vector3.Distance(target.hologram.gameObject.transform.position, target.transform.position) < MarginOfErrorForPlacing)
                {
                    target.transform.position = target.hologram.gameObject.transform.position;
                    target.hologram.GetComponent<MeshRenderer>().enabled = false;
                    target.gameObject.layer = LayerMask.NameToLayer("Default");
                    OrderOfAssembly.Remove(OrderOfAssembly[0]);
                    if(OrderOfAssembly.Count != 0)
                        OrderOfAssembly[0].hologram.GetComponent<MeshRenderer>().material = NextPartHologramMaterial;
                }
                else
                {
                    target.ResetToInitial();
                }
            }
            else
            {
                target.ResetToInitial();
            }
        }
        else
        {
           
            if (Vector3.Distance(target.hologram.gameObject.transform.position, target.transform.position) < MarginOfErrorForPlacing)
            {
                target.transform.position = target.hologram.gameObject.transform.position;
                target.gameObject.layer = LayerMask.NameToLayer("Default");
                target.hologram.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                target.ResetToInitial();
            }
            
        }
    
       

        
    }

    GameObject ReturnClickedObject(out RaycastHit hit, int layerToHit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << layerToHit;
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit, 300, layerMask))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
}
