using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Configuraciones para el sistema de subir datos
[CreateAssetMenu(fileName = "NewUploaderSettings", menuName = "Data/UploaderSettings")]
public class UploaderSettings : ScriptableObject
{
    public string Header;

    public List<DataUploader.pair> Entries;

    public string URL;

    public string getURL;
}
