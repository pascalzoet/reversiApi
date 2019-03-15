﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReversiApi.Dal;

namespace ReversiApi.Migrations.Game
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReversiApi.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Board");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("GameToken");

                    b.Property<int>("OnSet");

                    b.Property<string>("PlayerBlackToken");
                
                    b.Property<string>("PlayerWhiteToken");

                    b.Property<string>("GameStatus");

                    b.Property<int?>("Winner");

                    b.HasKey("GameId");

                    b.ToTable("Game");
                });
#pragma warning restore 612, 618
        }
    }
}
