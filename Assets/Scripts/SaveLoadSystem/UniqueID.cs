using System;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string _id;

    [SerializeField] private static SerializableDictionary<string, GameObject> idDataBase = new SerializableDictionary<string, GameObject>();

    public string ID => _id;

    private void Awake()
    {
        if (idDataBase == null)
        {
            idDataBase = new SerializableDictionary<string, GameObject>();
        }

        if (idDataBase.ContainsKey(_id))
        {
            Generate();
        }
        else 
        {
            idDataBase.Add(_id, this.gameObject);
        } 
    }
    

    private void OnDestroy()
    {
        if (idDataBase.ContainsKey(_id))
        {
            idDataBase.Remove(_id);
        }
    }

    [ContextMenu("Generate ID")]
    private void Generate()
    {
        _id = Guid.NewGuid().ToString();
        idDataBase.Add(_id, this.gameObject);
    }
}
