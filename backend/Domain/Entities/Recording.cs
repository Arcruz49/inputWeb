using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InputWeb.Domain.Entities;

[Table("recordings")]
public class Recording
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("project_name")]
    [MaxLength(200)]
    public string ProjectName { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("video_url")]
    public string VideoUrl { get; set; } = string.Empty;

    [Column("events_url")]
    public string EventsUrl { get; set; } = string.Empty;

    [Column("hash")]
    public string Hash { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}