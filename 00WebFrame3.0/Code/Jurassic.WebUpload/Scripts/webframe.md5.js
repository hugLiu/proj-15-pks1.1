//此文件依赖于spart.md5.js
//计算本地文件的md5码
//file-本地文件名
// endcallback - function(md5) 计算完的回调， 参数就是得到的md5码
// pcallback -  function(percent) 分片计算时，每一片计算完的回调，参数是一个小于1的百分数，通常用于进度条
$.sparkmd5 = function (file, endcallback, pcallback) {
    var fileReader = new FileReader();
    //文件分割方法（注意兼容性）
    blobSlice = File.prototype.mozSlice || File.prototype.webkitSlice || File.prototype.slice;

    //文件每块分割2M，计算分割详情
    chunkSize = 2097152;
    chunks = Math.ceil(file.size / chunkSize);
    currentChunk = 0;

    //创建md5对象（基于SparkMD5）
    var spark = new SparkMD5.ArrayBuffer();

    //每块文件读取完毕之后的处理
    fileReader.onload = function (e) {
        console.log("读取文件", currentChunk + 1, "/", chunks);

        pcallback && pcallback((currentChunk + 1) / chunks);
        //每块交由sparkMD5进行计算
        spark.append(e.target.result);
        currentChunk++;

        //如果文件处理完成计算MD5，如果还有分片继续处理
        if (currentChunk < chunks) {
            loadNext();
        } else {
            endcallback && endcallback(spark.end());
        }
    };

    //处理单片文件的上传
    function loadNext() {
        var start = currentChunk * chunkSize;
        var end = start + chunkSize >= file.size ? file.size : start + chunkSize;

        fileReader.readAsArrayBuffer(blobSlice.call(file, start, end));
    }
    loadNext();

};