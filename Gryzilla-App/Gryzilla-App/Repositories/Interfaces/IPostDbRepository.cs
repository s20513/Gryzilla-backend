﻿using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Post;
using Gryzilla_App.DTOs.Responses.Posts;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IPostDbRepository
{
    public Task<IEnumerable<PostDto>?> GetPostsFromDb();
    public Task<PostQtyDto?> GetQtyPostsFromDb(int qtyPosts);
    public Task<PostQtyDto?> GetQtyPostsByLikesFromDb(int qtyPosts);
    public Task<PostQtyDto?> GetQtyPostsByCommentsFromDb(int qtyPosts);
    public Task<PostQtyDto?> GetQtyPostsByDateFromDb(int qtyPosts);
    public Task<PostQtyDto?> GetQtyPostsByDateOldestFromDb(int qtyPosts);
    public Task<IEnumerable<PostDto>?> GetPostsByLikesFromDb();
    public Task<IEnumerable<PostDto>?> GetPostsByCommentsFromDb();
    public Task<IEnumerable<PostDto>?> GetPostsByDateFromDb();
    public Task<IEnumerable<PostDto>?> GetPostsByDateOldestFromDb();
    public Task<NewPostDto?> AddNewPostToDb(AddPostDto addPostDto);
    public Task<DeletePostDto?> DeletePostFromDb(int idPost);
    public Task<DeleteTagDto?> DeleteTagFromPost(int idPost, int idTag);
    public Task<ModifyPostDto?> ModifyPostFromDb(PutPostDto putPostDto, int idPost);
    public Task<OnePostDto?> GetOnePostFromDb(int idPost);
}