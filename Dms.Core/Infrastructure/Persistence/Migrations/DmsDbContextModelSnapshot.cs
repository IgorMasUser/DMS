﻿// <auto-generated />
using System;
using Dms.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dms.Core.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(DmsDbContext))]
    partial class DmsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dms.Core.Domain.Entities.FileAccounter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FileAccounters");
                });

            modelBuilder.Entity("Dms.Core.Domain.Entities.FileData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("FileAccounterId")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FileAccounterId");

                    b.ToTable("FilesData");
                });

            modelBuilder.Entity("Dms.Core.Domain.Entities.FileData", b =>
                {
                    b.HasOne("Dms.Core.Domain.Entities.FileAccounter", "FileAccounter")
                        .WithMany("files")
                        .HasForeignKey("FileAccounterId");

                    b.Navigation("FileAccounter");
                });

            modelBuilder.Entity("Dms.Core.Domain.Entities.FileAccounter", b =>
                {
                    b.Navigation("files");
                });
#pragma warning restore 612, 618
        }
    }
}
