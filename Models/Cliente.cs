using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuitandaOnline.Models
{
    public class Cliente
    {
        public enum SituacaoCliente
        {
            Bloqueado,
            Cadastrado,
            Aprovado,
            Especial
        }


        [Key]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [DataType(DataType.Date, ErrorMessage = "O campo \"{0}\" dever conter uma data válida.")]
        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        [MaxLength(11, ErrorMessage = "O campo \"{0}\" deve conter no máximo {1} caracteres.")]
        [MinLength(11, ErrorMessage = "O campo \"{0}\" deve conter no mínimo {1} caracteres.")]
        [RegularExpression(@"[0-9]{11}$", ErrorMessage = "O campo \"{0}\" deve ser preenchido com {1} dígitos númericos.")]
        [UIHint("_CpfTemplate")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        [MaxLength(11, ErrorMessage = "O campo \"{0}\" deve conter no máximo {1} caracteres.")]
        [MinLength(11, ErrorMessage = "O campo \"{0}\" deve conter no mínimo {1} caracteres.")]
        [RegularExpression(@"([0-9]){11}$", ErrorMessage = "O campo \"{0}\" deve ser preenchido com {1} dígitos númericos.")]
        [UIHint("_TelefoneTemplate")]
        public string Telefone { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo \"{0}\" deve conter um endereço de e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        [DisplayName("Situação")]
        public SituacaoCliente Situacao { get; set; }

        public Endereco Endereco { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
    }
}
