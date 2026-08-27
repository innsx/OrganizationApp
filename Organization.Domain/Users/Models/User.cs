using Organization.Domain.Commons.BaseEntity;
using Organization.Domain.Commons.Utilities;

namespace Organization.Domain.Users.Models
{
    [TableName("tblUserDetails")]
    public class User : IDbEntity
    {
        [PrimaryKey]
        [ColumnName("Id")]
        public string Id { get; set; } = ShortGuid.NewGuid();

        [ColumnName("UserName")]
        public string UserName { get; set; } = string.Empty;

        [DistinctUniqueKey]
        [ColumnName("Email")]
        public string Email { get; set; } = string.Empty;

        [ColumnName("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [ColumnName("RefreshToken")]
        public string? RefreshToken { get; set; } = string.Empty;

        [ColumnName("RefreshTokenExpiryDate")]
        public DateTime? RefreshTokenExpiryDate { get; set; }

    }

}
