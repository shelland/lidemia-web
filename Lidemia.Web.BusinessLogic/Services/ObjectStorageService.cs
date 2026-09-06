// Created on 11/12/2025 23:08 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Misc;
using Lidemia.Web.BusinessLogic.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Tags;
using Scrutor;

namespace Lidemia.Web.BusinessLogic.Services;

[ServiceDescriptor<IObjectStorageService>(ServiceLifetime.Singleton)]
public class ObjectStorageService : IObjectStorageService
{
    private readonly IMinioClient minioClient;
    private readonly IObjectStoragePathResolver pathResolver;
    private readonly ILogger<ObjectStorageService> logger;

    public ObjectStorageService(
        IMinioClient minioClient,
        IObjectStoragePathResolver pathResolver,
        ILogger<ObjectStorageService> logger
    )
    {
        this.minioClient = minioClient;
        this.pathResolver = pathResolver;
        this.logger = logger;
    }

    public async Task<Stream> GetFileContent(string bucket, string path, CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(path)
            .WithCallbackStream(async (stream, token) =>
            {
                await stream.CopyToAsync(memoryStream, token);
            });

        await this.minioClient.GetObjectAsync(args, cancellationToken);
        return memoryStream;
    }

    public async Task<ObjectStorageUploadResult> Upload(Stream content, ObjectStoragePayloadType type, string? fileName = null, IDictionary<string, string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        fileName ??= Guid.NewGuid().ToString();

        var length = content.Length;
        var path = this.pathResolver.Resolve(type, fileName);

        await EnsureBucketExists(path.BucketName, cancellationToken);

        var args = new PutObjectArgs()
            .WithStreamData(content)
            .WithObjectSize(length)
            .WithBucket(path.BucketName)
            .WithObject(path.Path);

        if (tags != null)
        {
            args.WithTagging(Tagging.GetObjectTags(tags));
        }

        var response = await this.minioClient.PutObjectAsync(args, cancellationToken);

        this.logger.LogInformation("A new object uploaded to {Bucket}/{Path} with status {Code}", path.BucketName, path.Path, response.ResponseStatusCode);
        return new ObjectStorageUploadResult(path.BucketName, response.ObjectName);
    }

    public async Task Delete(string bucket, string path, CancellationToken cancellationToken)
    {
        var args = new RemoveObjectArgs().WithBucket(bucket).WithObject(path);
        await this.minioClient.RemoveObjectAsync(args, cancellationToken);
    }

    private async Task EnsureBucketExists(string bucketName, CancellationToken cancellationToken)
    {
        var existsArgs = new BucketExistsArgs().WithBucket(bucketName);
        var isBucketExists = await this.minioClient.BucketExistsAsync(existsArgs, cancellationToken);

        if (isBucketExists)
        {
            return;
        }

        var makeArgs = new MakeBucketArgs().WithBucket(bucketName);
        await this.minioClient.MakeBucketAsync(makeArgs, cancellationToken);

        this.logger.LogInformation("A new storage bucket created: {Name}", bucketName);
    }
}