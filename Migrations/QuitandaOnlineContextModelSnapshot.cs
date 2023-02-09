﻿// <auto-generated />
using System;
using AspNetCoreWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace QuitandaOnline.Migrations
{
    [DbContext(typeof(QuitandaOnlineContext))]
    partial class QuitandaOnlineContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.13");

            modelBuilder.Entity("AspNetCoreWebApp.Models.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Situacao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("TEXT");

                    b.HasKey("IdCliente");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Favorito", b =>
                {
                    b.Property<int>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("TEXT");

                    b.HasKey("IdCliente", "IdProduto");

                    b.HasIndex("IdProduto");

                    b.ToTable("Favoritos");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.ItemPedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Quantidade")
                        .HasColumnType("REAL");

                    b.Property<decimal>("ValorUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdPedido", "IdProduto");

                    b.HasIndex("IdProduto");

                    b.ToTable("ItensPedido");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataHoraPedido")
                        .HasColumnType("TEXT");

                    b.Property<int>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Situacao")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdPedido");

                    b.HasIndex("IdCliente");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Produto", b =>
                {
                    b.Property<int>("IdProduto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<int?>("Estoque")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Preco")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdProduto");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Visitado", b =>
                {
                    b.Property<int>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("TEXT");

                    b.HasKey("IdCliente", "IdProduto");

                    b.HasIndex("IdProduto");

                    b.ToTable("Visitados");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Cliente", b =>
                {
                    b.OwnsOne("AspNetCoreWebApp.Models.Endereco", "Endereco", b1 =>
                        {
                            b1.Property<int>("ClienteIdCliente")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Bairro")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("TEXT");

                            b1.Property<string>("CEP")
                                .IsRequired()
                                .HasMaxLength(8)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Cidade")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Complemento")
                                .HasMaxLength(100)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Estado")
                                .IsRequired()
                                .HasMaxLength(2)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Logradouro")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Numero")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Referencia")
                                .HasMaxLength(100)
                                .HasColumnType("TEXT");

                            b1.HasKey("ClienteIdCliente");

                            b1.ToTable("Clientes");

                            b1.WithOwner()
                                .HasForeignKey("ClienteIdCliente");
                        });

                    b.Navigation("Endereco");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Favorito", b =>
                {
                    b.HasOne("AspNetCoreWebApp.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AspNetCoreWebApp.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("IdProduto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.ItemPedido", b =>
                {
                    b.HasOne("AspNetCoreWebApp.Models.Pedido", "Pedido")
                        .WithMany("ItemPedido")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AspNetCoreWebApp.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("IdProduto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Pedido", b =>
                {
                    b.HasOne("AspNetCoreWebApp.Models.Cliente", "Cliente")
                        .WithMany("Pedidos")
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("AspNetCoreWebApp.Models.Endereco", "Endereco", b1 =>
                        {
                            b1.Property<int>("PedidoIdPedido")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Bairro")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("TEXT");

                            b1.Property<string>("CEP")
                                .IsRequired()
                                .HasMaxLength(8)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Cidade")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Complemento")
                                .HasMaxLength(100)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Estado")
                                .IsRequired()
                                .HasMaxLength(2)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Logradouro")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Numero")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("TEXT");

                            b1.Property<string>("Referencia")
                                .HasMaxLength(100)
                                .HasColumnType("TEXT");

                            b1.HasKey("PedidoIdPedido");

                            b1.ToTable("Pedidos");

                            b1.WithOwner()
                                .HasForeignKey("PedidoIdPedido");
                        });

                    b.Navigation("Cliente");

                    b.Navigation("Endereco");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Visitado", b =>
                {
                    b.HasOne("AspNetCoreWebApp.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AspNetCoreWebApp.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("IdProduto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Cliente", b =>
                {
                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("AspNetCoreWebApp.Models.Pedido", b =>
                {
                    b.Navigation("ItemPedido");
                });
#pragma warning restore 612, 618
        }
    }
}
