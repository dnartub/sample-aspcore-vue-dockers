using Microsoft.EntityFrameworkCore;
using MsSqlDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlDatabase.Context
{
    /// <summary>
    /// Контекст данных
    /// </summary>
    public class SvContext:DbContext
    {
        public SvContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Источники
        /// </summary>
        public DbSet<Source> Sources { get; set; }
        /// <summary>
        /// Вакансии
        /// </summary>
        public DbSet<Vacancy> Vacancies { get; set; }
    }
}
