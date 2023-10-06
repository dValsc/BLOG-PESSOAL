using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLOGPESSOAL.Model
{
    public class Postagem : Auditable
    {
        [Key] //Primary Key (Id)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //IDENTITY(1,1)
        public long Id { get; set; }

        [Column(TypeName = "Varchar")] 
        [StringLength(100)]
        public String Titulo { get; set; } = string.Empty;

        [Column(TypeName = "Varchar")] 
        [StringLength(1000)]
        public String Texto { get; set; } = string.Empty;

        public virtual Tema? Tema { get; set; }
        public virtual User? Usuario { get; set; }

    }
}
