using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuitandaOnline.Data;
using QuitandaOnline.Models;
using QuitandaOnline.Services;

namespace QuitandaOnline.Pages
{
    [Authorize(Roles = "cliente")]
    public class FinalizarPedidoModel : PageModel
    {
        private QuitandaOnlineContext _context;
        private IEmailSender _emailSender;

        public string COOKIE_NAME
        {
            get { return ".AspNetCore.CartId"; }
        }

        public Pedido Pedido { get; set; }

        public Cliente Cliente { get; set; }

        public FinalizarPedidoModel(QuitandaOnlineContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.Cookies.ContainsKey(COOKIE_NAME))
            {
                var cartId = Request.Cookies[COOKIE_NAME];

                Pedido = await _context.Pedidos.Include("ItensPedido").
                    Include("ItensPedido.Produto").FirstOrDefaultAsync(p => p.IdCarrinho == cartId);

                Cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == User.Identity.Name);

                if ((Pedido.IdCliente > 0) && (Pedido.Endereco != null))
                {
                    Pedido.Situacao = Pedido.SituacaoPedido.Realizado;
                    Pedido.DataHoraPedido = DateTime.UtcNow;
                    foreach (var item in Pedido.ItensPedido)
                    {
                        item.Produto.Estoque -= (int)item.Quantidade;
                    }
                    await _context.SaveChangesAsync();
                    Response.Cookies.Delete(COOKIE_NAME);
                    await EnviarEmailResumoPedido();
                    return Page();
                }
                else
                {
                    return RedirectToPage("/ConfirmarPedido");
                }
            }

            return RedirectToPage("/Carrinho");
        }

        private async Task EnviarEmailResumoPedido()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<p>Olá, {Cliente.Nome}.<p>");
            sb.Append($"<p>Recebemos seu pedido de compra número {Pedido.IdPedido.ToString("D6")}, no valor total de <b>{Pedido.ValorTotal.ToString("C")}</b>, conforme detalhamento a seguir:<p>");
            sb.Append($"<table border='1'><tr><th>Produto</th><th>Qtde.</th><th>R$ Unit.</th><th>R$ Item</th></tr>");
            foreach (var item in Pedido.ItensPedido)
            {
                sb.Append($"<tr><td>{item.Produto.Nome}</td><td>{item.Quantidade}</td><td>" +
                    $"{item.ValorUnitario.ToString("F2")}</td><td>{item.ValorItem.ToString("F2")}</td></tr>");
            }
            sb.Append($"<tr><td colspan='3'>Valor Total</td><td>{Pedido.ValorTotal.ToString("C")}</td></tr></table>");
            sb.Append($"<p>Em breve seus produtos serão entregues no endereço a seguir:</p>");
            sb.Append($"<p>{Pedido.Endereco.Logradouro}, {Pedido.Endereco.Numero}, {Pedido.Endereco.Complemento}<br>" +
                $"Bairro {Pedido.Endereco.Bairro}<br>{Pedido.Endereco.Cidade}/{Pedido.Endereco.Estado}<br>" +
                $"CEP: {Pedido.Endereco.CEP.Insert(5, "-").Insert(2, ".")}</p>");
            sb.Append($"<p>Agradecemos pela confiança em nosso trabalho!</p>");
            sb.Append($"<p>Att.,<br>Equipe Quitanda Online</p>");
            try
            {
                await _emailSender.SendEmailAsync(Cliente.Email, $"Pedido {Pedido.IdPedido.ToString("D6")}",
                    "", sb.ToString());
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Erro ao enviar e-mail: {e.Message}");
            }
        }
    }
}