using System;
using Aliyun.OpenServices.OpenStorageService;
using System.IO;
using System.Collections.Generic;

namespace OSSDemo
{
    public class test
    {
        public static void MutiPartUpload ()
        {
            string bucketName = "babybus";
            string accessId = "oMhFxiUEplUV9xIt";
            string accessKey = "OZfbMNMOP8iHNJOvvZld1ZNFGcdijj";
            string localFile = "../../icon-29.png";
            string uploadFileKey = "icon-29";

//            string endpoint = "babybus.emolbase.com";
//            string endpoint = "www.oss-cn-qingdao.aliyuncs.com";


//
//            var uri = new Uri (endpoint, UriKind.RelativeOrAbsolute
//                      );

            OssClient ossClient = new OssClient (accessId, accessKey);

            InitiateMultipartUploadRequest initRequest = new InitiateMultipartUploadRequest (bucketName, uploadFileKey);
            var initResult = ossClient.InitiateMultipartUpload (initRequest);

            // ÉèÖÃÃ¿¿éÎª 5M
            int partSize = 1024 * 1024 * 5;

            FileInfo partFile = new FileInfo (localFile);

            // ¼ÆËã·Ö¿éÊýÄ¿
            int partCount = (int)(partFile.Length / partSize);
            if (partFile.Length % partSize != 0) {
                partCount++;
            }

            // ÐÂ½¨Ò»¸öList±£´æÃ¿¸ö·Ö¿éÉÏ´«ºóµÄETagºÍPartNumber
            List<PartETag> partETags = new List<PartETag> ();
            FileStream fs = null;
            try {
                // »ñÈ¡ÎÄ¼þÁ÷
                fs = new FileStream (partFile.FullName, FileMode.Open);
                for (int i = 0; i < partCount; i++) {
                    // Ìøµ½Ã¿¸ö·Ö¿éµÄ¿ªÍ·
                    long skipBytes = partSize * i;
                    fs.Position = skipBytes;

                    // ¼ÆËãÃ¿¸ö·Ö¿éµÄ´óÐ¡
                    long size = partSize < partFile.Length - skipBytes ?
                        partSize : partFile.Length - skipBytes;

                    // ´´½¨UploadPartRequest£¬ÉÏ´«·Ö¿é
                    UploadPartRequest uploadPartRequest = new UploadPartRequest (bucketName, uploadFileKey, initResult.UploadId);
                    uploadPartRequest.InputStream = fs;
                    uploadPartRequest.PartSize = size;
                    uploadPartRequest.PartNumber = (i + 1);
                    UploadPartResult uploadPartResult = ossClient.UploadPart (uploadPartRequest);

                    // ½«·µ»ØµÄPartETag±£´æµ½ListÖÐ¡£
                    partETags.Add (uploadPartResult.PartETag);
                }
            } finally {
                // ¹Ø±ÕÎÄ¼þ
                fs.Close ();
            }
            CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest (bucketName, uploadFileKey, initResult.UploadId);
            foreach (PartETag partETag in partETags) {
                completeRequest.PartETags.Add (partETag);
            }
            var completeResult = ossClient.CompleteMultipartUpload (completeRequest);
            Console.WriteLine (completeResult.ETag);
        }
    }
}

