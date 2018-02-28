using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PKS.Core;
using PKS.Models;
using PKS.Services;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;

namespace PKS.SubmissionTool.Index
{
    /// <summary>索引提交服务</summary>
    public class IndexSubmissionService : AppService, ISingletonAppService
    {
        /// <summary>索引提交服务</summary>
        public IndexSubmissionService(IApiServiceConfig apiServiceConfig)
        {
            this.ApiServiceConfig = apiServiceConfig.As<ModuleApiServiceConfig>();
            this.UploadChunkSize = ConfigurationManager.AppSettings[nameof(this.UploadChunkSize)].ToInt32();
        }
        /// <summary>上传文件分片大小,单位为K</summary>
        private int UploadChunkSize { get; set; }
        /// <summary>配置文件名称</summary>
        private string ConfigFileName
        {
            get { return "IndexSubmission.config"; }
        }
        /// <summary>默认配置文件</summary>
        public string DefaultConfigFile
        {
            get { return Path.Combine(Application.StartupPath, this.ConfigFileName); }
        }
        /// <summary>API服务配置</summary>
        internal ModuleApiServiceConfig ApiServiceConfig { get; set; }
        /// <summary>配置文件 </summary>
        internal string ConfigFile { get; set; }
        /// <summary>配置</summary>
        internal IndexSubmissionConfig Config { get; set; }
        /// <summary>元数据定义集合</summary>
        private MetadataDefinitionCollection MetadataTags { get; set; }
        /// <summary>索引页面数据映射</summary>
        private Dictionary<string, IndexPageData> PageMapper { get; set; }
        /// <summary>初始化方法</summary>
        public void Initialize(string configFile = null)
        {
            this.Config = null;
            this.Config = LoadConfig(configFile);
            if (this.Config.ApiService.IsValid())
            {
                LoadFromApiService(true, true);
            }
        }
        /// <summary>载入配置文件</summary>
        private IndexSubmissionConfig LoadConfig(string configFile = null)
        {
            if (configFile.IsNullOrEmpty())
            {
                if (this.ConfigFile.IsNullOrEmpty())
                {
                    configFile = this.DefaultConfigFile;
                }
                else
                {
                    configFile = this.ConfigFile;
                }
            }
            IndexSubmissionConfig config = null;
            if (File.Exists(configFile))
            {
                config = configFile.XmlDeserialize<IndexSubmissionConfig>();
            }
            if (config == null)
            {
                config = new IndexSubmissionConfig();
            }
            config.Load();
            AddBuildInVariables(config, config.Variables);
            this.ConfigFile = configFile;
            return config;
        }
        /// <summary>加入内置变量</summary>
        private void AddBuildInVariables(IndexSubmissionConfig config, List<XmlVariable> variables)
        {
            AddBuildInVariable(variables, SubmissionConsts.ToolName, "本工具名称", PKSWebConsts.GetSubSystemCode());
            AddBuildInVariable(variables, SubmissionConsts.Guid, "自动生成唯一值", string.Empty, new GuidValueProvider());
            AddBuildInVariable(variables, SubmissionConsts.UserName, "用户名称", config.ApiService.UserName);
            AddBuildInVariable(variables, SubmissionConsts.WebApiUrl, "WebAPI站点URL", config.ApiService.Url);
            AddBuildInVariable(variables, SubmissionConsts.ProductFolder, "成果文件夹", config.Product.Folder);
            AddBuildInVariable(variables, SubmissionConsts.ExcelFileName, "Excel文件名", Path.GetFileNameWithoutExtension(config.Product.ExcelFile));
            AddBuildInVariable(variables, SubmissionConsts.ShowType, "扩展名自动生成", string.Empty, new ExtToIndexDataTypeValueProvider());
            AddBuildInVariable(variables, SubmissionConsts.ProductFileName, "成果文件名(无扩展名)", string.Empty, new FileNameValueProvider());
        }
        /// <summary>加入内置变量</summary>
        private void AddBuildInVariable(List<XmlVariable> variables, string name, string title, string value, IValueProvider provider = null)
        {
            var variable = variables.FirstOrDefault(e => e.Name == name);
            if (variable == null)
            {
                variable = new XmlVariable();
                variable.Name = name;
                variables.Add(variable);
            }
            variable.Title = title;
            if (variable.Value.Length == 0) variable.Value = value;
            variable.BuildIn = true;
            variable.Provider = provider;
        }
        /// <summary>刷新变量值_API服务</summary>
        public void RefreshVariables_ApiService()
        {
            var config = this.Config.ApiService;
            var variables = this.Config.Variables;
            variables.First(e => e.Name == SubmissionConsts.UserName).Value = config.UserName;
            variables.First(e => e.Name == SubmissionConsts.WebApiUrl).Value = config.Url;
        }
        /// <summary>刷新变量值_API服务</summary>
        public void RefreshVariables_Product(params string[] variableNames)
        {
            var config = this.Config.Product;
            var variables = this.Config.Variables;
            foreach (var variableName in variableNames)
            {
                if (variableName == SubmissionConsts.ProductFolder)
                {
                    variables.First(e => e.Name == variableName).Value = config.Folder;
                }
                else if (variableName == SubmissionConsts.ExcelFileName)
                {
                    variables.First(e => e.Name == variableName).Value = Path.GetFileNameWithoutExtension(config.ExcelFile);
                }
            }
        }
        /// <summary>从API服务加载</summary>
        public void LoadFromApiService()
        {
            LoadFromApiService(true, true);
        }
        /// <summary>从API服务加载</summary>
        public void LoadFromApiService(bool force, bool loadFromConfig)
        {
            if (!force && this.PageMapper != null) return;
            if (!Login()) return;
            FileFormatExtension.Init();
            LoadMetadataTags(loadFromConfig);
            LoadIndexPageDatas();
        }
        /// <summary>登录</summary>
        private bool Login()
        {
            this.ApiServiceConfig.LoginResult = null;
            var service = GetService<ISecurityServiceWrapper>();
            var config = this.Config.ApiService;
            this.ApiServiceConfig.Config = config;
            var request = new LoginRequest();
            request.AppCode = PKSWebConsts.GetSubSystemCode();
            request.UserName = config.UserName;
            request.Password = config.Password;
            request.AuthenticationType = AuthenticationType.Forms;
            service.ResetServiceUrl();
            var result = service.Login(request);
            if (result.Succeed)
            {
                this.ApiServiceConfig.LoginResult = result;
                return true;
            }
            return false;
        }
        /// <summary>获得元数据标签集合</summary>
        private void LoadIndexPageDatas()
        {
            var service = GetService<IPageDataServiceWrapper>();
            service.ResetServiceUrl();
            var request = new IndexDataMatchRequest();
            request.Size = 100;
            var result = service.Match(request);
            this.PageMapper = result.Values.ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>获得元数据标签集合</summary>
        private void LoadMetadataTags(bool loadMetadataTagsFromConfig)
        {
            var service = GetService<ISearchServiceWrapper>();
            service.ResetServiceUrl();
            var tags = service.GetMetadataDefinitions();
            this.MetadataTags = new MetadataDefinitionCollection(tags);
            ExtToIndexDataTypeValueProvider.BuildMapper(this.MetadataTags[MetadataConsts.ShowType]);
            MetadataDefinitionCollection.Instance = this.MetadataTags;
            if (loadMetadataTagsFromConfig)
            {
                var tags2 = tags.ToList();
                RemoveMetadataTag(tags2, MetadataConsts.IIId);
                RemoveMetadataTag(tags2, MetadataConsts.IndexedDate);
                //RemoveMetadataTag(tags2, MetadataConsts.PageId);
                RemoveMetadataTag(tags2, MetadataConsts.DataId);
                LoadMetadataTags(tags2.ToArray());
            }
        }
        /// <summary>删除元数据标签</summary>
        private void RemoveMetadataTag(List<MetadataDefinition> tags, string tagName)
        {
            var tag = tags.FirstOrDefault(e => e.Name == tagName);
            if (tag != null) tags.Remove(tag);
        }
        /// <summary>加载元数据标签</summary>
        private void LoadMetadataTags(MetadataDefinition[] tags)
        {
            var xmlTags = this.Config.MetadataTags;
            foreach (var tag in tags)
            {
                var config = xmlTags.FirstOrDefault(e => e.Name == tag.Name);
                if (config != null)
                {
                    config.Refer = tag;
                    if (config.Width >= 100)
                    {
                        config.Width = 13;
                        if (tag.Type == MetadataTagType.Date.ToString()) config.Width = 21;
                    }
                    continue;
                }
                config = new XmlMetadataTag();
                config.Name = tag.Name;
                config.Refer = tag;
                config.Enabled = true;
                config.Width = 13;
                if (tag.Type == MetadataTagType.Date.ToString()) config.Width = 21;
                if (tag.Items.IsNullOrEmpty())
                {
                    config.EnumValues = string.Empty;
                }
                else
                {
                    var enumValues = tag.Items.Select(e => e.Text.Trim()).ToList();
                    config.EnumValues = string.Join(",", enumValues);
                }
                this.Config.MetadataTags.Add(config);
            }
            for (int i = 0; i < this.Config.MetadataTags.Count;)
            {
                if (this.Config.MetadataTags[i].Refer == null)
                {
                    this.Config.MetadataTags.RemoveAt(i);
                    continue;
                }
                i++;
            }
            BindMetadataTagDefaultValue(MetadataConsts.ShowType, $"{SubmissionConsts.ShowType.NormalizeVariable()}");
            BindMetadataTagDefaultValue(MetadataConsts.Title, $"{SubmissionConsts.ProductFileName.NormalizeVariable()}");
            BindMetadataTagDefaultValue(MetadataConsts.ResourceKey, $"/{SubmissionConsts.ToolName.NormalizeVariable()}/{SubmissionConsts.ExcelFileName.NormalizeVariable()}/{SubmissionConsts.Guid.NormalizeVariable()}");
        }
        /// <summary>绑定标签默认值</summary>
        private void BindMetadataTagDefaultValue(string tagName, string defaultValue)
        {
            var xmlTag = this.Config.MetadataTags.First(e => e.Name == tagName);
            if (xmlTag.DefaultValue.IsNullOrEmpty()) xmlTag.DefaultValue = defaultValue;
        }
        /// <summary>保存配置</summary>
        public void Save()
        {
            this.ConfigFile.XmlSerialize(this.Config);
        }
        /// <summary>生成Excel模板文件</summary>
        public void BuildExcelTemplateFile()
        {
            LoadFromApiService(false, true);
            var excelFile = this.Config.Product.ExcelFile;
            var excelSheet = ExcelUtil.CreateSheet(excelFile, "成果数据");
            var rowIndex = 0;
            //加入第一行标题
            var cellData = BuildExcelTemplateFirstTitleRow();
            excelSheet.AddRow(rowIndex++, cellData, true);
            //加入第二行标题
            cellData = BuildExcelTemplateSecondTitleRow();
            excelSheet.AddRow(rowIndex++, cellData, false);
            //生成合并单元格
            //excelSheet.BuildMergedRegion(0, 2, 0, 0);
            //excelSheet.BuildMergedRegion(0, 2, 1, 1);
            excelSheet.SetTitleStyle(0, 1);
            //加入成果文件
            foreach (var tag in this.Config.MetadataTags)
            {
                tag.CheckDefaultValue(this.Config.Variables);
            }
            var ptFiles = Directory.GetFiles(this.Config.Product.Folder, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < ptFiles.Length; i++)
            {
                var ext = ptFiles[i].GetExtension();
                cellData = BuildExcelTemplateDataRow(i, ptFiles[i], ext);
                excelSheet.AddRow(rowIndex++, cellData, false);
            }
            excelSheet.SetNumberStyle(2, 0);
            excelSheet.CreateFreezePane(3, 2);
            //设置列下拉列表
            var startRow = 2;
            var encodingCol = 1;
            var startCol = 4;
            BuildExcelTemplateDropdownList(excelSheet, startRow, encodingCol, startCol);
            //保存
            var excelWorkBook = excelSheet.Workbook;
            using (var stream = new FileStream(excelFile, FileMode.Create, FileAccess.Write))
            {
                excelWorkBook.Write(stream);
            }
            excelWorkBook.Close();
        }
        /// <summary>生成Excel模板第一标题行</summary>
        private List<CellProperty> BuildExcelTemplateFirstTitleRow()
        {
            var cellData = new List<CellProperty>();
            cellData.Add(new CellProperty() { Value = "序号", Span = 1, Width = 6 });
            cellData.Add(new CellProperty() { Value = "编码", Span = 1, Width = 8 });
            cellData.Add(new CellProperty() { Value = "成果文件", Span = 1, Width = 30 });
            cellData.Add(new CellProperty() { Value = "选项", Span = 1, Width = 12 });
            foreach (var tag in this.Config.MetadataTags)
            {
                var title = tag.Title;
                if (tag.Refer.Required) title += "(必需)";
                cellData.Add(new CellProperty() { Value = title, Span = 1, Width = tag.Enabled ? tag.Width : 0 });
            }
            return cellData;
        }
        /// <summary>生成Excel模板第二标题行</summary>
        private List<CellProperty> BuildExcelTemplateSecondTitleRow()
        {
            var cellData = new List<CellProperty>();
            cellData.Add(new CellProperty() { Value = SubmissionConsts.ST_ProductOrder, Span = 1 });
            cellData.Add(new CellProperty() { Value = SubmissionConsts.ST_TextEncoding, Span = 1 });
            cellData.Add(new CellProperty() { Value = SubmissionConsts.ST_ProductFile, Span = 1 });
            cellData.Add(new CellProperty() { Value = SubmissionConsts.ST_Options, Span = 1 });
            foreach (var tag in this.Config.MetadataTags)
            {
                cellData.Add(new CellProperty() { Value = tag.Name, Span = 1 });
            }
            return cellData;
        }
        /// <summary>生成Excel模板数据行</summary>
        private List<CellProperty> BuildExcelTemplateDataRow(int index, string ptFile, string ext)
        {
            var cellData = new List<CellProperty>();
            cellData.Add(new CellProperty() { Value = index + 1, Span = 1 });
            var encoding = ExtToIndexDataTypeValueProvider.GetEncoding(ext);
            cellData.Add(new CellProperty() { Value = encoding, Span = 1 });
            cellData.Add(new CellProperty() { Value = ptFile, Span = 1 });
            var options = ExtToIndexDataTypeValueProvider.GetOptions(ext);
            cellData.Add(new CellProperty() { Value = options, Span = 1 });
            var context = ptFile;
            foreach (var tag in this.Config.MetadataTags)
            {
                var property = new CellProperty() { Span = 1 };
                object value = null;
                if (tag.Variables != null)
                {
                    value = tag.BuildVariablesValue(context);
                }
                else if (tag.DefaultValue.Length > 0)
                {
                    value = tag.DefaultValue;
                }
                var tagType = MetadataTagType.String;
                if (!tag.Refer.Type.IsNullOrEmpty())
                {
                    tagType = tag.Refer.Type.ToEnum<MetadataTagType>();
                }
                switch (tagType)
                {
                    case MetadataTagType.Date:
                    case MetadataTagType.ISODate:
                        property.ValueType = typeof(DateTime);
                        if (value != null)
                        {
                            var dtValue = value.ToString().TryParseStandardString().GetValueOrDefault(DateTime.MinValue);
                            if (dtValue != DateTime.MinValue) value = dtValue;
                        }
                        break;
                    case MetadataTagType.Number:
                        property.ValueType = typeof(double);
                        if (value != null) value = double.Parse(value.ToString());
                        break;
                    default:
                        if (value == null) value = string.Empty;
                        break;
                }
                property.Value = value;
                cellData.Add(property);
            }
            return cellData;
        }
        /// <summary>生成Excel模板下拉列表</summary>
        private void BuildExcelTemplateDropdownList(ISheet excelSheet, int startRow, int encodingCol, int startCol)
        {
            excelSheet.SetColumnDropdownList(Utility.GetEncodings(true), startRow, encodingCol);
            for (int i = 0; i < this.Config.MetadataTags.Count; i++)
            {
                var tag = this.Config.MetadataTags[i];
                if (!tag.EnumValues.IsNullOrEmpty())
                {
                    excelSheet.SetColumnDropdownList(tag.GetEnumValues(), startRow, startCol + i);
                }
                if (tag.Refer.Type == MetadataTagType.Date.ToString())
                {
                    excelSheet.SetDateTimeStyle(startRow, startCol + i);
                }
                if (tag.Name == MetadataConsts.PageId)
                {
                    excelSheet.SetColumnDropdownList(this.PageMapper.Keys.ToArray(), startRow, startCol + i);
                }
            }
        }
        /// <summary>提交上下文</summary>
        public IndexSubmissionContext Context { get; set; }
        /// <summary>获得提交统计</summary>
        public string GetSubmissionStat()
        {
            var total = this.Context.Values.Count;
            var failure = this.Context.FailureValues.Count;
            var success = total - failure;
            return $"共提交了{total.ToString()}条索引数据,{success.ToString()}条提交成功,{failure.ToString()}条提交失败!";
        }
        /// <summary>批量提交</summary>
        public void BatchSubmit(frmIndexSubmissionPresenter presenter, bool parallel, bool retry)
        {
            var context = this.Context;
            LoadFromApiService(false, true);
            var title = string.Empty;
            if (retry)
            {
                context.Splash = presenter.WorkItem.Services.Get<ISplashService>();
                context.ResetProgress();
                context.Move();
                title = "重新提交成果数据...";
            }
            else
            {
                this.Context = context = new IndexSubmissionContext();
                context.View = presenter.View;
                context.Splash = presenter.WorkItem.Services.Get<ISplashService>();
                context.WorkItem = presenter.WorkItem;
                context.Values = new ConcurrentQueue<SubmissionProduct>();
                context.FailureValues = new ConcurrentQueue<SubmissionProduct>();
                context.ResetProgress();
                context.ProgressChanged += presenter.View.HandleProgressChanged;
                context.AppDataService = GetService<IAppDataServiceWrapper>();
                context.AppDataService.ResetServiceUrl();
                context.IndexerService = GetService<IIndexerService>();
                context.IndexerService.As<IApiServiceWrapper>().ResetServiceUrl();
                LoadProducts(context);
                title = "正在提交成果数据...";
            }
            var products = context.Values.ToArray();
            context.View.PopuldateProducts(products);
            context.StartProgress(title, products.Length);
            if (parallel)
            {
                context.Values.AsParallel<SubmissionProduct>()
                    .ForAll(product => UploadProduct(context, product));
            }
            else
            {
                while (context.Values.Count > 0)
                {
                    SubmissionProduct product;
                    if (!context.Values.TryDequeue(out product)) continue;
                    UploadProduct(context, product);
                }
            }
            context.FinishProgress("提交成果数据完成");
        }
        /// <summary>从Excel文件加载成果集合</summary>
        public void LoadProducts(IndexSubmissionContext context)
        {
            context.StartProgress("正在加载Excel文件...", -1);
            var excelFile = this.Config.Product.ExcelFile;
            var excelSheet = ExcelUtil.OpenFirst(excelFile);
            var excelTable = excelSheet.ToDataTable(1, 2);
            var orderColumn = excelTable.Columns[SubmissionConsts.ST_ProductOrder];
            var encodingColumn = excelTable.Columns[SubmissionConsts.ST_TextEncoding];
            var ptFileColumn = excelTable.Columns[SubmissionConsts.ST_ProductFile];
            var optionsColumn = excelTable.Columns[SubmissionConsts.ST_Options];
            var total = excelTable.Rows.Count;
            context.StartProgress("正在生成成果数据...", total);
            for (int i = 0; i < total; i++)
            {
                var row = excelTable.Rows[i];
                var product = new SubmissionProduct();
                product.ID = row[orderColumn].ConvertTo<int>();
                if (encodingColumn != null) product.CharSet = row[encodingColumn]?.ToString();
                product.File = row[ptFileColumn].ToString();
                product.Ext = product.File.GetExtension();
                if (optionsColumn != null) product.Options = row[optionsColumn]?.ToString();
                try
                {
                    product.IndexData = BuildIndexData(excelSheet, row);
                    BuildPageData(product.IndexData);
                    product.AppData = BuildAppData(product, product.IndexData);
                    context.Values.Enqueue(product);
                    context.NextProgress();
                }
                catch (Exception ex)
                {
                    var tag = ex.Data["IndexTag"]?.ToString() ?? string.Empty;
                    var message = $"序号={product.ID.ToString()},成果文件=[{product.File}]生成索引数据失败[标签={tag}]," + ex.Message;
                    throw new Exception(message, ex);
                }
            }
        }
        /// <summary>生成索引数据</summary>
        private Metadata BuildIndexData(ISheet excelSheet, DataRow row)
        {
            var metadata = new Metadata();
            var tags = this.MetadataTags;
            var cols = row.Table.Columns;
            foreach (DataColumn col in cols)
            {
                var tag = tags.GetValueBy(col.ColumnName);
                if (tag == null) continue;
                var tagValue = row[col];
                if (tagValue == null || tagValue == DBNull.Value) continue;
                try
                {
                    var tagType = MetadataTagType.String;
                    if (!tag.Type.IsNullOrEmpty()) tagType = tag.Type.ToEnum<MetadataTagType>();
                    switch (tagType)
                    {
                        case MetadataTagType.Date:
                        case MetadataTagType.ISODate:
                            if (tagValue is double)
                            {
                                tagValue = excelSheet.GetDateFromCell((double)tagValue).ToUniversalTime();
                            }
                            else
                            {
                                var dtValue = tagValue.ToString().Trim();
                                if (dtValue.Length == 0) continue;
                                tagValue = dtValue.Replace("/", "-").ToStandardDateTime().ToUniversalTime();
                            }
                            break;
                        case MetadataTagType.StringArray:
                            tagValue = tagValue.ToString().Split(',', '，');
                            break;
                        case MetadataTagType.String:
                            if (tag.Name == MetadataConsts.ShowType)
                            {
                                var tagItem = ExtToIndexDataTypeValueProvider.Mapper.GetValueBy(tagValue.ToString());
                                tagValue = tagItem == null ? tagValue.ToString() : tagItem.Value;
                            }
                            else
                            {
                                tagValue = tagValue.ToString();
                            }
                            break;
                    }
                    metadata.SetValue(tag.Name, tagValue);
                }
                catch (Exception ex)
                {
                    ex.Data["IndexTag"] = tag.Name;
                    throw;
                }
            }
            return metadata;
        }
        /// <summary>生成页面数据</summary>
        private void BuildPageData(Metadata indexData)
        {
            var pageData = this.PageMapper.GetValueBy(indexData.PageId);
            indexData.PageId = pageData == null ? "0" : pageData.PageId;
        }
        /// <summary>生成应用数据</summary>
        private AppDataSaveRequest BuildAppData(SubmissionProduct product, Metadata indexData)
        {
            var appData = new AppDataSaveRequest();
            appData.GenerateThumbnail = false;
            appData.GenerateFulltext = false;
            var fileName = Path.GetFileName(product.File);
            appData.Name = fileName;
            var indexShowType = indexData.ShowType.ToEnum<IndexShowType>(true);
            switch (indexShowType)
            {
                case IndexShowType.Html:
                    appData.ContentType = IndexAppContentType.Html;
                    break;
                case IndexShowType.Table:
                case IndexShowType.PropertyGrid:
                    appData.ContentType = IndexAppContentType.Json;
                    if (ExcelUtil.Support(product.Ext))
                    {
                        appData.Content = new object[] { ExcelBuilder.BuildTable(product) };
                    }
                    break;
                case IndexShowType.Chart:
                    appData.ContentType = IndexAppContentType.Json;
                    if (ExcelUtil.Support(product.Ext))
                    {
                        appData.Content = ExcelBuilder.BuildChart(product.File);
                    }
                    break;
                default:
                    appData.ContentType = IndexAppContentType.File;
                    appData.StorageType = FileStorageType.FileSystem;
                    appData.IsOnline = true;
                    if (indexData.Thumbnail.IsNullOrEmpty()) appData.GenerateThumbnail = true;
                    if (indexData.Fulltext.IsNullOrEmpty()) appData.GenerateFulltext = true;
                    break;
            }
            appData.Uploader = this.Config.ApiService.UserName;
            appData.System = indexData.System;
            appData.ResourceType = indexData.ResourceType;
            appData.ResourceKey = indexData.ResourceKey;
            return appData;
        }
        /// <summary>上传成果</summary>
        public void UploadProduct(IndexSubmissionContext context, SubmissionProduct product)
        {
            var file = product.File;
            try
            {
                product.AppData.UploadFileId = context.AppDataService.Upload(product.File, product.CharSet, this.UploadChunkSize);
                var appDataResult = context.AppDataService.Save(product.AppData);
                product.IndexData.DataId = appDataResult.DataId;
                if (product.AppData.GenerateThumbnail && !appDataResult.Thumbnail.IsNullOrEmpty())
                {
                    product.IndexData.Thumbnail = appDataResult.Thumbnail;
                }
                if (product.AppData.GenerateFulltext && !appDataResult.Fulltext.IsNullOrEmpty())
                {
                    product.IndexData.Fulltext = appDataResult.Fulltext;
                }
                var indexSaveRequest = new IndexSaveRequest();
                indexSaveRequest.Replace = true;
                indexSaveRequest.Metadatas = new MetadataCollection();
                indexSaveRequest.Metadatas.Add(product.IndexData);
                context.IndexerService.Save(indexSaveRequest);
                context.View.RefreshProductStatus(product, "提交成功", string.Empty);
            }
            catch (Exception ex)
            {
                ModuleBootstrapper.Error(this, nameof(UploadProduct), ex);
                context.FailureValues.Enqueue(product);
                context.View.RefreshProductStatus(product, "提交失败", ex.GetFullMessage());
            }
            context.NextProgress();
        }
        /// <summary>合并</summary>
        public void Merge(frmIndexSubmissionPresenter presenter, MergeContext context)
        {
            LoadFromApiService(false, false);
            var souceTable = LoadMergeSource(context);
            MergeSourceToTarget(context, souceTable);
        }
        /// <summary>加载源文件</summary>
        private DataTable LoadMergeSource(MergeContext context)
        {
            var workbook = WorkbookFactory.Create(context.MergeSourceFile);
            var excelSheet = workbook.GetSheetAt(0);
            return excelSheet.ToDataTable(context.MergeSourceFieldRow - 1, context.MergeSourceDataRow - 1);
        }
        /// <summary>合并</summary>
        private void MergeSourceToTarget(MergeContext context, DataTable sourceTable)
        {
            IWorkbook workbook = null;
            using (var stream = new FileStream(context.MergeTargetFile, FileMode.Open, FileAccess.Read))
            {
                workbook = WorkbookFactory.Create(stream);
            }
            var targetSheet = workbook.GetSheetAt(0);
            var targetFieldRowNum = 1;
            var targetFieldRow = targetSheet.GetRow(targetFieldRowNum);
            var targetProductFileColIndex = targetFieldRow.Cells.First(cell => cell != null && cell.StringCellValue == SubmissionConsts.ST_ProductFile).ColumnIndex;
            var mapper = new Dictionary<int, DataColumn>();
            foreach (var cell in targetFieldRow.Cells)
            {
                mapper[cell.ColumnIndex] = sourceTable.Columns.Cast<DataColumn>().FirstOrDefault(col => col.ColumnName == cell.StringCellValue);
            }
            var sourcePTFileTagColumn = sourceTable.Columns.Cast<DataColumn>().FirstOrDefault(col => col.ColumnName == SubmissionConsts.ST_ProductFile);
            var sourceTitleTagColumn = sourceTable.Columns.Cast<DataColumn>().First(col => col.ColumnName == MetadataConsts.Title);
            var targetStartRowIndex = 2;
            var targetLastRowNum = targetSheet.LastRowNum;
            var targetRows = new List<IRow>();
            for (int rowIndex = targetStartRowIndex; rowIndex <= targetLastRowNum; rowIndex++)
            {
                var row = targetSheet.GetRow(rowIndex);
                if (row == null) continue;
                targetRows.Add(row);
            }
            MergeSourceToTarget(context, targetRows, mapper, targetProductFileColIndex, sourceTable, sourcePTFileTagColumn, FindFullMerge);
            MergeSourceToTarget(context, targetRows, mapper, targetProductFileColIndex, sourceTable, sourceTitleTagColumn, FindFullMerge);
            MergeSourceToTarget(context, targetRows, mapper, targetProductFileColIndex, sourceTable, sourceTitleTagColumn, FindFuzzyMerge);
            if (sourceTable.Rows.Count > 0)
            {
                targetLastRowNum++;
                var resourceKeyColIndex = targetFieldRow.Cells.First(cell => cell != null && cell.StringCellValue == MetadataConsts.ResourceKey).ColumnIndex;
                var resourceKeyTag = this.Config.MetadataTags.Find(e => e.Name == MetadataConsts.ResourceKey);
                resourceKeyTag.CheckDefaultValue(this.Config.Variables);
                for (int i = 0; i < sourceTable.Rows.Count; i++)
                {
                    var row = sourceTable.Rows[i];
                    var cellData = BuildExcelMergeDataRow(mapper, row);
                    cellData[resourceKeyColIndex].Value = resourceKeyTag.BuildVariablesValue(null);
                    targetSheet.AddRow(targetLastRowNum + i, cellData, false);
                }
            }
            using (var stream = new FileStream(context.MergeTargetFile, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
            workbook.Close();
        }
        /// <summary>合并文件名完全匹配的数据</summary>
        private void MergeSourceToTarget(MergeContext context, List<IRow> targetRows, Dictionary<int, DataColumn> mapper, int targetProductFileColIndex, DataTable sourceTable, DataColumn sourceTitleTagColumn, Func<string, DataTable, DataColumn, DataRow> mergeValidator)
        {
            for (int rowIndex = 0; rowIndex < targetRows.Count; rowIndex++)
            {
                var row = targetRows[rowIndex];
                if (row == null) continue;
                var targetFileCell = row.Cells.First(cell => cell != null && cell.ColumnIndex == targetProductFileColIndex);
                var targetFileName = Path.GetFileName(targetFileCell.StringCellValue);
                var sourceRow = mergeValidator(targetFileName, sourceTable, sourceTitleTagColumn);
                if (sourceRow == null) continue;
                foreach (var pair in mapper)
                {
                    var dataColumn = pair.Value;
                    if (dataColumn == null || dataColumn.ColumnName == MetadataConsts.Title) continue;
                    var sourceValue = sourceRow[dataColumn];
                    if (sourceValue == null || sourceValue == DBNull.Value) continue;
                    var cell = row.GetCell(pair.Key);
                    if (cell == null) cell = row.CreateCell(pair.Key);
                    if (sourceValue is double)
                    {
                        cell.SetCellValue((double)sourceValue);
                    }
                    else if (sourceValue is bool)
                    {
                        cell.SetCellValue((bool)sourceValue);
                    }
                    else if (sourceValue is DateTime)
                    {
                        cell.SetCellValue((DateTime)sourceValue);
                    }
                    else
                    {
                        cell.SetCellValue(sourceValue.ToString());
                    }
                }
                sourceTable.Rows.Remove(sourceRow);
                targetRows[rowIndex] = null;
            }
            sourceTable.AcceptChanges();
        }
        /// <summary>完全匹配</summary>
        private DataRow FindFileNameMerge(string targetFileName, DataTable sourceTable, DataColumn sourceFileColumn)
        {
            foreach (DataRow row in sourceTable.Rows)
            {
                var sourceFileName = row[sourceFileColumn];
                if (sourceFileName == null || sourceFileName == DBNull.Value) continue;
                if (targetFileName.Equals(sourceFileName.ToString(), StringComparison.OrdinalIgnoreCase)) return row;
                var sourceFileName2 = Path.GetFileName(sourceFileName.ToString());
                if (targetFileName.Equals(sourceFileName2, StringComparison.OrdinalIgnoreCase)) return row;
            }
            return null;
        }
        /// <summary>完全匹配</summary>
        private DataRow FindFullMerge(string targetFileName, DataTable sourceTable, DataColumn sourceTitleTagColumn)
        {
            foreach (DataRow row in sourceTable.Rows)
            {
                var sourceFileName = row[sourceTitleTagColumn];
                if (sourceFileName == null || sourceFileName == DBNull.Value) continue;
                if (!targetFileName.Equals(sourceFileName.ToString(), StringComparison.OrdinalIgnoreCase)) continue;
                return row;
            }
            return null;
        }
        /// <summary>模糊匹配</summary>
        private DataRow FindFuzzyMerge(string targetFileName, DataTable sourceTable, DataColumn sourceTitleTagColumn)
        {
            var targetFileName2 = TrimFuzzyMerge(targetFileName);
            foreach (DataRow row in sourceTable.Rows)
            {
                var sourceFileName = row[sourceTitleTagColumn];
                if (sourceFileName == null || sourceFileName == DBNull.Value) continue;
                var sourceFileName2 = TrimFuzzyMerge(sourceFileName.ToString());
                if (targetFileName2.Equals(sourceFileName2, StringComparison.OrdinalIgnoreCase)) return row;
                var sourceExt = sourceFileName2.GetExtension();
                if (sourceExt.IsNullOrEmpty())
                {
                    var targetFileName3 = Path.GetFileNameWithoutExtension(targetFileName2);
                    if (targetFileName3.Equals(sourceFileName2, StringComparison.OrdinalIgnoreCase)) return row;
                }
            }
            return null;
        }
        /// <summary>模糊匹配</summary>
        private string TrimFuzzyMerge(string value, bool trimExt = false)
        {
            return value
                .Replace(" ", string.Empty)
                .Replace("　", string.Empty)
                .Replace("（", "(")
                .Replace("）", ")")
                ;
        }
        /// <summary>生成Excel合并数据行</summary>
        private List<CellProperty> BuildExcelMergeDataRow(Dictionary<int, DataColumn> mapper, DataRow row)
        {
            var cellData = new List<CellProperty>();
            var cellCount = mapper.Keys.Max() + 1;
            for (int i = 0; i < cellCount; i++)
            {
                cellData.Add(new CellProperty() { Value = null, Span = 1 });
            }
            foreach (var pair in mapper)
            {
                if (pair.Value == null) continue;
                var value = row[pair.Value];
                if (value == null || value == DBNull.Value) continue;
                cellData[pair.Key].Value = value;
            }
            return cellData;
        }
    }
}
