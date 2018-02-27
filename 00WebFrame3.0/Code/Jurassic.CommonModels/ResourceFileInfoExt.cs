namespace Jurassic.CommonModels
{
    using Jurassic.AppCenter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ResourceFileInfoExt
    {
        public ResourceFileInfoExt()
        {
        }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public FileInfoExtDataType DataType { get; set; }
        
        public string Value { get; set; }
        
    }
    public enum FileInfoExtDataType
    {
        SingleNumber,
        FloatNumber,
        Date,
        DateAndTime,
        String
    }
}
