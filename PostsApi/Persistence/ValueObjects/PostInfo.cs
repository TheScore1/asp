using System.ComponentModel.DataAnnotations.Schema;
using CSharpFunctionalExtensions;

namespace Persistence.ValueObjects;

public record PostInfo
{
	public const int TITLE_MAX_LENGTH = 50;
	public const int TEXT_MAX_LENGTH = 50;

	private PostInfo() {}
	
	public required string Title { get; init; }
	public required string Text { get; init; }

	public static Result<PostInfo> Create(string title, string text)
	{
		if (string.IsNullOrEmpty(title) || title.Length > TITLE_MAX_LENGTH)
			return Result.Failure<PostInfo>($"Title cant be empty or longer than {TITLE_MAX_LENGTH} symbols");
		
		if (string.IsNullOrEmpty(text) || text.Length > TEXT_MAX_LENGTH)
			return Result.Failure<PostInfo>($"Text cant be empty or longer than {TITLE_MAX_LENGTH} symbols");

		return new PostInfo()
		{
			Title = title,
			Text = text
		};
	}
}