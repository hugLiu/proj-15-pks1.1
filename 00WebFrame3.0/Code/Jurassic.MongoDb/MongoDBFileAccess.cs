using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;

namespace Jurassic.MongoDb
{
    /// <summary>
    /// mongodb文件管理
    /// </summary>
    public class MongoDBFileAccess
    {
        
        private string _connectStr = string.Empty;
        private string _database = string.Empty;
        private string _collection = string.Empty;

        public MongoDBFileAccess(string connectStr, string database, string collection)
        {
            _connectStr = connectStr;
            _database = database;
            _collection = collection;
        }

        private IMongoDatabase GetDatabase()
        {
            MongoClient client = new MongoClient(_connectStr);
            IMongoDatabase database = client.GetDatabase(_database);

            return database;
        }

        private IGridFSBucket GetGridFSBucket(string bucketName = null)
        {
            MongoClient client = new MongoClient(_connectStr);
            IMongoDatabase database = client.GetDatabase(_database);

            IGridFSBucket bucket;

            bucketName = "geobankfile";

            if(bucketName == null)
            {
                bucket = new GridFSBucket(database);
            }
            else
            {
                bucket = new GridFSBucket(database, new GridFSBucketOptions
                {
                    BucketName = bucketName,
                    ChunkSizeBytes = 1048576, // 1MB
                    //WriteConcern = WriteConcern.WMajority,
                    //ReadConcern = ReadConcern.Majority,
                    //ReadPreference = ReadPreference.Secondary
                });
            }
            
            
            return bucket;
        }


        public string UploadingFile(string filename, Stream source, GridFSUploadOptions options = null)
        {

            var bucket = GetGridFSBucket();

            //string filename = Guid.NewGuid().ToString();

            var id = bucket.UploadFromStream(filename, source, options);

            return id.ToString();
        }
        public async Task<string> UploadingFile_Async(string filename, Stream source, GridFSUploadOptions options = null)
        {
            
            var bucket = GetGridFSBucket();

            //string filename = Guid.NewGuid().ToString();

            var id = await bucket.UploadFromStreamAsync(filename, source, options);

            return id.ToString();
        }

        public Stream DownloadingFile(string id)
        {

            IGridFSBucket bucket = GetGridFSBucket();

            ObjectId objectId = new ObjectId(id);
            
            GridFSDownloadStream<ObjectId> stream = bucket.OpenDownloadStream(objectId);

            return stream;


        }
        private async void DownloadingFile_Async(string id, Stream destination)
        {

            IGridFSBucket bucket = GetGridFSBucket();

            ObjectId objectId = new ObjectId(id);

            bucket.DownloadToStream(objectId, destination);  //The driver does not close the Stream when it is done. 


        }
        
        /// <summary>
        /// 查找一个文件信息
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        public GridFSFileInfo FindingFile(string filename)  //ObjectId objectId
        {
            //ObjectId objectId = new ObjectId(id);

            IGridFSBucket bucket = GetGridFSBucket();
            
            //FilterDefinition<GridFSFileInfo> filter = Builders<GridFSFileInfo>.Filter.Where(t => t.Id.ToString() == id);
            //FilterDefinition<GridFSFileInfo> filter = Builders<GridFSFileInfo>.Filter.Eq(t => t.Id, objectId);
            FilterDefinition<GridFSFileInfo> filter = Builders<GridFSFileInfo>.Filter.Eq(t => t.Filename, filename);
            SortDefinition<GridFSFileInfo> sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            var options = new GridFSFindOptions
            {
                Limit = 1,
                Sort = sort
            };
            
            //var cursor = bucket.Find(filter, options);
            var cursor = bucket.Find(filter);
            var fileInfo = cursor.ToList().FirstOrDefault();

            cursor.Dispose();

            return fileInfo;

        }
        //Each file stored in GridFS has a unique Id assigned to it, and that is the primary way of accessing the stored files.
        public List<GridFSFileInfo> FindingAllFiles()
        {
            //For example, to find the newest revision of the file named “securityvideo” uploaded in January 2015:

            IGridFSBucket bucket = GetGridFSBucket();

            //FilterDefinition<GridFSFileInfo> filter = Builders<GridFSFileInfo>.Filter.And(
            //    Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, "securityvideo"),
            //    Builders<GridFSFileInfo>.Filter.Gte(x => x.UploadDateTime, new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            //    Builders<GridFSFileInfo>.Filter.Lt(x => x.UploadDateTime, new DateTime(2015, 2, 1, 0, 0, 0, DateTimeKind.Utc)));

            //SortDefinition<GridFSFileInfo> sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);

            //var options = new GridFSFindOptions
            //{
            //    Limit = 1,
            //    Sort = sort
            //};

            //using (var cursor = bucket.Find(filter, options))
            //{
            //    var fileInfo = cursor.ToList().FirstOrDefault();
            //    // fileInfo either has the matching file information or is null
            //}

            //using (var cursor = await bucket.FindAsync(filter, options))
            //{
            //    var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
            //    // fileInfo either has the matching file information or is null
            //}

            FilterDefinition<GridFSFileInfo> filter = Builders<GridFSFileInfo>.Filter.Empty;

            var cursor = bucket.Find(filter);
            var fileInfos = cursor.ToList();
            cursor.Dispose();
            return fileInfos;

        }
        public IEnumerable<GridFSFileInfo> GetFilesEnumerable()
        {
            IGridFSBucket bucket = GetGridFSBucket();

            FilterDefinition<GridFSFileInfo> filter = Builders<GridFSFileInfo>.Filter.Empty;

            var cursor = bucket.Find(filter).ToEnumerable();

            return cursor;
        }
        
