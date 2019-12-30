﻿// <auto-generated />
using ChessGame.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChessGame.Migrations
{
    [DbContext(typeof(SqlServerDbContext))]
    [Migration("20191228135735_board2")]
    partial class board2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChessGame.Models.Board", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("User1Color")
                        .HasColumnType("int");

                    b.Property<string>("User1Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User1Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("User2Color")
                        .HasColumnType("int");

                    b.Property<string>("User2Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User2Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("boards");
                });
#pragma warning restore 612, 618
        }
    }
}
