﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// 此源代码由 xsd 自动生成, Version=4.0.30319.17929。
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.jurassic.com.cn/3gx")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.jurassic.com.cn/3gx", IsNullable = true)]
public partial class CRS
{

    private string codeSpaceField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string codeSpace
    {
        get
        {
            return this.codeSpaceField;
        }
        set
        {
            this.codeSpaceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.jurassic.com.cn/3gx")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.jurassic.com.cn/3gx", IsNullable = false)]
public partial class FeatureCollection
{

    private CRS[] cRSField;

    private GF[] gfField;

    private string idField;

    private string nameField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("CRS", IsNullable = true)]
    public CRS[] CRS
    {
        get
        {
            return this.cRSField;
        }
        set
        {
            this.cRSField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("GF")]
    public GF[] GF
    {
        get
        {
            return this.gfField;
        }
        set
        {
            this.gfField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true, Namespace = "http://www.jurassic.com.cn/3gx")]
public partial class GF
{

    private string titleField;

    private string descriptionField;

    private string subFeaturesField;

    private FeatureCollectionGFName[] nameField;

    private FeatureCollectionGFPropertySetsPropertySet[] PropertySetsField;

    private CRS[] cRSField;

    private FeatureCollectionGFShapesShape[] shapesField;

    private string idField;

    private string typeField;

    private string classField;

    /// <remarks/>
    public string Title
    {
        get
        {
            return this.titleField;
        }
        set
        {
            this.titleField = value;
        }
    }

    /// <remarks/>
    public string Description
    {
        get
        {
            return this.descriptionField;
        }
        set
        {
            this.descriptionField = value;
        }
    }

    /// <remarks/>
    public string SubFeatures
    {
        get
        {
            return this.subFeaturesField;
        }
        set
        {
            this.subFeaturesField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Name", IsNullable = true)]
    public FeatureCollectionGFName[] Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("PropertySet", typeof(FeatureCollectionGFPropertySetsPropertySet), IsNullable = false)]
    public FeatureCollectionGFPropertySetsPropertySet[] PropertySets
    {
        get
        {
            return this.PropertySetsField;
        }
        set
        {
            this.PropertySetsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("CRS", IsNullable = true)]
    public CRS[] CRS
    {
        get
        {
            return this.cRSField;
        }
        set
        {
            this.cRSField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Shape", typeof(FeatureCollectionGFShapesShape), IsNullable = false)]
    public FeatureCollectionGFShapesShape[] Shapes
    {
        get
        {
            return this.shapesField;
        }
        set
        {
            this.shapesField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string @class
    {
        get
        {
            return this.classField;
        }
        set
        {
            this.classField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.jurassic.com.cn/3gx")]
public partial class FeatureCollectionGFName
{

    private string codeSpaceField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string codeSpace
    {
        get
        {
            return this.codeSpaceField;
        }
        set
        {
            this.codeSpaceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute("PropertySet", AnonymousType = true, Namespace = "http://www.jurassic.com.cn/3gx")]
[System.Xml.Serialization.XmlRootAttribute("PropertySet", Namespace = "http://www.jurassic.com.cn/3gx", IsNullable = false)]
public partial class FeatureCollectionGFPropertySetsPropertySet
{

    private FeatureCollectionGFPropertySetsPropertySetP[] pField;

    private string nameField;

    private string codeSpaceField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("P", IsNullable = true)]
    public FeatureCollectionGFPropertySetsPropertySetP[] P
    {
        get
        {
            return this.pField;
        }
        set
        {
            this.pField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string codeSpace
    {
        get
        {
            return this.codeSpaceField;
        }
        set
        {
            this.codeSpaceField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.jurassic.com.cn/3gx")]
public partial class FeatureCollectionGFPropertySetsPropertySetP
{

    private string nField;

    private string tField;

    private string rField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string n
    {
        get
        {
            return this.nField;
        }
        set
        {
            this.nField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string t
    {
        get
        {
            return this.tField;
        }
        set
        {
            this.tField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string r
    {
        get
        {
            return this.rField;
        }
        set
        {
            this.rField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.jurassic.com.cn/3gx")]
public partial class FeatureCollectionGFShapesShape
{
    private PolygonExteriorLinearRing[][] polygonField;

    private LineString lineStringField;

    private Point pointField;

    private string idField;

    private string nameField;

    private string labelField;

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.opengis.net/gml")]
    [System.Xml.Serialization.XmlArrayItemAttribute("exterior", IsNullable = false)]
    [System.Xml.Serialization.XmlArrayItemAttribute("LinearRing", IsNullable = false, NestingLevel = 1)]
    public PolygonExteriorLinearRing[][] Polygon
    {
        get
        {
            return this.polygonField;
        }
        set
        {
            this.polygonField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute( Namespace = "http://www.opengis.net/gml")]
    public LineString LineString
    {
        get
        {
            return this.lineStringField;
        }
        set
        {
            this.lineStringField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.opengis.net/gml")]
    public Point Point
    {
        get
        {
            return this.pointField;
        }
        set
        {
            this.pointField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string label
    {
        get
        {
            return this.labelField;
        }
        set
        {
            this.labelField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.opengis.net/gml")]
public partial class PolygonExteriorLinearRing
{

    private string posListField;

    /// <remarks/>
    public string posList
    {
        get
        {
            return this.posListField;
        }
        set
        {
            this.posListField = value;
        }
    }
}



/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(TypeName = "gml:LineString", AnonymousType = true, Namespace = "http://www.opengis.net/gml")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.opengis.net/gml", IsNullable = false)]
public partial class LineString
{

    private string posListField;

    /// <remarks/>
    public string posList
    {
        get
        {
            return this.posListField;
        }
        set
        {
            this.posListField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(TypeName = "gml:Point", AnonymousType = true, Namespace = "http://www.opengis.net/gml")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.opengis.net/gml", IsNullable = false)]
public partial class Point
{

    private string posField;

    /// <remarks/>
    public string pos
    {
        get
        {
            return this.posField;
        }
        set
        {
            this.posField = value;
        }
    }

    public Point()
    { }
}



/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.jurassic.com.cn/3gx")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.jurassic.com.cn/3gx", IsNullable = false)]
public partial class NewDataSet
{

    private object[] itemsField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("CRS", typeof(CRS), IsNullable = true)]
    [System.Xml.Serialization.XmlElementAttribute("FeatureCollection", typeof(FeatureCollection))]
    public object[] Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }
}


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.opengis.net/gml")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.opengis.net/gml", IsNullable = false)]
public partial class Polygon
{

    private PolygonExteriorLinearRing[] exteriorField;

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("LinearRing", typeof(PolygonExteriorLinearRing), IsNullable = false)]
    public PolygonExteriorLinearRing[] exterior
    {
        get
        {
            return this.exteriorField;
        }
        set
        {
            this.exteriorField = value;
        }
    }
}

