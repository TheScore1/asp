namespace Api.Controllers.Post.Dto;

public record PostDto(Guid Id, Guid UserId, string Title, string Text, DateTime DateCreated);