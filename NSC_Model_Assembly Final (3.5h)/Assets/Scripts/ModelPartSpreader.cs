using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPartSpreader : MonoBehaviour
{
    public int SizeOfSpreadArea = 10;
    public Vector3 StartingAreaOffset = new Vector3(10, 0, 10);
    int[,] takenPositions;

    public List<GameObject> PartsToSpread = new List<GameObject>();

    // Start is called before the first frame update
    public void Spread()
    {
        //Create enough positions for all the pieces
        int Xsize = PartsToSpread.Count/2;
        int Zsize = PartsToSpread.Count/2;

        takenPositions = new int[Xsize*2,Zsize*2];

        //Find each part a position
        foreach (var Part in PartsToSpread)
        {
            int RandomX, RandomZ;

            do
            {
                RandomX = Random.Range(0, (Xsize*2)-1);
                RandomZ = Random.Range(0, (Zsize*2)-1);

            } while (takenPositions[RandomX, RandomZ] != 0 || 
            (RandomX == Mathf.Floor(Xsize /2) || RandomZ == Mathf.Floor(Zsize/2)));//TODO: take a better look a this, not working as intended now

            //Making sure a part is not on top of another part
            takenPositions[RandomX,RandomZ] = 1;

            Part.transform.position = StartingAreaOffset + new Vector3(
                (RandomX - Xsize) * SizeOfSpreadArea, 
                0,
                (RandomZ - Zsize) * SizeOfSpreadArea);

            //Making sure all the parts are on the ground properly
            Part.transform.position += new Vector3(0, Part.GetComponent<MeshRenderer>().bounds.extents.y,0);
            Part.GetComponent<Part>().SetIntialPos();
        }

       
    }
}
