using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWorldObject : MonoBehaviour
{
    public int id;

    public WorldObject GetData()
    {
        //id = WorldManager.instance.dictionary.IndexOf(PrefabUtility.GetCorrespondingObjectFromSource(gameObject));
        /*string n = gameObject.name;
        string myNameWithNotBrackets = n.Substring(0,n.IndexOf('(') - 2);
        for (int i = 0; i < WorldManager.instance.dictionary.Count; i++)
        {
            if (WorldManager.instance.dictionary[i].name == myNameWithNotBrackets) 
                id = i;
        } */
        return new WorldObject(id,
                               transform.position.x,
                               transform.position.y,
                               transform.position.z,
                               transform.rotation.x,
                               transform.rotation.y,
                               transform.rotation.z,
                               transform.rotation.w);
    }
}