        /// <summary>
        /// Use the Delete or DeleteAsync methods to delete a single file identified by its Id.
        /// </summary>
        public void DeletingFiles(ObjectId id)
        {
            //For example, to find the newest revision of the file named “securityvideo” uploaded in January 2015:

            IGridFSBucket bucket = GetGridFSBucket();

            bucket.Delete(id);
            //await bucket.DeleteAsync(id);
        }
        public void DropingAllFiles()
        {
            IGridFSBucket bucket = GetGridFSBucket();

            bucket.Drop();
        }
        public void DeletingAllFiles(string filename)
        {
            
            IGridFSBucket bucket = GetGridFSBucket();

            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, filename);

            var filesCursor = bucket.Find(filter);
            var files = filesCursor.ToList();

            foreach (var file in files)
            {
                bucket.Delete(file.Id);
            }

        }


        /// <summary>
        /// Use the Drop or DropAsync methods to drop an entire GridFS bucket at once.
        /// Note 
        /// The “fs.files” collection will be dropped first, followed by the “fs.chunks” collection. 
        /// This is the fastest way to delete all files stored in a GridFS bucket at once. 
        /// </summary>
        /// <param name="id"></param>
        public void DroppingFiles()
        {
            
            IGridFSBucket bucket = GetGridFSBucket();

            bucket.Drop();

        }

        /// <summary>
        /// Use the Rename or RenameAsync methods to rename a single file identified by its Id.
        /// </summary>
        public void RenamingFiles(ObjectId id, string newFilename)
        {
            //For example, to find the newest revision of the file named “securityvideo” uploaded in January 2015:

            IGridFSBucket bucket = GetGridFSBucket();

            bucket.Rename(id, newFilename);
            //await bucket.RenameAsync(id, newFilename);

        }

        /// <summary>
        /// Renaming all revisions of a file
        /// To rename all revisions of a file you first use the Find or FindAsync method to find all the revisions, 
        /// and then loop over the revisions and use the Rename or RenameAsync method to rename each revision one at a time.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newFilename"></param>

        public void RenamingAllFiles(string oldFilename, string newFilename)
        {
            //For example, to find the newest revision of the file named “securityvideo” uploaded in January 2015:

            IGridFSBucket bucket = GetGridFSBucket();

            //string oldFilename;
            //string newFilename;
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, oldFilename);

            var filesCursor = bucket.Find(filter);
            var files = filesCursor.ToList();

            foreach (var file in files)
            {
                bucket.Rename(file.Id, newFilename);
            }

            //var filesCursor = await bucket.FindAsync(filter);
            //var files = await filesCursor.ToListAsync();

            //foreach (var file in files)
            //{
            //    await bucket.RenameAsync(file.Id, newFilename);
            //}

        }
    }
}
