using Azure.Storage.Blobs;
using System;
using System.Threading.Tasks;

namespace FunctionApp.Services
{
    public interface ICreateSASBlobService
    {
        Uri CreateSASBlob(
    BlobClient blobClient,
    string storedPolicyName = null);
    }
}