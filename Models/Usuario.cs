using System.ComponentModel.DataAnnotations;

namespace AgendaConsultas.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public ICollection<Consulta> Consultas { get; set; }
            = new List<Consulta>();
    }
}