﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoIPC22011903872.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OthelloEntities : DbContext
    {
        public OthelloEntities()
            : base("name=OthelloEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BLOQUEO> BLOQUEO { get; set; }
        public virtual DbSet<MOVIMIENTO> MOVIMIENTO { get; set; }
        public virtual DbSet<PAIS> PAIS { get; set; }
        public virtual DbSet<PARTIDA> PARTIDA { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TORNEO> TORNEO { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
    }
}
