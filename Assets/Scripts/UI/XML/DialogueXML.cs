using System.Xml;
using System.Xml.Serialization;

[XmlRoot(ElementName = "info")]
public class DialogueXML {

    [XmlArray("dialogue")]
    [XmlArrayItem("item")]
    public DialogueItemXml[] dialogueItems { get; set; }

    [XmlElement(ElementName = "font")]
    public FontXml font;
}

[XmlRoot(ElementName = "font")]
public class FontXml {

    [XmlElement(ElementName = "size")]
    public int size;

    [XmlElement(ElementName = "style")]
    public string style;
}

[XmlRoot(ElementName = "item")]
public class DialogueItemXml {

    [XmlElement(ElementName = "speaker")]
    public string speaker;

    [XmlElement(ElementName = "text")]
    public string text;

    [XmlArray("options")]
    [XmlArrayItem("option")]
    public string[] options { get; set; }
}