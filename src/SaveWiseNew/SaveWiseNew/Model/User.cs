using FluentMigrator.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace SaveWise.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите свое имя")]
        [StringLength(100, ErrorMessage = "Привышен лимит символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите свое возраст")]
        [Range(1,150, ErrorMessage = "Некорректный возраст")]
        public int Age { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
