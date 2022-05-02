﻿namespace Gryzilla_App.Repositories.Interfaces;

public interface ILikesPostDbRepository
{
    public Task<string?> AddLikeToPost(int idUser, int idPost);
    public Task<string?> DeleteLikeToPost(int idUser, int idPost);
}