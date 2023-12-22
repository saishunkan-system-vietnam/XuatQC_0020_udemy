using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Entities
{
    public class PersonDBContext: DbContext
    {
        public PersonDBContext(DbContextOptions options): base(options)
        {
            
        }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<Person>().ToTable("Person");

            // Seed data to countries
            string countriesJson = File.ReadAllText("countries.json");

            List<Country> countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            // Seed data to persons
            string personsJson = File.ReadAllText("persons.json");

            List<Person> persons = JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person perrson in persons)
            {
                modelBuilder.Entity<Person>().HasData(perrson);
            }
        }

    }
}
