﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.38967
//    <NameSpace>Terradue.ServiceModel.W3c.Xlink</NameSpace><Collection>Array</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>False</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net40</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>True</ExcludeIncludedTypes><EnableInitializeFields>False</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace Terradue.ServiceModel.W3c.Xlink
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    [System.Xml.Serialization.XmlRootAttribute("title", Namespace = "http://www.w3.org/1999/xlink", IsNullable = false)]
    public partial class titleEltType
    {

        private System.Xml.XmlNode[] anyField;

        private typeType typeField;

        private string langField;

        public titleEltType()
        {
            this.typeField = typeType.title;
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlNode[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public typeType type
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    public enum typeType
    {

        /// <remarks/>
        simple,

        /// <remarks/>
        extended,

        /// <remarks/>
        title,

        /// <remarks/>
        resource,

        /// <remarks/>
        locator,

        /// <remarks/>
        arc,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    [System.Xml.Serialization.XmlRootAttribute("resource", Namespace = "http://www.w3.org/1999/xlink", IsNullable = false)]
    public partial class resourceType
    {

        private System.Xml.XmlNode[] anyField;

        private typeType typeField;

        private string roleField;

        private string titleField;

        private string labelField;

        public resourceType()
        {
            this.typeField = typeType.resource;
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlNode[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public typeType type
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "anyURI")]
        public string role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string title
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "NCName")]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    [System.Xml.Serialization.XmlRootAttribute("locator", Namespace = "http://www.w3.org/1999/xlink", IsNullable = false)]
    public partial class locatorType
    {

        private titleEltType[] titleField;

        private typeType typeField;

        private string hrefField;

        private string roleField;

        private string title1Field;

        private string labelField;

        public locatorType()
        {
            this.typeField = typeType.locator;
        }

        [System.Xml.Serialization.XmlElementAttribute("title")]
        public titleEltType[] title
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public typeType type
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "anyURI")]
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "anyURI")]
        public string role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("title", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string title1
        {
            get
            {
                return this.title1Field;
            }
            set
            {
                this.title1Field = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "NCName")]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    [System.Xml.Serialization.XmlRootAttribute("arc", Namespace = "http://www.w3.org/1999/xlink", IsNullable = false)]
    public partial class arcType
    {

        private titleEltType[] titleField;

        private typeType typeField;

        private string arcroleField;

        private string title1Field;

        private showType showField;

        private bool showFieldSpecified;

        private actuateType actuateField;

        private bool actuateFieldSpecified;

        private string fromField;

        private string toField;

        public arcType()
        {
            this.typeField = typeType.arc;
        }

        [System.Xml.Serialization.XmlElementAttribute("title")]
        public titleEltType[] title
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public typeType type
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "anyURI")]
        public string arcrole
        {
            get
            {
                return this.arcroleField;
            }
            set
            {
                this.arcroleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("title", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string title1
        {
            get
            {
                return this.title1Field;
            }
            set
            {
                this.title1Field = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public showType show
        {
            get
            {
                return this.showField;
            }
            set
            {
                this.showField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool showSpecified
        {
            get
            {
                return this.showFieldSpecified;
            }
            set
            {
                this.showFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public actuateType actuate
        {
            get
            {
                return this.actuateField;
            }
            set
            {
                this.actuateField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actuateSpecified
        {
            get
            {
                return this.actuateFieldSpecified;
            }
            set
            {
                this.actuateFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "NCName")]
        public string from
        {
            get
            {
                return this.fromField;
            }
            set
            {
                this.fromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "NCName")]
        public string to
        {
            get
            {
                return this.toField;
            }
            set
            {
                this.toField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    public enum showType
    {

        /// <remarks/>
        @new,

        /// <remarks/>
        replace,

        /// <remarks/>
        embed,

        /// <remarks/>
        other,

        /// <remarks/>
        none,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    public enum actuateType
    {

        /// <remarks/>
        onLoad,

        /// <remarks/>
        onRequest,

        /// <remarks/>
        other,

        /// <remarks/>
        none,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/1999/xlink", IsNullable = true)]
    public partial class simple
    {

        private System.Xml.XmlNode[] anyField;

        private typeType typeField;

        private bool typeFieldSpecified;

        private string hrefField;

        private string roleField;

        private string arcroleField;

        private string titleField;

        private showType showField;

        private bool showFieldSpecified;

        private actuateType actuateField;

        private bool actuateFieldSpecified;

        public simple()
        {
            this.typeField = typeType.simple;
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlNode[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public typeType type
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeSpecified
        {
            get
            {
                return this.typeFieldSpecified;
            }
            set
            {
                this.typeFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "anyURI")]
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "anyURI")]
        public string role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "anyURI")]
        public string arcrole
        {
            get
            {
                return this.arcroleField;
            }
            set
            {
                this.arcroleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string title
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public showType show
        {
            get
            {
                return this.showField;
            }
            set
            {
                this.showField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool showSpecified
        {
            get
            {
                return this.showFieldSpecified;
            }
            set
            {
                this.showFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public actuateType actuate
        {
            get
            {
                return this.actuateField;
            }
            set
            {
                this.actuateField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actuateSpecified
        {
            get
            {
                return this.actuateFieldSpecified;
            }
            set
            {
                this.actuateFieldSpecified = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/1999/xlink")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/1999/xlink", IsNullable = true)]
    public partial class extended
    {

        private object[] itemsField;

        private typeType typeField;

        private string roleField;

        private string titleField;

        public extended()
        {
            this.typeField = typeType.extended;
        }

        [System.Xml.Serialization.XmlElementAttribute("arc", typeof(arcType))]
        [System.Xml.Serialization.XmlElementAttribute("locator", typeof(locatorType))]
        [System.Xml.Serialization.XmlElementAttribute("resource", typeof(resourceType))]
        [System.Xml.Serialization.XmlElementAttribute("title", typeof(titleEltType))]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public typeType type
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "anyURI")]
        public string role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string title
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
    }
}