using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreWebApp.Models
{
    public class ItemPedido
    {
        [Required]
        public int IdPedido { get; set; }

        [Required]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        public float Quantidade { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Valor Unitário")]
        public decimal ValorUnitario { get; set; }

        [ForeignKey("IdPedido")]
        public Pedido Pedido { get; set; }

        [ForeignKey("IdProduto")]
        public Produto Produto { get; set; }
    }
}
