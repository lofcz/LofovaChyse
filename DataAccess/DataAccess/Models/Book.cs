using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class Book : IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Nazev knihy je vyžadován")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Autor knihy je vyžadován")]
        public virtual string Author { get; set; }

        [Required(ErrorMessage = "Rok vydání je vyžadován")]
        [Range(0, 2100, ErrorMessage = "Rok musí být v rozsahu 0 - 2020")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Datum vydání musí být číslo")]
        public virtual int PublishedYear { get; set; }

        [AllowHtml] // XSS díra!
        public virtual string Description { get; set; }

        public virtual string ImageName { get; set; }

        public virtual BookCategory Category { get; set; }
        public virtual KnihovnaKategorie Kategorie { get; set; }
        public virtual KnihovnaUser OwnerId { get; set; }


        // Přejít na summernote

    }
}
