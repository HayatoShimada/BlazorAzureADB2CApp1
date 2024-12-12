﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorAzureADB2CApp1.Helpers;
using BlazorAzureADB2CApp1.Models;
using Microsoft.Extensions.Options;

namespace BlazorAzureADB2CApp1.Data
{
    public class CommentsContext
    {
        // make sure that appsettings.json is filled with the necessary details of the azure storage
        private readonly AzureStorageConfig _config;

        public CommentsContext(IOptions<AzureStorageConfig> config)
        {
            _config = config.Value;

        }

        public List<Comment>? Comments { get; set; }

        public async Task<List<Comment>> GetComments()
        {
            List<CommentBlobDTO> blobs = await StorageHelper.GetBlobs();

            List<Comment> comments = new List<Comment>();
            foreach (CommentBlobDTO blob in blobs)
            {
                Comment comment = new Comment();
                comment.Name = blob.Name;
                comment.UserComment = blob.Contents;

                comments.Add(comment);
            }

            return comments;
        }

        public async Task CreateComment(Comment comment)
        {
            await StorageHelper.UploadBlob(comment.Name, comment.UserComment);
        }

        public async Task DeleteComment(Comment comment)
        {

            await StorageHelper.DeleteBlob(comment.Name);
        }
    }
}