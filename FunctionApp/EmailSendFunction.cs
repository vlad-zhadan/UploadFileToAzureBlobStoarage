using System;
using System.Collections.Generic;
using System.IO;
using Azure.Storage.Blobs;
using FunctionApp.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

    namespace TrigerFunction    
    {
        [StorageAccount("BlobConectionString")]
        public class EmailSendFunction
        {
            private readonly ISendEmaisService _sendEmaisService;
           

        public EmailSendFunction(ISendEmaisService sendEmaisService)
            {
                _sendEmaisService = sendEmaisService;             
            }

            [FunctionName("EmailSendFunction")]
            public void Run(
                [BlobTrigger("files/{name}")] Stream myBlob, 
                string name,
                IDictionary<string, string> metadata,
                ILogger log)
            {
            if (metadata.TryGetValue("Email", out string email))
            {
                log.LogInformation($"Blob Email: {email}");
            }

            if (metadata.TryGetValue("Name", out string actualName))
            {
                log.LogInformation($"Actual name: {actualName}");
            }

            if (metadata.TryGetValue("Uri", out string uri))
            {
                log.LogInformation($"Uri: {uri}");
            }

            // get metadata
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            

         
                _sendEmaisService.SendEmail(email, uri);
            }
        }
    }
 