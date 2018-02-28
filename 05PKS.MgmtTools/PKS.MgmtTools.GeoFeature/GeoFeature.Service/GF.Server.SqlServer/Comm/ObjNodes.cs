using Juarssic.Server.Comm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json_Sql
{
    [DebuggerDisplay("Type={GetType().Name} Text = {Text}")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ObjModel
    {

        public ObjModel()
        {
            _nodes = new List<ObjModel>();
        }

        #region 属性
        private string _fieldN;
        private string _opt;
        private string _relations;
        private bool _leaives;
        private object _value;
        private List<ObjModel> _nodes;
        private JsonType _jsonType;
        private ObjModel _parent;

        public ObjModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public JsonType JsonType
        {
            get { return _jsonType; }
            set { _jsonType = value; }
        }


        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public List<ObjModel> Nodes
        {
            get
            {
                return _nodes;
            }
            set
            {
                _nodes = value;
            }
        }


        public string FieldN
        {
            get
            {
                return _fieldN;
            }
            set
            {
                _fieldN = value;
            }
        }


        public string Opt
        {
            get
            {
                return _opt;
            }
            set
            {
                _opt = value;
            }
        }


        public string Relations
        {
            get
            {
                return _relations;
            }
            set
            {
                _relations = value;
            }
        }

        public bool Leaives
        {
            get
            {
                return _leaives;
            }
            set
            {
                _leaives = value;
            }
        }
        #endregion



    }
}
