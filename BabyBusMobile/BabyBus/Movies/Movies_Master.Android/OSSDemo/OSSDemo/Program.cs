using System;
using System.IO;
using Aliyun.OpenServices.OpenStorageService;

namespace OSSDemo
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine ("Hello World!");

            string bucketName = "babybus-image";
            string filePath = "../../icon-29.png";
            string filename = "icon-29-Bcc.png";


            PutObject (bucketName, filename, filePath);

//            test.MutiPartUpload ();
        }

        public static void PutObject (string bucketName, string filename, string filePath)
        {
            //access key id
            //oMhFxiUEplUV9xIt
            //access key secret
            //OZfbMNMOP8iHNJOvvZld1ZNFGcdijj

            // 初始化OSSClient oss-cn-qingdao
            string secret = "OZfbMNMOP8iHNJOvvZld1ZNFGcdijj";
//            string endpoint = "http://babybus.emolbase.com/";
//            oss-cn-qingdao.aliyuncs.com  
//            string endpoint = "http://oss-cn-qingdao.aliyuncs.com/";
//            string endpoint = "babybus-image.oss-cn-qingdao.aliyuncs.com";
            string endpoint = "http://oss-cn-qingdao.aliyuncs.com"; 
            http://oss-cn-qingdao.aliyuncs.com／icon-29.png
            string key = "oMhFxiUEplUV9xIt";
            var uri = new Uri (endpoint, UriKind.Absolute);

            OssClient client = new OssClient (uri, key, secret);
           
            // 创建上传Object的Metadata
            ObjectMetadata meta = new ObjectMetadata ();

            // 获取指定文件的输入流
            try {
                byte[] data = File.ReadAllBytes(filePath);
                MemoryStream content = new MemoryStream(data);

//                var fileMode = FileMode.Open;
//                var content = new FileStream (filePath, fileMode, FileAccess.Read,
//                                  FileShare.Delete);
                // 必须设置ContentLength
                meta.ContentLength = (content.Length);
                // 上传Object.
                PutObjectResult result = client.PutObject (bucketName, filename, content, meta);
                // 打印ETag
                Console.WriteLine (result.ETag);

                content.Flush ();
               
                content.Close ();
            } catch (Exception e) {


                Console.WriteLine (e.Message);
            }
            Console.ReadKey ();
        }

    }
}



//oss-cn-qingdao:青岛数据中心

//注意防止盗链：refer和header中得权限
