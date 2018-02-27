namespace Jurassic.CommonModels.Articles
{
    using Jurassic.AppCenter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ��Ŀ����չ���ԣ��ڸ��ݴ���Ŀ�����¶���ʱ��
    /// �������չ������ȫ���������ڵ���Ŀ��չ���Լ�����
    /// </summary>
    [Table("BASE_CATALOGEXT")]
    public partial class Base_CatalogExt : StateEntity, IIdNameBase<int>
    {
        /// <summary>
        /// ����һ���µ���Ŀ��չ����
        /// </summary>
        public Base_CatalogExt()
        {
        }

        /// <summary>
        /// ����ĿID
        /// </summary>
        [Column("CATALOGID")]
        public int CatalogId { get; set; }

        /// <summary>
        /// ��չ��������
        /// </summary>
        [Column("NAME")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("DATATYPE")]
        public ExtDataType DataType { get; set; }

        /// <summary>
        /// Ĭ��ֵ
        /// </summary>
        [Column("DEFAULTVALUE")]
        [StringLength(200)]
        public string DefaultValue { get; set; }

        /// <summary>
        /// �ṩ���û�ѡ�������Դ����
        /// </summary>
        [Column("DATASOURCETYPE")]
        public ExtDataSourceType DataSourceType { get; set; }

        /// <summary>
        /// �û�ѡ�������Դ����
        /// </summary>
        [Column("DATASOURCE")]
        public string DataSource { get; set; }

        /// <summary>
        /// ��󳤶�
        /// </summary>
        [Column("MAXLENGTH")]
        public int MaxLength { get; set; }

        ///// <summary>
        ///// ��С���ȣ�Ĭ��0
        ///// </summary>
        //public int MinLength { get; set; }

        /// <summary>
        /// �Ƿ������ֵ
        /// </summary>
        [Column("ALLOWNULL")]
        public bool AllowNull { get; set; }

        /// <summary>
        /// ���ֵ
        /// </summary>
        public object MaxValue { get; set; }

        /// <summary>
        /// ��Сֵ
        /// </summary>
        public object MinValue { get; set; }

        /// <summary>
        /// ����λ
        /// </summary>
        [Column("ORD")]
        public int Ord { get; set; }
    }
}
