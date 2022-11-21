﻿namespace UnitTest;

public class DatabaseSql
{
    public static string GetTruncateSql()
    {
        return "TRUNCATE TABLE [Friends]" +
                          "TRUNCATE TABLE [BlockedUser] " +
                          "TRUNCATE TABLE [AchievementUser]" +
                          "DELETE FROM [Achievement]" + 
                          "DBCC CHECKIDENT ([Achievement], RESEED, 0)" + 
                          "TRUNCATE TABLE [Blocked]" + 
                          "TRUNCATE TABLE [Notification]" +
                          "TRUNCATE TABLE [ProfileComment]" +
                          "TRUNCATE TABLE [GroupUser]" +
                          "DELETE FROM [Group]" +
                          "DBCC CHECKIDENT ([Group], RESEED, 0)" +
                          "TRUNCATE TABLE [LikePost]" +
                          "TRUNCATE TABLE [LikeArticle]" +
                          "TRUNCATE TABLE [PostTag]" +
                          "TRUNCATE TABLE [ArticleTag]" +
                          "DELETE FROM [Tag]" +
                          "DBCC CHECKIDENT ([Tag], RESEED, 0)" +
                          "TRUNCATE TABLE [ReportCommentArticle]" +
                          "TRUNCATE TABLE [ReportCommentPost]" +
                          "TRUNCATE TABLE [ReportPost]" +
                          "DELETE FROM [CommentPost]" +
                          "DBCC CHECKIDENT ([CommentPost], RESEED, 0)" +
                          "DELETE FROM [CommentArticle]" +
                          "DBCC CHECKIDENT ([CommentArticle], RESEED, 0)" +
                          "DELETE FROM [Reason]" +
                          "DBCC CHECKIDENT ([Reason], RESEED, 0)" +
                          "DELETE FROM [Post]" +
                          "DBCC CHECKIDENT ([Post], RESEED, 0)" +
                          "DELETE FROM [Article]" +
                          "DBCC CHECKIDENT ([Article], RESEED, 0)" +
                          "DELETE FROM [UserData]" +
                          "DBCC CHECKIDENT ([UserData], RESEED, 0)" +
                          "DELETE FROM [Rank]" +
                          "DBCC CHECKIDENT ([Rank], RESEED, 0)";
    }
}