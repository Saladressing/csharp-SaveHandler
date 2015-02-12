# csharp-SaveHandler

<h3>Overview</h3>
Xml File Serialization. Save, Load, Remove, and Create XML files over serialization.

# <h3>Generic File</h3>
The SaveHandler contains a basic default Xml file template. This class can be inherited for generic use.
```
[Serializable]
public class XmlFile
{
    [XmlAttribute]
    public string Title = string.Empty;

    public XmlFile() { }

    public XmlFile(string title)
    {
        Title = title;
    }

    ~XmlFile() { }
}
```  

# <h3>Custom File</h3>
The SaveHandler supports both inherited and custom files for use. Sample outputs can be found here: <a href="SaveHandler/bin">Sample Files</a>

```

// Custom Inheriated Class.
[Serializable]
public class CustomXmlFile : XmlFile
{
    public CustomXmlFile()
    {
        Title = "CustomXmlFile";
        Element1 = 0;
        Element2 = "value";
    }

    [XmlElement]
    public short Element1;

    [XmlElement("Element2")]
    public string Element2;
}

// Custom Xml File.
[Serializable]
public class SampleFile
{
  public SampleFile()
  {
      listItems = new List<string>();
      listItemsArray = listItems;
  }

  ~SampleFile()
  {
      if (listItems != null)
          listItems = null;

      if (listItemsArray != null)
          listItemsArray = null;
  }

  [XmlElement]
  public List<string> listItems = null;

  [XmlArray]
  public List<string> listItemsArray = null;
}
```

# <h3>Methods</h3>

<h5>GetFile<T>(string location)</h5>
Attempts to retrieve a loaded file from a hashtable. If the file does not exist, an attempt will be made to load it. If the file doesn't exist and/or the path is incorrect, it will return NULL.
```
// Attempt to get a loaded file
string location = @"..\MyXmlFile.xml";
XmlFile file = saveHandler.GetFile<XmlFile>(location);
```

<h5>RemoveLoadedFile(string location)</h5>
Attempts to remove a loaded file from the hashtable.
````
// Remove the loaded file
string location = @"..\MyXmlFile.xml";
saveHandler.RemoveLoadedFile(location);
````

<h5>DeleteFile(string location)</h5>
Attempts to remove a loaded file from the hashtable, and deletes the file from the computer.
````
// Delete the loaded file
string location = @"..\MyXmlFile.xml";
saveHandler.DeleteFile(location);
````

<h5>CreateFile<T>(string location, T file, bool overwrite)</h5>
Attempts to create a new file, and loads it to a hashtable. If the file exists, an overwrite is required to replace the currently exiting file. 
```
// Create a new XmlFile. Will
// overwrite if file already exists.
string location = @"..\MyXmlFile.xml";
saveHandler.CreateFile<XmlFile>(location, new XmlFile(), true);
```

<h5>SaveFile<T>(string location)</h5>
Attempts to save the file. False will be returned if the file doesn't exist and/or a save attempt failed.
```
// Save the file.
string location = @"..\MyXmlFile.xml";
saveHandler.SaveFile<XmlFile>(location);
```
