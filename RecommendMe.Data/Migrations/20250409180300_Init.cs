using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendMe.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Poster = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    MediaDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.MediaId);
                });

            migrationBuilder.CreateTable(
                name: "RatingList",
                columns: table => new
                {
                    RatingListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingList", x => x.RatingListId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    SourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RssUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.SourceId);
                });

            migrationBuilder.CreateTable(
                name: "RatingListMedia",
                columns: table => new
                {
                    RatingListMediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    MediaId = table.Column<int>(type: "int", nullable: false),
                    RatingListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingListMedia", x => x.RatingListMediaId);
                    table.ForeignKey(
                        name: "FK_RatingListMedia_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RatingListMedia_RatingList_RatingListId",
                        column: x => x.RatingListId,
                        principalTable: "RatingList",
                        principalColumn: "RatingListId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositivityRate = table.Column<double>(type: "float", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "SourceId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalCommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_OriginalCommentId",
                        column: x => x.OriginalCommentId,
                        principalTable: "Comment",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Comment_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    FormId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.FormId);
                    table.ForeignKey(
                        name: "FK_Form_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CommentRating",
                columns: table => new
                {
                    CommentRatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Like = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentRating", x => x.CommentRatingId);
                    table.ForeignKey(
                        name: "FK_CommentRating_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CommentRating_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RatingListComment",
                columns: table => new
                {
                    RatingListCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingListId = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingListComment", x => x.RatingListCommentId);
                    table.ForeignKey(
                        name: "FK_RatingListComment_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RatingListComment_RatingList_RatingListId",
                        column: x => x.RatingListId,
                        principalTable: "RatingList",
                        principalColumn: "RatingListId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RatingListRating",
                columns: table => new
                {
                    RatingListRatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Like = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingListRating", x => x.RatingListRatingId);
                    table.ForeignKey(
                        name: "FK_RatingListRating_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.    NoAction);
                    table.ForeignKey(
                        name: "FK_RatingListRating_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SourceId",
                table: "Articles",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_OriginalCommentId",
                table: "Comment",
                column: "OriginalCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId1",
                table: "Comment",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_CommentRating_CommentId",
                table: "CommentRating",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentRating_UserId1",
                table: "CommentRating",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Form_UserId1",
                table: "Form",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_RatingListComment_CommentId",
                table: "RatingListComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingListComment_RatingListId",
                table: "RatingListComment",
                column: "RatingListId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingListMedia_MediaId",
                table: "RatingListMedia",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingListMedia_RatingListId",
                table: "RatingListMedia",
                column: "RatingListId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingListRating_CommentId",
                table: "RatingListRating",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingListRating_UserId1",
                table: "RatingListRating",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "CommentRating");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropTable(
                name: "RatingListComment");

            migrationBuilder.DropTable(
                name: "RatingListMedia");

            migrationBuilder.DropTable(
                name: "RatingListRating");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "RatingList");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
